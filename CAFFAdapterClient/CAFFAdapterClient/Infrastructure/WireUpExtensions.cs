using CAFFAdapterClient.Domain;
using CAFFAdapterClient.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CAFFAdapterClient.Infrastructure
{
    public static class WireUpExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {

            services.AddIdentityCore<User>()
             .AddEntityFrameworkStores<AppModelDbContext>()
             .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
            });


            return services;
        }

        public static IServiceCollection AddSqlServer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppModelDbContext>(options => options.UseSqlServer(connectionString));

            return services;
        }

        public static IServiceCollection AddInMemoryDb(this IServiceCollection services)
        {
            services.AddDbContext<AppModelDbContext>(options => options.UseInMemoryDatabase("CAFFAdapterClient"));

            return services;
        }
    }
}
