using AutoMapper;
using CAFFAdapterClient.DataTransferObjects.Account;
using CAFFAdapterClient.Domain;
using CAFFAdapterClient.Framework.Providers;
using CAFFAdapterClient.Infrastructure.Constants;
using CAFFAdapterClient.Infrastructure.Data;
using CAFFAdapterClient.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CAFFAdapterClient.Services
{
    public class UserService : IUserService
    {
        private readonly AppModelDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(
            AppModelDbContext dbContext,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = mapper;
        }
       
        public async Task EditUserAsync(int userId, JsonPatchDocument<EditUserDto> editUserDto)
        {
            var user = await CheckUserId(userId);

            var fname = editUserDto.Operations.FirstOrDefault(o => o.path == "firstname");
            Console.WriteLine(fname.value.ToString());

            var userJson = _mapper.Map<JsonPatchDocument<User>>(editUserDto);
            userJson.ApplyTo(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            var user = await CheckUserId(userId);

            var identityResult = await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);

            if (!identityResult.Succeeded)
            {
                throw new InvalidPasswordException("Password is invalid.");
            }           
        }

        private async Task<User> CheckUserId(int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);

            if (user == null)
            {
                throw new DataNotFoundException("User not found with the given Id.");
            }

            return user;
        }
    }
}