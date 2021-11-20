using CAFFAdapterClient.Services.Implementations;
using CAFFAdapterClient.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CAFFAdapterClient.Services
{
    public static class WireUpExtensions
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(WireUpExtensions));

            services.AddScoped<IDataseedService, DataseedService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICaffFilesServices, CaffFilesServices>();

            return services;
        }
    }
}
