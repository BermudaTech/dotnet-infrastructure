using Bermuda.Core.Database.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bermuda.Infrastructure.Database.EF.Mapping
{
    public abstract class EntityBaseTypeConfiguration<TEntity, PKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : EntityBase<PKey>
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Guid);
            builder.Property<StatusType>(x => x.StatusType);

            EntityConfigure(builder);
        }

        public abstract void EntityConfigure(EntityTypeBuilder<TEntity> builder);
    }
}
