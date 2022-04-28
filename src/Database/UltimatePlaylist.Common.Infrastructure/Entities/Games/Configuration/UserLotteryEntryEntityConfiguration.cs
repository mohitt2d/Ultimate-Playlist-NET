#region Usings

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UltimatePlaylist.Database.Infrastructure.Entities.Base.Configuration;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Games.Configuration
{
    public class UserLotteryEntryEntityConfiguration : BaseEntityConfiguration<UserLotteryEntryEntity>
    {
        public override void Configure(EntityTypeBuilder<UserLotteryEntryEntity> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(p => p.User)
                .WithMany(s => s.UserLotteryEntries)
                .HasForeignKey(p => p.UserId)
                .IsRequired();

            builder
                .HasOne(p => p.Game)
                .WithMany(s => s.UserLotteryEntries)
                .HasForeignKey(p => p.GameId)
                .IsRequired();
        }
    }
}
