using CAFFAdapterClient.Domain;
using CAFFAdapterClient.Domain.Enums;
using CAFFAdapterClient.Infrastructure.Data;
using CAFFAdapterClient.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CAFFAdapterClient.Services.Implementations
{
    public class DataSeedService : IDataSeedService
    {
        private readonly AppModelDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly ICaffFilesServices _caffFilesServices;

        public DataSeedService(
            AppModelDbContext dbContext,
            UserManager<User> userManager,
            ICaffFilesServices caffFilesServices)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _caffFilesServices = caffFilesServices;
        }

        public async Task SeedAsync()
        {
            var adminResult = await _userManager.CreateAsync(new User()
            {
                Role = UserRoles.Admin,
                Email = "admin@example.com",
                UserName = "admin",
                FirstName = "John",
                LastName = "Admin",
            }, "Admin123.");

            var userResult = await _userManager.CreateAsync(new User()
            {
                Role = UserRoles.Standard,
                Email = "test@test.com",
                UserName = "user",
                FirstName = "Joseph",
                LastName = "McAllsiter",
            }, "Username9!");

            for (int i = 0; i < 10; i++)
            {
                var caffId = await _caffFilesServices.CreateAsync(new DataTransferObjects.CaffFiles.CreateCaffFileDto()
                {
                    File = null                    
                });

                for (int j = 0; j < 3; j++)
                {
                    await _caffFilesServices.AddCommentAsync(caffId, new DataTransferObjects.CaffFiles.AddComment()
                    {
                        Message = $"#{j} example comment"
                    });
                }
            }
        }
    }
}
