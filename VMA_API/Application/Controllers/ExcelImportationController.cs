using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VMA_API.Domain.Service.Interface;

namespace VMA_API.Application.Controllers
{
    [Route("api/importacao")]
    [ApiController]
    public class ExcelImportationController(IExcelImportService service) : ControllerBase
    {
        private readonly IExcelImportService _service = service;

        [HttpPost]
        public IActionResult ImportExcel(IFormFile file)
        {
            _service.SaveImport(file);
            return Created();
        }
    }
}
