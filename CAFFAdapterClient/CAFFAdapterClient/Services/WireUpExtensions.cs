using Microsoft.Extensions.DependencyInjection;

namespace CAFFAdapterClient.Services
{
    public static class WireUpExtensions
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(WireUpExtensions));

            services.AddScoped<IAccountService, AccountService>();

            return services;
        }
    }
}
