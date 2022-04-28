#region Usings

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UltimatePlaylist.Database.Infrastructure.Entities.Base.Configuration;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Playlist.Configuration
{
    public class UserPlaylistSongEntityConfiguration : BaseEntityConfiguration<UserPlaylistSongEntity>
    {
        #region Configure

        public override void Configure(EntityTypeBuilder<UserPlaylistSongEntity> builder)
        {
            base.Configure(builder);

            builder
             .HasMany(p => p.Tickets)
             .WithOne(s => s.UserPlaylistSong)
             .HasForeignKey(p => p.UserPlaylistSongId);

            builder
                .HasIndex(p => p.IsFinished);
        }

        #endregion
    }
}
