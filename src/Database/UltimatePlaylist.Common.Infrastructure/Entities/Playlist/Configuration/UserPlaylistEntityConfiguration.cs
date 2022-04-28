#region Usings

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Base.Configuration;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Playlist.Configuration
{
    public class UserPlaylistEntityConfiguration : BaseEntityConfiguration<UserPlaylistEntity>
    {
        #region Configure

        public override void Configure(EntityTypeBuilder<UserPlaylistEntity> builder)
        {
            base.Configure(builder);

            builder
                 .HasMany(p => p.UserPlaylistSongs)
                 .WithOne(s => s.UserPlaylist)
                 .HasForeignKey(p => p.UserPlaylistId)
                 .IsRequired();

            builder
               .Property(c => c.State)
               .HasConversion(new EnumToStringConverter<PlaylistState>());
        }

        #endregion
    }
}
