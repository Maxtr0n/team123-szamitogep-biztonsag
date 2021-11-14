using CAFFAdapterClient.Framework.Providers;
using CAFFAdapterClient.Infrastructure.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace CAFFAdapterClient.Providers
{
    public class UserProvider : IUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? GetUserId()
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User != null)
            {
                var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(i => i.Type == AppClaimTypes.UserId);
                if (userIdClaim == null)
                {
                    return null;
                }

                return Convert.ToInt32(userIdClaim.Value);
            }

            return null;
        }

    }
}
