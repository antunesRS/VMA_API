﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;
using VMA_API.Domain.Model;
using VMA_API.Domain.Service.Interface;

namespace VMA_API.Application.Controllers
{
    [Route("api/importacao")]
    [ApiController]
    public class ExcelImportationController(Channel<ExcelInfo> channel) : ControllerBase
    {
        //private readonly IExcelImportService _service = service;
        private readonly Channel<ExcelInfo> _channel = channel;

        [HttpPost]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            await _channel.Writer.WriteAsync(new ExcelInfo(file));
            return Ok("Planilha enviada para processamento.");
        }
    }
}
