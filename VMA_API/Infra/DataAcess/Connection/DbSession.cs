using Microsoft.Data.SqlClient;
using System.Data;

namespace VMA_API.Infra.DataAcess.Connection
{
    public sealed class DbSession(IConfiguration configuration) : IDisposable
    {
        public SqlBulkCopy Connection { get; } = new SqlBulkCopy(configuration["ConnectionStrings:SqlServer"], SqlBulkCopyOptions.FireTriggers);

        public void Dispose()
        {
            Connection.Close();
      
        }
    }
}
