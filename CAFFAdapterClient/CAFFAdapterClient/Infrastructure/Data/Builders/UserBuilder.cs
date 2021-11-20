using CAFFAdapterClient.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CAFFAdapterClient.Infrastructure.Data.Builders
{
    internal class UserBuilder
    {
        protected EntityTypeBuilder<User> entityBuilder;

        public UserBuilder(ModelBuilder builder)
        {
            entityBuilder = builder.Entity<User>();

            entityBuilder
                .HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
