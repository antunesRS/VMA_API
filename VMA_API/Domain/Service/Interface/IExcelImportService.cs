﻿namespace VMA_API.Domain.Service.Interface
{
    public interface IExcelImportService
    {
        void ProcessExcelFile(MemoryStream stream, string tableName);
    }
}
