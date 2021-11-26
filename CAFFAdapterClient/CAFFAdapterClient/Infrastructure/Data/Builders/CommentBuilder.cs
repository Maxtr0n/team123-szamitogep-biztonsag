using CAFFAdapterClient.Domain;
using Microsoft.EntityFrameworkCore;

namespace CAFFAdapterClient.Infrastructure.Data.Builders
{
    internal class CommentBuilder : EntityBaseBuilder<Comment>
    {
        public CommentBuilder(ModelBuilder builder) : base(builder)
        {
            entityBuilder.HasOne(i => i.Caff);
            entityBuilder.HasOne(i => i.User);
        }
    }
}
