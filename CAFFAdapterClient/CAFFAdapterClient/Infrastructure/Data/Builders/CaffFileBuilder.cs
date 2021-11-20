using CAFFAdapterClient.Domain;
using Microsoft.EntityFrameworkCore;

namespace CAFFAdapterClient.Infrastructure.Data.Builders
{
    internal class CaffFileBuilder : EntityBaseBuilder<CaffFile>
    {
        public CaffFileBuilder(ModelBuilder builder) : base(builder)
        {
            //entityBuilder.Property(i => i.Name)
            //    .HasMaxLength(255)
            //    .IsRequired();
        }
    }
}
