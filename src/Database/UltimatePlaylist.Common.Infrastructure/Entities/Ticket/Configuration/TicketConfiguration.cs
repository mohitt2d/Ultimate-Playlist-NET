#region Usings

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Base.Configuration;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Ticket.Configuration
{
    public class TicketConfiguration : BaseEntityConfiguration<TicketEntity>
    {
        #region Configure

        public override void Configure(EntityTypeBuilder<TicketEntity> builder)
        {
            base.Configure(builder);

            builder
               .Property(c => c.Type)
               .HasConversion(new EnumToStringConverter<TicketType>());

            builder
               .Property(p => p.EarnedType)
               .HasConversion(new EnumToStringConverter<TicketEarnedType>());
        }

        #endregion
    }
}
