using CAFFAdapterClient.Domain;
using CAFFAdapterClient.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CAFFAdapterClient.Test.DataSeeder
{
    public static class CAFFAdapterClientDataSeeder
    {
        public static async Task InitilazieTestUsers(UserManager<User> userManager)
        {
            var adminResult = await userManager.CreateAsync(new User()
            {
                Role = UserRoles.Admin,
                Email = "admin@example.com",
                UserName = "admin",
                FirstName = "John",
                LastName = "Admin",
            }, "Admin123.");

            var userResult = await userManager.CreateAsync(new User()
            {
                Role = UserRoles.Standard,
                Email = "test@test.com",
                UserName = "user",
                FirstName = "Joseph",
                LastName = "McAllsiter",
            }, "Username9!");
        }
    }
}
