using CAFFAdapterClient.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CAFFAdapterClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataseedController : ControllerBase
    {
        private readonly IDataseedService _dataseedService;

        public DataseedController(IDataseedService dataseedService)
        {
            _dataseedService = dataseedService;
        }

        [HttpPost]
        public async Task<IActionResult> Seed()
        {
            await _dataseedService.SeedAsync();
            return Ok();
        }
    }
}
