#region Usings

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Games.Configuration
{
    public class DailyCashDrawingEntityConfiguration : IEntityTypeConfiguration<DailyCashDrawingEntity>
    {
        public void Configure(EntityTypeBuilder<DailyCashDrawingEntity> builder)
        {
        }
    }
}
