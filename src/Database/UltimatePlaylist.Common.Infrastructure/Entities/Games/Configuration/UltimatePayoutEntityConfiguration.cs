#region Usings

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Games.Configuration
{
    public class UltimatePayoutEntityConfiguration : IEntityTypeConfiguration<UltimatePayoutEntity>
    {
        public void Configure(EntityTypeBuilder<UltimatePayoutEntity> builder)
        {
        }
    }
}
