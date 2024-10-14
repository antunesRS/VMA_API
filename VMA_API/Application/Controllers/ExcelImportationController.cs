using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;
using VMA_API.Domain.Model;

namespace VMA_API.Application.Controllers
{
    [Route("api/importacao")]
    [ApiController]
    public class ExcelImportationController(Channel<ExcelInfo> channel) : ControllerBase
    {
        private readonly Channel<ExcelInfo> _channel = channel;

        [HttpPost]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            await _channel.Writer.WriteAsync(new ExcelInfo(file));
            return Ok("Planilha enviada para processamento.");
        }
    }
}
