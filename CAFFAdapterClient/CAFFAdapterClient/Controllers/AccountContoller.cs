using CAFFAdapterClient.DataTransferObjects.Account;
using CAFFAdapterClient.Infrastructure.Constants;
using CAFFAdapterClient.Services;
using CAFFAdapterClient.ViewModels;
using CAFFAdapterClient.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CAFFAdapterClient.Controllers
{
    [Authorize(Policy = AppPolicies.Administrator)]
    [ApiController]
    [Route("account")]
    public class AccountContoller : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountContoller(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            await _accountService.RegisterUserAsync(registerUserDto);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginUserViewModel>> Login([FromBody] LoginUserDto loginUserDto)
        {
            var result = await _accountService.LoginUserAsync(loginUserDto);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserInfoViewModel>> GetUserInfo(int id)
        {
            var result = await _accountService.GetUserInfoAsync(id);
            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> EditUser(int id, [FromBody] JsonPatchDocument<EditUserDto> editUserDto)
        {            
            await _accountService.EditUserAsync(id, editUserDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteByIdAsync(int id)
        {
            await _accountService.DeleteByIdAsync(id);
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<TableViewModel<UserRowViewModel>>> GetAsync()
        {
            return await _accountService.GetAsync();
        }

        
        [HttpPost("registerAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterUserDto registerUserDto)
        {
          await _accountService.RegisterAdminAsync(registerUserDto);
          return Ok();
        }

  }
}
