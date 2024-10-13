using VMA_API.Application.Middleware;
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

builder.Services.AddScoped<IExcelImportService, ExcelImportService>();
builder.Services.AddScoped<IExcelImportRepository, ExcelImportRepository>();

builder.Services.AddScoped<DbSession>();

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
