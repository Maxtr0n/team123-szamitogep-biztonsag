using CAFFAdapterClient.DataTransferObjects.CaffFiles;
using CAFFAdapterClient.Services;
using CAFFAdapterClient.ViewModels;
using CAFFAdapterClient.ViewModels.CaffFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CaffFileViewModel>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}/preview")]
        public async Task<FileStreamResult> PreviewByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}/download")]
        public async Task<FileStreamResult> DownloadByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateCaffFileDto dto)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdateCaffFileDto dto)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
