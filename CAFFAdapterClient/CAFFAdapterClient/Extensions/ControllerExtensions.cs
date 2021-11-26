using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace CAFFAdapterClient.Extensions
{
    public static class ControllerExtensions
    {
        public static FileStreamResult ToFile(this byte[] file, string contentType)
        {
            var stream = new MemoryStream(file);
            return new FileStreamResult(stream, contentType);
        }
    }
}
