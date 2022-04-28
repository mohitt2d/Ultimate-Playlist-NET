#region Usings

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.File.Configuration
{
    public class CoverFileEntityConfiguration : IEntityTypeConfiguration<CoverFileEntity>
    {
        public void Configure(EntityTypeBuilder<CoverFileEntity> builder)
        {
        }
    }
}
