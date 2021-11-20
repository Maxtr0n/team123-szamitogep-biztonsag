using CAFFAdapterClient.DataTransferObjects.CaffFiles;
using CAFFAdapterClient.ViewModels;
using CAFFAdapterClient.ViewModels.CaffFiles;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CAFFAdapterClient.Services
{
    public interface ICaffFilesServices
    {
        Task<TableViewModel<CaffFileRowViewModel>> GetAsync();
        Task<ActionResult<CaffFileViewModel>> GetByIdAsync(int id);
        Task DeleteByIdAsync(int id);
        Task CreateAsync(CreateCaffFileDto dto);
        Task UpdateAsync(int id, UpdateCaffFileDto dto);
        Task<byte[]> GetPreviewByIdAsync(int id);
        Task<byte[]> GetFileByIdAsync(int id);
    }
}
