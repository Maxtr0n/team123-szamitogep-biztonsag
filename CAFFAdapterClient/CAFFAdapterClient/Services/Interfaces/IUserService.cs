using CAFFAdapterClient.DataTransferObjects.Account;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;

namespace CAFFAdapterClient.Services
{
    public interface IUserService
    {
        Task EditUserAsync(int userId, JsonPatchDocument<EditUserDto> editUserDto);
        Task ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
    }
}