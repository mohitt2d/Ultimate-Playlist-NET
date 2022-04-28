#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Base.Configuration;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Dsp.Configuration
{
    public class UserDspConfiguration : BaseEntityConfiguration<UserDspEntity>
    {
        #region Configure

        public override void Configure(EntityTypeBuilder<UserDspEntity> builder)
        {
            base.Configure(builder);

            builder
                .Property(c => c.Type)
                .HasConversion(new EnumToStringConverter<DspType>());
        }

        #endregion
    }
}
