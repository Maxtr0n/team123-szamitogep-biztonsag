﻿using CAFFAdapterClient.DataTransferObjects.CaffFiles;
using CAFFAdapterClient.Extensions;
using CAFFAdapterClient.Infrastructure.Constants;
using CAFFAdapterClient.Services;
using CAFFAdapterClient.ViewModels;
using CAFFAdapterClient.ViewModels.CaffFiles;
using CAFFAdapterClient.ViewModels.Comments;
using CAFFAdapterClient.ViewModels.GifFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly int userId;

        public CaffFilesController(ICaffFilesServices caffFilesServices, IHttpContextAccessor httpContextAccessor)
        {
            _caffFilesServices = caffFilesServices;
            userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(AppClaimTypes.UserId).Value);
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

        /* Kellene UI-ra:
         * 1. User szinten getGifsByUser: példa Json (createdDate alapján rendezve (legutóbbi az első))
         * [
         *      {
         *          gifId: "1",
         *          gifDescription: "vmi leírás ide 1",
         *          gifBase64: "data:image/gif;base64,..."
         *      },
         *      {
         *          gifId: "2",
         *          gifDescription: "vmi leírás ide 2",
         *          gifBase64: "data:image/gif;base64,..."
         *      },
         *      ..
         * ]
         * 
         * 2. Ugyan ez, csak nem kell user-re szűrni -> getAllGifs
         * 
         * 3. Get Comments By Gif Id:
         * [
         *      {
         *          commentId: "1",
         *          userId" "1",
         *          username: "Joseph King",
         *          comment: "Valami comment 1"
         *      },
         *      {
         *          commentId: "2",
         *          userId" "1",
         *          username: "Joseph King",
         *          comment: "Valami comment 2"
         *      },
         *      {
         *          commentId: "3",
         *          userId" "2",
         *          username: "Davy Jones",
         *          comment: "Valami comment 3"
         *      },
         *      ...
         * ]
         */

        //1.
        [HttpGet("/getGifsByUserId")]
        public async Task<ActionResult<TableViewModel<GifViewModel>>> getGifsByUserId()
        {
            var result = await _caffFilesServices.getGifs(true);
            return Ok(result);
        }

        //2.
        [HttpGet("/getAllGifs")]
        public async Task<ActionResult<TableViewModel<GifViewModel>>> getAllGifs()
        {
            var result = await _caffFilesServices.getGifs(false);
            return Ok(result);
        }

        //3.
        [HttpGet("getCommentsByGifId/{gifId}")]
        public async Task<ActionResult<TableViewModel<CommentByGifViewModel>>> getCommentsByGifId(int gifId)
        {
            var result = await _caffFilesServices.getCommentsByGifId(gifId);
            return Ok(result);
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

        [HttpPost("preview")]
        public async Task<IActionResult> PreviewAsync(CreateCaffFileDto dto)
        {
            var result = await _caffFilesServices.PreviewAsync(dto);
            return result.ToFile("image/gif");
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
