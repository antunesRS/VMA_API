using System.IO;

namespace VMA_API.Domain.Model
{
    public class ExcelInfo
    {
        public MemoryStream Stream { get; set; } = new MemoryStream();
        public string FileName { get; set; }
        public string Id { get; set; } = string.Empty;

        public ExcelInfo(IFormFile file, string id)
        {
            file.CopyTo(Stream);
            FileName = file.FileName.Replace(".xlsx","");
            Id = id;
        }
    }
}
