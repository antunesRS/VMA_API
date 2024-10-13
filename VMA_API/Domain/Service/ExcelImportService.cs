using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using VMA_API.Domain.Service.Interface;
using VMA_API.Infra.DataAcess.Repository;

namespace VMA_API.Domain.Service
{
    public class ExcelImportService(IExcelImportRepository repository) : IExcelImportService
    {
        private readonly IExcelImportRepository _repository = repository;

        public void SaveImport(IFormFile file)
        {
            ProcessExcelFile(file, "InfoValidacao");
        }

        private void ProcessExcelFile(IFormFile file, string tableName) 
        {
            using var stream = new MemoryStream();
            file.CopyTo(stream);
            stream.Position = 0;

            // Carrega o arquivo Excel
            IWorkbook workbook = new XSSFWorkbook(stream);
            ISheet sheet = workbook.GetSheetAt(0);

            int chunkSize = 5; // Define o tamanho do bloco (chunk)
            int totalRows = sheet.LastRowNum; // Pega o número total de linhas
            int currentRow = 0;
            int count = 0;
            DataTable dataTable = new();

            while(currentRow <= totalRows)
            {
                // Iterar pelas linhas do chunk atual
                while(count < chunkSize)
                {
                    //var teste = Math.Min(currentRow + chunkSize, totalRows + 1);
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
                    }
                }
                _repository.BulkInsertToDatabase(dataTable, tableName);
                dataTable.Rows.Clear();
                count = 0;
            }
           
        }
    }
    
}
