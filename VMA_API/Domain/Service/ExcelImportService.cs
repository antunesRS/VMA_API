using VMA_API.Domain.Helper;
using VMA_API.Domain.Service.Interface;
using VMA_API.Infra.DataAcess.Repository;

namespace VMA_API.Domain.Service
{
    public class ExcelImportService(IExcelImportRepository repository) : IExcelImportService
    {
        private readonly IExcelImportRepository _repository = repository;

        public void SaveImport(IFormFile file)
        {
            var dataTable = ExcelImportHelper.ProcessExcelFile(file);
            _repository.BulkInsertToDatabase(dataTable, "InfoValidacao");
        }
    }
}
