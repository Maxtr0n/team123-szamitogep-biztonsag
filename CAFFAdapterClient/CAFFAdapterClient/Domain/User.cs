using CAFFAdapterClient.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace CAFFAdapterClient.Domain
{
    public class User : IdentityUser<int>
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public UserRoles Role { get; set; }
    }
}
