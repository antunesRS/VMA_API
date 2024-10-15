using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.ResponseCompression;
using System.Threading.Channels;
using VMA_API.Application.Middleware;
using VMA_API.Application.Workers;
using VMA_API.Application.Hubs;
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
//builder.Services.AddTransient<IHubContext<ImportHub>, ImportHub>();

// Adiciona o HostedService (Worker)
builder.Services.AddHostedService<ImportWorker>();

// Adiciona o Channel para comunica��o
builder.Services.AddSingleton(Channel.CreateUnbounded<ExcelInfo>());
builder.Services.AddTransient<DbSession>();

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});


var app = builder.Build();

app.UseResponseCompression();

app.UseCors("AllowAll");

app.UseRouting();

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

app.MapHub<ImportHub>("/hub");

app.Run();
