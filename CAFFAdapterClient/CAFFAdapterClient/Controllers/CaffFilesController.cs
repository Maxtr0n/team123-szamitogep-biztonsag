using CAFFAdapterClient.DataTransferObjects.CaffFiles;
using CAFFAdapterClient.Extensions;
using CAFFAdapterClient.Services;
using CAFFAdapterClient.ViewModels;
using CAFFAdapterClient.ViewModels.CaffFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CAFFAdapterClient.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CaffFilesController : ControllerBase
    {
        private readonly ICaffFilesServices _caffFilesServices;

        public CaffFilesController(ICaffFilesServices caffFilesServices)
        {
            _caffFilesServices = caffFilesServices;
        }

        [HttpGet]
        public async Task<ActionResult<TableViewModel<CaffFileRowViewModel>>> Get()
        {
            return await _caffFilesServices.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CaffFileViewModel>> GetByIdAsync(int id)
        {
            return await _caffFilesServices.GetByIdAsync(id);
        }

        [HttpPost("{id}/comments")]
        public async Task<ActionResult<CaffFileViewModel>> CreateCommentAsync(int id, AddComment dto)
        {
            await _caffFilesServices.AddCommentAsync(id, dto);

            return Ok();
        }

        [HttpDelete("{id}/comments/{cid}")]
        public async Task<ActionResult<CaffFileViewModel>> DeleteCommentByIdAsync(int id, int cid)
        {
            await _caffFilesServices.DeleteCommentByIdAsync(id, cid);

            return Ok();
        }

        [HttpGet("{id}/preview")]
        public async Task<FileStreamResult> GetPreviewByIdAsync(int id)
        {
            var result = await _caffFilesServices.GetPreviewByIdAsync(id);
            return result.ToFile("image/gif");
        }

        [HttpGet("{id}/download")]
        public async Task<FileStreamResult> DownloadByIdAsync(int id)
        {
            var result = await _caffFilesServices.GetFileByIdAsync(id);
            return result.ToFile("application/octet-stream");
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(CreateCaffFileDto dto)
        {
            await _caffFilesServices.CreateAsync(dto);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdateCaffFileDto dto)
        {
            await _caffFilesServices.UpdateAsync(id, dto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {
            await _caffFilesServices.DeleteByIdAsync(id);
            return NoContent();
        }
    }
}
