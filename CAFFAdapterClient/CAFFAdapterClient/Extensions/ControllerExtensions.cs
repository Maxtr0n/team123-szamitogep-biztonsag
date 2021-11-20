using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace CAFFAdapterClient.Extensions
{
    public static class ControllerExtensions
    {
        public static FileStreamResult ToFile(this byte[] file)
        {
            using (MemoryStream pdfStream = new MemoryStream())
            {
                pdfStream.Write(file, 0, file.Length);
                pdfStream.Position = 0;
                return new FileStreamResult(pdfStream, "application/pdf");
            }
        }
    }
}
