using CAFFAdapterClient.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace CAFFAdapterClient.Domain
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string LastName { get; internal set; }
        public UserRoles Role { get; set; }
        public bool IsDeleted { get; set; }
    }
}
