#region Usings

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.File.Configuration
{
    public class AudioFileEntityConfiguration : IEntityTypeConfiguration<AudioFileEntity>
    {
        public void Configure(EntityTypeBuilder<AudioFileEntity> builder)
        {
            builder
                .Property(c => c.JobErrorCode)
                .HasConversion(new EnumToStringConverter<MediaServicesJobErrorCode>());

            builder
                .Property(c => c.JobState)
                .HasConversion(new EnumToStringConverter<MediaServicesJobState>());
        }
    }
}
