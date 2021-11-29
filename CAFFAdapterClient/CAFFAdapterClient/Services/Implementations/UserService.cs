using AutoMapper;
using CAFFAdapterClient.DataTransferObjects.Account;
using CAFFAdapterClient.Domain;
using CAFFAdapterClient.Framework.Providers;
using CAFFAdapterClient.Infrastructure.Constants;
using CAFFAdapterClient.Infrastructure.Data;
using CAFFAdapterClient.Infrastructure.Exceptions;
using CAFFAdapterClient.ViewModels.Account;
using CAFFAdapterClient.ViewModels.CaffFiles;
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
        private readonly IUserProvider _userProvider;

        public UserService(
            AppModelDbContext dbContext,
            UserManager<User> userManager,
            IMapper mapper,
            IUserProvider userProvider)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = mapper;
            _userProvider = userProvider;
        }

        public async Task<GetUserInfoViewModel> GetUserInfoAsync(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);

            if (user == null)
            {
                throw new DataNotFoundException("User not found with the given Id.");
            }

            var caffFiles = await _dbContext.CaffFiles
               .AsNoTracking()
               .Where(x => x.UserId == _userProvider.GetUserId())
               .Select(x => new CaffFileRowViewModel
               {
                   Id = x.Id
               })
               .ToListAsync();

            return new GetUserInfoViewModel()
            {
                Firstname = user.FirstName,
                Lastname = user.LastName,
                Email = user.Email,
                CaffFiles = caffFiles
            };
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

        public async Task DeleteComment(int commentId)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == commentId);

            if (comment.UserId != this._userProvider.GetUserId())
            {
                throw new BusinessLogicException("Az adott komment nem törölhetõ!");
            }

            comment.IsDeleted = true;

            _dbContext.Comments.Update(comment);

            await _dbContext.SaveChangesAsync();
        }
    }
}