#region Usings

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Base.Configuration;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.UserSongHistory.Configuration
{
    public class UserSongHistoryConfiguration : BaseEntityConfiguration<UserSongHistoryEntity>
    {
        #region Configure

        public override void Configure(EntityTypeBuilder<UserSongHistoryEntity> builder)
        {
            base.Configure(builder);

            builder
                .HasMany(p => p.Tickets)
                .WithOne(s => s.UserSongHistory)
                .HasForeignKey(p => p.UserSongHistoryId);
        }

        #endregion
    }
}
