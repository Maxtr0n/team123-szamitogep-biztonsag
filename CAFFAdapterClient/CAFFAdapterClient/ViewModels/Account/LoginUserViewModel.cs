using CAFFAdapterClient.Domain.Enums;

namespace CAFFAdapterClient.ViewModels.Account
{
    public class LoginUserViewModel
    {
        public bool IsSuccess { get; set; }
        public UserRoles Role { get; set; }
        public string Token { get; set; }
    }
}
