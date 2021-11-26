using CAFFAdapterClient.Domain.Enums;

namespace CAFFAdapterClient.Framework.Providers
{
    public interface IUserProvider
    {
        int? GetUserId();
        UserRoles? GetUserRole();
    }
}
