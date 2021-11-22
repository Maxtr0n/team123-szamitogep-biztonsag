using AutoMapper;
using CAFFAdapterClient.DataTransferObjects.Account;
using CAFFAdapterClient.Domain;
using CAFFAdapterClient.Domain.Enums;
using CAFFAdapterClient.Framework;
using CAFFAdapterClient.Framework.Providers;
using CAFFAdapterClient.Infrastructure.Constants;
using CAFFAdapterClient.Infrastructure.Data;
using CAFFAdapterClient.Infrastructure.Exceptions;
using CAFFAdapterClient.ViewModels;
using CAFFAdapterClient.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CAFFAdapterClient.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppModelDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IUserProvider _userProvider;
        private readonly IMapper _mapper;

        public AccountService(
            AppModelDbContext dbContext,
            UserManager<User> userManager,
            IUserProvider userProvider,
            IMapper mapper)
        {
            _dbContext = dbContext; 
            _userManager = userManager;
            _userProvider = userProvider;
            _mapper = mapper;
        }  

        public async Task<LoginUserViewModel> LoginUserAsync(LoginUserDto loginUserDto)
        {
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(i => i.Email == loginUserDto.Email);
            if (user == null)
            {
                return new LoginUserViewModel() { IsSuccess = false };
            }

            var isSuccess = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);
            if (!isSuccess)
            {
                return new LoginUserViewModel() { IsSuccess = false };
            }

            var key = new SymmetricSecurityKey(GlobalConfigurations.SecurityKey);

            var claims = new List<Claim>();
            claims.Add(new Claim(AppClaimTypes.UserId, user.Id.ToString()));
            claims.Add(new Claim(AppClaimTypes.FirstName, user.FirstName));
            claims.Add(new Claim(AppClaimTypes.Lastname, user.Lastname));
            claims.Add(new Claim(AppClaimTypes.Email, user.Email));
            claims.Add(new Claim(AppClaimTypes.Role, user.Role.ToString()));

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(GlobalConfigurations.TokenExpiresDays),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginUserViewModel()
            {
                IsSuccess = true,
                Role = user.Role,
                Token = tokenHandler.WriteToken(token)
            };
        }

        public async Task RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            var identityResult = await _userManager.CreateAsync(new User()
            {
                FirstName = registerUserDto.FirstName,
                Lastname = registerUserDto.LastName,
                Role = UserRoles.Standard,
                Email = registerUserDto.Email,
                UserName = Guid.NewGuid().ToString(),
            }, registerUserDto.Password);

            if (!identityResult.Succeeded)
            {
                throw new BusinessLogicException(string.Join(", ", identityResult.Errors.Select(i => i.Description)));
            }
        }

        public async Task<GetUserInfoViewModel> GetUserInfoAsync(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);

            if (user == null)
            {
                throw new DataNotFoundException("User not found with the given Id.");
            }

            return new GetUserInfoViewModel()
            {
                Firstname = user.FirstName,
                Lastname = user.Lastname,
                Email = user.Email
            };
        }

        public async Task EditUserAsync(int id, JsonPatchDocument<EditUserDto> editUserDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);

            if (user == null)
            {
                throw new DataNotFoundException("User not found with the given Id.");
            }
            var fname = editUserDto.Operations.FirstOrDefault(o => o.path == "firstname");
            Console.WriteLine(fname.value.ToString());

            var userJson = _mapper.Map<JsonPatchDocument<User>>(editUserDto);
            userJson.ApplyTo(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new DataNotFoundException();

            user.IsDeleted = true;

            _dbContext.Users.Update(user);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<ActionResult<TableViewModel<UserRowViewModel>>> GetAsync()
        {
            var items = _dbContext.Users
                .AsNoTracking()
                .Select(x => new UserRowViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName
                })
                .ToList();

            var count = items.Count();

            return new TableViewModel<UserRowViewModel>
            {
                Items = items,
                Count = count
            };
        }
    }
}
