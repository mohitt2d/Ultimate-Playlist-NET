#region Usings

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Base.Configuration;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Song.Configuration
{
    public class SongGenreEntityConfiguration : BaseEntityConfiguration<SongGenreEntity>
    {
        #region Configure

        public override void Configure(EntityTypeBuilder<SongGenreEntity> builder)
        {
            base.Configure(builder);

            builder
                .Property(c => c.Type)
                .HasConversion(new EnumToStringConverter<SongGenreType>());
        }

        #endregion
    }
}
