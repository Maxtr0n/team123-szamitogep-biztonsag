using CAFFAdapterClient.DataTransferObjects.Account;
using CAFFAdapterClient.ViewModels.Account;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;

namespace CAFFAdapterClient.Services
{
    public interface IUserService
    {
        Task<GetUserInfoViewModel> GetUserInfoAsync(int id);
        Task EditUserAsync(int userId, JsonPatchDocument<EditUserDto> editUserDto);
        Task ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
    }
}