using CAFFAdapterClient.Domain;
using CAFFAdapterClient.Infrastructure.Data.Builders;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CAFFAdapterClient.Infrastructure.Data
{
    public class AppModelDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<CaffFile> CaffFiles { get; set; }

        public AppModelDbContext(DbContextOptions<AppModelDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var cascades = builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(c => !c.IsOwnership && c.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var c in cascades)
            {
                c.DeleteBehavior = DeleteBehavior.Restrict;
            }

            new UserBuilder(builder);
            new CaffFileBuilder(builder);
            // new TodoCategorykBuilder(builder);
        }
    }
}