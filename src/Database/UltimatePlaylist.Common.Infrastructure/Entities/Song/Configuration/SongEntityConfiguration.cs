#region Usings

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UltimatePlaylist.Database.Infrastructure.Entities.Base.Configuration;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Song.Configuration
{
    public class SongEntityConfiguration : BaseEntityConfiguration<SongEntity>
    {
        #region Configure

        public override void Configure(EntityTypeBuilder<SongEntity> builder)
        {
            base.Configure(builder);

            builder
               .HasOne(t => t.AudioFile)
               .WithOne(t => t.Song)
               .HasForeignKey<SongEntity>(t => t.AudioFileId)
               .OnDelete(DeleteBehavior.NoAction)
               .IsRequired();

            builder
               .HasOne(t => t.CoverFile)
               .WithOne(t => t.Song)
               .HasForeignKey<SongEntity>(t => t.CoverFileId)
               .OnDelete(DeleteBehavior.NoAction)
               .IsRequired();

            builder
                 .HasMany(p => p.SongGenres)
                 .WithOne(s => s.Song)
                 .HasForeignKey(p => p.SongId);

            builder
                 .HasMany(p => p.SongSocialMedias)
                 .WithOne(s => s.Song)
                 .HasForeignKey(p => p.SongId);

            builder
                 .HasMany(p => p.PlaylistSongs)
                 .WithOne(s => s.Song)
                 .HasForeignKey(p => p.SongId);

            builder
                 .HasMany(p => p.SongDSPs)
                 .WithOne(s => s.Song)
                 .HasForeignKey(p => p.SongId);

            builder
                .HasMany(p => p.UserSongHistory)
                .WithOne(s => s.Song)
                .HasForeignKey(p => p.SongId);

            builder
                 .HasMany(p => p.UserPlaylistSongs)
                 .WithOne(s => s.Song)
                 .HasForeignKey(p => p.SongId);
        }

        #endregion
    }
}
