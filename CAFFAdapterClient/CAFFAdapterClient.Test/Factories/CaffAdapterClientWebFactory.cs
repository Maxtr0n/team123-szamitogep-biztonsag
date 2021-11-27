using CAFFAdapterClient.Domain;
using CAFFAdapterClient.Infrastructure.Data;
using CAFFAdapterClient.Test.DataSeeder;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace CAFFAdapterClient.Test.Factories
{
    public class CaffAdapterClientWebFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // builder.UseStartup<Startup>();

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppModelDbContext>));

                services.Remove(descriptor);

                var provider = services
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.AddDbContext<AppModelDbContext>(options =>
                {
                    options.UseInMemoryDatabase($"InMemoryCAFFAdapterClient");
                    options.UseInternalServiceProvider(provider);
                });

                var serviceProvider = services.BuildServiceProvider();

                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<AppModelDbContext>();
                    var userManager = scopedServices.GetRequiredService<UserManager<User>>();

                    db.Database.EnsureCreated();

                    CAFFAdapterClientDataSeeder.InitilazieTestUsers(userManager).Wait();
                }
            });

            base.ConfigureWebHost(builder);
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder();
        }
    }
}
