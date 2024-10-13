using System.Data;
using VMA_API.Infra.DataAcess.Connection;

namespace VMA_API.Infra.DataAcess.Repository
{
    public class ExcelImportRepository(DbSession session) : IExcelImportRepository
    {
        private readonly DbSession _session = session;

        public void BulkInsertToDatabase(DataTable dataTable, string tableName)
        {
            _session.Connection.DestinationTableName = tableName;
            _session.Connection.WriteToServer(dataTable);
        }
    }
}
