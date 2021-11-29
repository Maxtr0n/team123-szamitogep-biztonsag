using CAFFAdapterClient.DataTransferObjects.CaffFiles;
using CAFFAdapterClient.DataTransferObjects.Comment;
using CAFFAdapterClient.Domain;
using CAFFAdapterClient.Domain.Enums;
using CAFFAdapterClient.Infrastructure.Data;
using CAFFAdapterClient.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var dbUsers = _dbContext.Users.Where(x => x.Role == UserRoles.Standard).ToList();
            var dbAdmins = _dbContext.Users.Where(x => x.Role == UserRoles.Admin).ToList();

            if (dbUsers.Count == 0)
            {
                await createUsers();
                dbUsers = _dbContext.Users.Where(x => x.Role == UserRoles.Standard).ToList();
            }                        

            var dbCaffs = _dbContext.CaffFiles.Select(x => x).ToList();

            if (dbCaffs.Count == 0)
            {
                await createCaffs(dbUsers);
                dbCaffs = _dbContext.CaffFiles.Select(x => x).ToList();
            }

            var dbComments = _dbContext.Comments.Select(x => x).ToList();

            if (dbComments.Count == 0)
            {
                await createComments(dbUsers, dbCaffs);
            }            
        }        

        private async Task createUsers()
        {
            var users = new List<User>();
            var admins = new List<User>();

            for (int i = 0; i < 10; i++)
            {
                var user = createUser(i);
                Console.WriteLine(user.Id);
                await generateUser(user);
                users.Add(user);
            }

            for (int i = 0; i < 10; i++)
            {
                var admin = createAdmin(i);
                Console.WriteLine(admin.Id);
                await generateAdmin(admin);
                admins.Add(admin);
            }
        }

        private async Task createCaffs(List<User> users)
        {
            var caff1File = getCaff("1.caff");
            var gif1File = getGif("testingv1.gif");
            var gifWeb = new System.Net.WebClient().DownloadData("https://c.tenor.com/eFPFHSN4rJ8AAAAd/example.gif");

            List<int> userIds = getUserIds(users);

            for (int i = 0; i < 10; i++)
            {
                var caff = new CreateCaffSeedDto();
                caff.Caff = caff1File;
                caff.Description = Faker.Lorem.Sentence();

                if (i < 5)
                {
                    caff.UserId = users.First().Id;
                }
                else
                {
                    var random = new Random();
                    var idx = random.Next(0, userIds.Count);
                    var id = userIds[idx];

                    caff.UserId = id;
                }

                if (i % 2 == 0)
                {
                    caff.Gif = gif1File;
                }
                else
                {
                    caff.Gif = gifWeb;
                }
                await _caffFilesServices.createCaffForSeed(caff);                                
            }
        }

        private async Task createComments(List<User> users, List<CaffFile> caffs)
        {
            List<int> userIds = getUserIds(users);

            foreach (var caff in caffs)
            {
                var random = new Random();
                var limit = random.Next(0, 10);

                for (int j = 0; j < limit; j++)
                {
                    var randomUser = new Random();
                    var idx = randomUser.Next(0, userIds.Count);
                    var id = userIds[idx];

                    var commentSeed = new CommentSeedDto();
                    commentSeed.CaffId = caff.Id;
                    commentSeed.Message = Faker.Lorem.Sentence();
                    commentSeed.UserId = id;

                    await _caffFilesServices.addCommentForSeed(commentSeed);
                }
            }
        }

        private List<int> getUserIds(List<User> users)
        {
            List<int> userIds = new List<int>();
            foreach (var user in users)
            {
                userIds.Add(user.Id);
            }
            return userIds;
        }

        private User createUser(int idx)
        {
            var lastname = Faker.Name.Last();

            return new User()
            {
                //Id = idx,
                Role = UserRoles.Standard,
                Email = "test" + idx + "@test.com",
                UserName = "user" + idx,
                FirstName = Faker.Name.First(),
                LastName = lastname,                
            };
        }

        private User createAdmin(int idx)
        {
            var lastname = Faker.Name.Last();

            return new User()
            {
                //Id = idx,
                Role = UserRoles.Admin,
                Email = "admin" + idx + "@example.com",
                UserName = "admin" + idx,
                FirstName = Faker.Name.First(),
                LastName = lastname                
            };
        }

        private async Task generateUser(User user)
        {
            await _userManager.CreateAsync(user, "Username9!");
        }

        private async Task generateAdmin(User admin)
        {
            await _userManager.CreateAsync(admin, "Admin123.");            
        }

        private byte[] getCaff(string fileName)
        {
            var caffAdapterClientPath1 = Directory.GetCurrentDirectory();
            var caffAdapterClientPath2 = Directory.GetParent(caffAdapterClientPath1).FullName;
            var gitBasePath = Directory.GetParent(caffAdapterClientPath2).FullName;

            var caffFolder = gitBasePath + "\\parser\\examples";
            var caff = caffFolder + "\\" + fileName;
            
            return File.ReadAllBytes(caff);            
        }

        private byte[] getGif(string fileName)
        {
            var caffAdapterClientPath1 = Directory.GetCurrentDirectory();
            var caffAdapterClientPath2 = Directory.GetParent(caffAdapterClientPath1).FullName;
            var gitBasePath = Directory.GetParent(caffAdapterClientPath2).FullName;

            var gifFolder = gitBasePath + "\\parser\\testing";
            var gif = gifFolder + "\\" + fileName;

            return File.ReadAllBytes(gif);
        }
    }
}
