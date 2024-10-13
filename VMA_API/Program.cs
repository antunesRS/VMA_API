using System.Threading.Channels;
using VMA_API.Application.Middleware;
using VMA_API.Application.Workers;
using VMA_API.Domain.Model;
using VMA_API.Domain.Service;
using VMA_API.Domain.Service.Interface;
using VMA_API.Infra.DataAcess.Connection;
using VMA_API.Infra.DataAcess.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IExcelImportService, ExcelImportService>();
builder.Services.AddTransient<IExcelImportRepository, ExcelImportRepository>();

// Adiciona o HostedService (Worker)
builder.Services.AddHostedService<ImportWorker>();

// Adiciona o Channel para comunicação
builder.Services.AddSingleton(Channel.CreateUnbounded<ExcelInfo>());

builder.Services.AddTransient<DbSession>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandler>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
