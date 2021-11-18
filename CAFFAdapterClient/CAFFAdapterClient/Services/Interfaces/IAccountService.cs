using CAFFAdapterClient.DataTransferObjects.Account;
using CAFFAdapterClient.ViewModels.Account;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;

namespace CAFFAdapterClient.Services
{
    public interface IAccountService
    {
        Task RegisterUserAsync(RegisterUserDto registerUserDto);
        Task<LoginUserViewModel> LoginUserAsync(LoginUserDto loginUserDto);
        Task<GetUserInfoViewModel> GetUserInfoAsync(int id);
        Task EditUserAsync(int id, JsonPatchDocument<EditUserDto> editUserDto);
    }
}
