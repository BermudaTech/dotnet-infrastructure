using Bermuda.Core.Database.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bermuda.Infrastructure.Database.EF.Mapping
{
    public abstract class EntityBaseWithLogTypeConfiguration<TEntity, PKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : EntityBaseAudit<PKey>
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.TrackId);
            builder.Property<StateType>(x => x.State);
            builder.Property(x => x.InsertedDate);
            builder.Property(x => x.InsertedIp).IsRequired().HasMaxLength(25);
            builder.Property(x => x.InsertedUser).IsRequired().HasMaxLength(25);
            builder.Property(x => x.UpdatedDate);
            builder.Property(x => x.UpdatedIp).HasMaxLength(25);
            builder.Property(x => x.UpdatedUser).HasMaxLength(25);

            EntityConfigure(builder);
        }

        public abstract void EntityConfigure(EntityTypeBuilder<TEntity> builder);
    }
}
