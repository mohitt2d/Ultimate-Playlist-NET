#region Usings

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Base.Configuration;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Song.Configuration
{
    public class SongSocialMediaEntityConfiguration : BaseEntityConfiguration<SongSocialMediaEntity>
    {
        #region Configure

        public override void Configure(EntityTypeBuilder<SongSocialMediaEntity> builder)
        {
            base.Configure(builder);

            builder
                .Property(c => c.Type)
                .HasConversion(new EnumToStringConverter<SocialMediaType>());
        }

        #endregion
    }
}
