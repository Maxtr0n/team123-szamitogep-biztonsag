using CAFFAdapterClient.Domain;
using CAFFAdapterClient.Domain.Enums;
using CAFFAdapterClient.Infrastructure.Data;
using CAFFAdapterClient.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CAFFAdapterClient.Services.Implementations
{
    public class DataseedService : IDataseedService
    {
        private readonly AppModelDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public DataseedService(
            AppModelDbContext dbContext,
            UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            var identityResult = await _userManager.CreateAsync(new User()
            {
                Role = UserRoles.Admin,
                Email = "admin@example.com",
                UserName = "admin",
            }, "Admin123.");
        }
    }
}
