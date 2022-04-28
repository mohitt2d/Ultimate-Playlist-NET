#region Usings

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UltimatePlaylist.Database.Infrastructure.Entities.Base.Configuration;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Playlist.Configuration
{
    public class PlaylistEntityConfiguration : BaseEntityConfiguration<PlaylistEntity>
    {
        #region Configure

        public override void Configure(EntityTypeBuilder<PlaylistEntity> builder)
        {
            base.Configure(builder);

            builder
                 .HasMany(p => p.PlaylistSongs)
                 .WithOne(s => s.Playlist)
                 .HasForeignKey(p => p.PlaylistId)
                 .IsRequired();
        }

        #endregion
    }
}
