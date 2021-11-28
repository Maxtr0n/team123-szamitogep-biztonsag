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

            var caff1File = getCaff("1.caff");
            var gif1File = getGif("testingv1.gif");
            List<int> caffIds = new List<int>();

            for (int i = 0; i < 10; i++)
            {
                var caff = new CreateCaffSeedDto();
                caff.Caff = caff1File;
                caff.Description = Faker.Lorem.Sentence();

                if (i < 5)
                {
                    caff.UserId = users[0].Id;
                }
                else
                {
                    caff.UserId = users[i].Id;
                }

                if (i % 2 == 0)
                {
                    caff.Gif = gif1File;
                }
                else
                {
                    caff.Gif = new System.Net.WebClient().DownloadData("https://c.tenor.com/eFPFHSN4rJ8AAAAd/example.gif");
                }
                var caffId = await _caffFilesServices.createCaffForSeed(caff);
                caffIds.Add(caffId);

                var random = new Random();
                var limit = random.Next(0, 10);
                for (int j = 0; j < limit; j++)
                {
                    var commentSeed = new CommentSeedDto();
                    commentSeed.CaffId = caffId;
                    commentSeed.Message = Faker.Lorem.Sentence();
                    commentSeed.UserId = users[j].Id;

                    await _caffFilesServices.addCommentForSeed(commentSeed);
                }
            }

            //for (int i = 0; i < 10; i++)
            //{
            //    var caffId = await _caffFilesServices.CreateAsync(new DataTransferObjects.CaffFiles.CreateCaffFileDto()
            //    {
            //        File = caff1File                    
            //    });

            //    for (int j = 0; j < 3; j++)
            //    {
            //        await _caffFilesServices.AddCommentAsync(caffId, new DataTransferObjects.CaffFiles.AddComment()
            //        {
            //            Message = $"#{j} example comment"
            //        });
            //    }
            //}
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
                Lastname = lastname
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
                LastName = lastname,
                Lastname = lastname
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
