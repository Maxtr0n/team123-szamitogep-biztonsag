using CAFFAdapterClient.DataTransferObjects.Account;
using CAFFAdapterClient.Services;
using CAFFAdapterClient.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CAFFAdapterClient.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountContoller : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountContoller(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            await _accountService.RegisterUserAsync(registerUserDto);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginUserViewModel>> Login([FromBody] LoginUserDto loginUserDto)
        {
            var result = await _accountService.LoginUserAsync(loginUserDto);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserInfoViewModel>> GetUserInfo(int id)
        {
            var result = await _accountService.GetUserInfoAsync(id);
            return Ok(result);
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> EditUser(int id, [FromBody] JsonPatchDocument<EditUserDto> editUserDto)
        {
            //await _accountService.EditUserAsync(id, editUserDto);
            return Ok();
        }
    }
}
