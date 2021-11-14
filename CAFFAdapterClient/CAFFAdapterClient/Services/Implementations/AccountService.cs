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
            claims.Add(new Claim(AppClaimTypes.Name, user.UserName));
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
                Token = tokenHandler.WriteToken(token)
            };
        }

        public async Task RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            var identityResult = await _userManager.CreateAsync(new User()
            {
                Role = UserRoles.Standard,
                Email = registerUserDto.Email,
                UserName = Guid.NewGuid().ToString(),
            }, registerUserDto.Password);

            if (!identityResult.Succeeded)
            {
                throw new BusinessLogicException(string.Join(", ", identityResult.Errors.Select(i => i.Description)));
            }
        }
    }
}
