using CAFFAdapterClient.DataTransferObjects.CaffFiles;
using CAFFAdapterClient.DataTransferObjects.Comment;
using CAFFAdapterClient.ViewModels;
using CAFFAdapterClient.ViewModels.CaffFiles;
using CAFFAdapterClient.ViewModels.Comments;
using CAFFAdapterClient.ViewModels.GifFiles;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CAFFAdapterClient.Services
{
    public interface ICaffFilesServices
    {
        Task<TableViewModel<CaffFileRowViewModel>> GetAsync();
        Task<ActionResult<CaffFileViewModel>> GetByIdAsync(int id);
        Task DeleteByIdAsync(int id);
        Task<int> CreateAsync(CreateCaffFileDto dto);
        Task<byte[]> PreviewAsync(CreateCaffFileDto dto);
        Task UpdateAsync(int id, UpdateCaffFileDto dto);
        Task<byte[]> GetPreviewByIdAsync(int id);
        Task<byte[]> GetFileByIdAsync(int id);
        Task AddCommentAsync(int caddId, AddComment dto);
        Task DeleteCommentByIdAsync(int id, int cid);


        Task<TableViewModel<GifViewModel>> getGifs(bool byUserId);
        Task<TableViewModel<CommentByGifViewModel>> getCommentsByGifId(int gifId);
        Task<int> createCaffForSeed(CreateCaffSeedDto createCaffSeedDto);
        Task addCommentForSeed(CommentSeedDto commentSeedDto);
    }
}
