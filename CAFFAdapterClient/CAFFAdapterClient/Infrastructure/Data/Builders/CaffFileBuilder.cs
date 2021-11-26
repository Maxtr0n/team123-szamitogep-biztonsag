using CAFFAdapterClient.Domain;
using Microsoft.EntityFrameworkCore;

namespace CAFFAdapterClient.Infrastructure.Data.Builders
{
    internal class CaffFileBuilder : EntityBaseBuilder<CaffFile>
    {
        public CaffFileBuilder(ModelBuilder builder) : base(builder)
        {
        }
    }
}
