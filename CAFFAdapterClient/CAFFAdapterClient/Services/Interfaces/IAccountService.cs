using CAFFAdapterClient.DataTransferObjects.Account;
using CAFFAdapterClient.ViewModels.Account;
using System.Threading.Tasks;

namespace CAFFAdapterClient.Services
{
    public interface IAccountService
    {
        Task RegisterUserAsync(RegisterUserDto registerUserDto);
        Task<LoginUserViewModel> LoginUserAsync(LoginUserDto loginUserDto);
        Task<GetUserInfoViewModel> GetUserInfoAsync(int id);
    }
}
