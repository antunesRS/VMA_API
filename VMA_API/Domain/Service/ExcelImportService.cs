using Microsoft.AspNetCore.SignalR;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using VMA_API.Application.ProgressUpdate;
using VMA_API.Domain.Service.Interface;
using VMA_API.Infra.DataAcess.Repository;

namespace VMA_API.Domain.Service
{
    public class ExcelImportService(IExcelImportRepository repository, 
                                    ILogger<ExcelImportService> logger,
                                    ImportHub hubContext) : IExcelImportService
    {
        private readonly IExcelImportRepository _repository = repository;
        private readonly ILogger<ExcelImportService> _logger = logger;
        private readonly ImportHub _hubContext = hubContext;

        public async void ProcessExcelFile(MemoryStream stream, string tableName) 
        {
            stream.Position = 0;

            // Carrega o arquivo Excel
            XSSFWorkbook workbook = new(stream);
            ISheet sheet = workbook.GetSheetAt(0);

            int chunkSize = 5; // Define o tamanho do bloco (chunk)
            int totalRows = sheet.LastRowNum; // Pega o número total de linhas
            int currentRow = 0;
            int count = 0;
            DataTable dataTable = new();

            while (currentRow <= totalRows)
            {
                // Iterar pelas linhas do chunk atual
                while(count < chunkSize)
                {
                    IRow rowData = sheet.GetRow(currentRow);
                    if (rowData != null)
                    {
                        DataRow dataRow = dataTable.NewRow();
                        // Processar cada célula da linha
                        for (int col = 0; col < rowData.LastCellNum; col++)
                        {
                            ICell cell = rowData.GetCell(col);
                            if (cell != null)
                            {
                                // Processar o valor da célula
                                if (currentRow == 0)
                                    dataTable.Columns.Add(cell.ToString(), typeof(string));
                                else
                                    dataRow.SetField(col, cell.ToString());
                            }
                        }
                        if (currentRow != 0)
                        {
                            dataTable.Rows.Add(dataRow);
                            count++;
                        }
                        currentRow++;
                        int processedRows = Math.Min(currentRow + chunkSize, totalRows);
                        int percentage = (processedRows * 100) / totalRows;

                        await _hubContext.SendProgress(percentage);
                    }
                }
                _repository.BulkInsertToDatabase(dataTable, tableName);
                _logger.LogInformation($"Inseridos {dataTable.Rows.Count} registros na tabela {tableName}");
                dataTable.Rows.Clear();
                count = 0;
            }
            _logger.LogInformation($"Inseridos {totalRows} registros na tabela {tableName} no total.");

        }
    }
    
}
