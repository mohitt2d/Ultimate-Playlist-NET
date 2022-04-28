#region Usings

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UltimatePlaylist.Database.Infrastructure.Entities.Base.Configuration;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Identity.Configuration
{
    public class GenderEntityConfiguration : BaseEntityConfiguration<GenderEntity>
    {
        #region Configure

        public override void Configure(EntityTypeBuilder<GenderEntity> builder)
        {
            base.Configure(builder);

            builder
                 .HasMany(p => p.Users)
                 .WithOne(s => s.Gender)
                 .HasForeignKey(p => p.GenderId)
                 .IsRequired();
        }

        #endregion
    }
}
