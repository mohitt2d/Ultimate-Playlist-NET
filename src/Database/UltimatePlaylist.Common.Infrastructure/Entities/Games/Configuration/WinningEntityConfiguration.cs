#region Usings

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UltimatePlaylist.Database.Infrastructure.Entities.Base.Configuration;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Games.Configuration
{
    public class WinningEntityConfiguration : BaseEntityConfiguration<WinningEntity>
    {
        public override void Configure(EntityTypeBuilder<WinningEntity> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(p => p.Game)
                .WithMany(s => s.Winnings)
                .HasForeignKey(p => p.GameId)
                .IsRequired();

            builder
                .HasOne(p => p.Winner)
                .WithMany(s => s.Winnings)
                .HasForeignKey(p => p.WinnerId)
                .IsRequired();
        }
    }
}
