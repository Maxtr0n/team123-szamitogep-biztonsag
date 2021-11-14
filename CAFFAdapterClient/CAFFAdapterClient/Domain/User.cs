using CAFFAdapterClient.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace CAFFAdapterClient.Domain
{
    public class User : IdentityUser<int>
    {
        public UserRoles Role { get; set; }
    }
}
