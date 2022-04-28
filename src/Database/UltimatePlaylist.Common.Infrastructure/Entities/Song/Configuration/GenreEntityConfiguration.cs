#region Usings

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UltimatePlaylist.Database.Infrastructure.Entities.Base.Configuration;
using UltimatePlaylist.Database.Infrastructure.Entities.Song;

#endregion
namespace UltimatePlaylist.Database.Infrastructure.Entities.Song.Configuration
{
    public class GenreEntityConfiguration : BaseEntityConfiguration<GenreEntity>
    {
        #region Configure

        public override void Configure(EntityTypeBuilder<GenreEntity> builder)
        {
            base.Configure(builder);

            builder
                 .HasMany(p => p.SongGenres)
                 .WithOne(s => s.Genre)
                 .HasForeignKey(p => p.GenreId)
                 .IsRequired();
        }

        #endregion
    }
}
