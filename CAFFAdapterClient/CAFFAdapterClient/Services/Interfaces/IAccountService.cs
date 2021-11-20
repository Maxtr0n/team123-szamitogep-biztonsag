using CAFFAdapterClient.DataTransferObjects.Account;
using CAFFAdapterClient.ViewModels;
using CAFFAdapterClient.ViewModels.Account;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CAFFAdapterClient.Services
{
    public interface IAccountService
    {
        Task RegisterUserAsync(RegisterUserDto registerUserDto);
        Task<LoginUserViewModel> LoginUserAsync(LoginUserDto loginUserDto);
        Task<GetUserInfoViewModel> GetUserInfoAsync(int id);
        Task EditUserAsync(int id, JsonPatchDocument<EditUserDto> editUserDto);
        Task DeleteByIdAsync(int id);
        Task<ActionResult<TableViewModel<UserRowViewModel>>> GetAsync();
    }
}
