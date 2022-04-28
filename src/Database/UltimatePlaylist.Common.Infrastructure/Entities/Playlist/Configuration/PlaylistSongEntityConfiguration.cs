#region Usings

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UltimatePlaylist.Database.Infrastructure.Entities.Base.Configuration;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Playlist.Configuration
{
    public class PlaylistSongEntityConfiguration : BaseEntityConfiguration<PlaylistSongEntity>
    {
        #region Configure

        public override void Configure(EntityTypeBuilder<PlaylistSongEntity> builder)
        {
            base.Configure(builder);
        }

        #endregion
    }
}
