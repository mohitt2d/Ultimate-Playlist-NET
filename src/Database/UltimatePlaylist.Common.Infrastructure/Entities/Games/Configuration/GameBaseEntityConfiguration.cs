#region Usings

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Base.Configuration;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Games.Configuration
{
    public class GameBaseEntityConfiguration : BaseEntityConfiguration<GameBaseEntity>
    {
        public override void Configure(EntityTypeBuilder<GameBaseEntity> builder)
        {
            base.Configure(builder);

            builder
                .ToTable(nameof(GameBaseEntity)
                    .Replace("Entity", string.Empty, StringComparison.InvariantCultureIgnoreCase)
                    .Replace("Base", string.Empty, StringComparison.InvariantCultureIgnoreCase));

            builder
             .Property(e => e.Type)
             .HasConversion(
                 v => v.ToString(),
                 v => (GameType)Enum.Parse(typeof(GameType), v));

            builder
                .HasDiscriminator<GameType>(nameof(GameBaseEntity.Type))
                .HasValue<DailyCashDrawingEntity>(GameType.DailyCashDrawing)
                .HasValue<UltimatePayoutEntity>(GameType.UltimatePayout);

            builder
                .Property(p => p.Type)
                .IsRequired();
        }
    }
}
