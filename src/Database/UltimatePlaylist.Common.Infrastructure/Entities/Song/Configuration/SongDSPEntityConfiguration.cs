#region Usings

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Base.Configuration;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Song.Configuration
{
    public class SongDSPEntityConfiguration : BaseEntityConfiguration<SongDSPEntity>
    {
        #region Configure

        public override void Configure(EntityTypeBuilder<SongDSPEntity> builder)
        {
            base.Configure(builder);

            builder
                .Property(c => c.DspType)
                .HasConversion(new EnumToStringConverter<DspType>());
        }

        #endregion
    }
}
