using System.Data;

namespace VMA_API.Infra.DataAcess.Repository
{
    public interface IExcelImportRepository
    {
        void BulkInsertToDatabase(DataTable dataTable, string tableName);
    }
}
