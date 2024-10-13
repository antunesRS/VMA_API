using Microsoft.Data.SqlClient;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;

namespace VMA_API.Domain.Helper
{
    public static class ExcelImportHelper
    {
        public static DataTable ProcessExcelFile(IFormFile file)
        {
            using var stream = new MemoryStream();
            file.CopyTo(stream);
            stream.Position = 0;

            // Carrega o arquivo Excel
            IWorkbook workbook = new XSSFWorkbook(stream);
            ISheet sheet = workbook.GetSheetAt(0);

            int chunkSize = 10000; // Define o tamanho do bloco (chunk)
            int totalRows = sheet.LastRowNum; // Pega o número total de linhas
            DataTable dataTable = new();

            for (int row = 0; row <= totalRows; row += chunkSize)
            {
               
                // Iterar pelas linhas do chunk atual
                for (int currentRow = row; currentRow < Math.Min(row + chunkSize, totalRows + 1); currentRow++)
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
                                if(currentRow == 0)
                                    dataTable.Columns.Add(cell.ToString(), typeof(string));
                                else
                                    dataRow.SetField(col, cell.ToString());
                            }
                        }
                        if (currentRow != 0)
                            dataTable.Rows.Add(dataRow);
                    }

                }

                
            }
            return dataTable;
            // Processa os dados em chunks para salvar no banco de dados
            //int chunkSize = 10000;
            //for (int row = 0; row <= sheet.LastRowNum; row += chunkSize)
            //{
            //    var chunk = sheet.Skip(row).Take(chunkSize);
            //    DataTable dataTable = new DataTable();

            //    // Converte o chunk para DataTable
            //    foreach (var currentRow in chunk)
            //    {
            //        DataRow dataRow = dataTable.NewRow();
            //        for (int col = 0; col < currentRow.LastCellNum; col++)
            //        {
            //            ICell cell = currentRow.GetCell(col);
            //            dataRow[col] = cell.ToString();
            //        }
            //        dataTable.Rows.Add(dataRow);
            //    }

            //    // Bulk insert no banco de dados
            //    BulkInsertToDatabase(dataTable);
            //}
        }
    }
}
