using CAFFAdapterClient.DataTransferObjects.Account;
using CAFFAdapterClient.Infrastructure.Constants;
using CAFFAdapterClient.Services;
using CAFFAdapterClient.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CAFFAdapterClient.Controllers
{
    [Authorize]
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly int userId;

        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;           
            userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(AppClaimTypes.UserId).Value);
        }

        [HttpGet("getprofiledata")]
        public async Task<ActionResult<GetUserInfoViewModel>> GetUserInfo()
        {
            var result = await _userService.GetUserInfoAsync(userId);
            return Ok(result);
        }

        [HttpPatch("editprofile")]
        public async Task<IActionResult> EditUser([FromBody] JsonPatchDocument<EditUserDto> editUserDto)
        {            
            await _userService.EditUserAsync(userId, editUserDto);
            return Ok();
        }

        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            await _userService.ChangePasswordAsync(userId, changePasswordDto);
            return Ok();
        }

        [HttpDelete("deleteComment/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            await _userService.DeleteComment(commentId);
            return Ok();
        }
    }
}