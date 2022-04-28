#region Usings

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Base.Configuration
{
    public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T>
        where T : BaseEntity, IBaseEntity
    {
        #region BuildEntity

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder
                .HasKey(c => c.Id);

            builder
                .ToTable(typeof(T).Name.Replace("Entity", string.Empty, System.StringComparison.InvariantCultureIgnoreCase));

            builder
                .Property(entity => entity.Created)
                .HasDefaultValueSql("getutcdate()");

            builder
                .Property(c => c.ExternalId)
                .HasDefaultValueSql("newsequentialid()");

            builder
                .HasAlternateKey(c => c.ExternalId);
        }

        #endregion
    }
}
