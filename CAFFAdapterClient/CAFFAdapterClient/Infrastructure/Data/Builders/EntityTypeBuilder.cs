using CAFFAdapterClient.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CAFFAdapterClient.Infrastructure.Data.Builders
{
    internal class EntityBaseBuilder<T> where T : EntityBase
    {
        protected EntityTypeBuilder<T> entityBuilder;

        public EntityBaseBuilder(ModelBuilder builder)
        {
            entityBuilder = builder.Entity<T>();

            #region ITypedIdEntity
            entityBuilder
                .Property(nameof(EntityBase.Id))
                .UseIdentityColumn();
            #endregion
        }
    }
}
