#region Usings

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Base.Configuration;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.File.Configuration
{
    public class BaseFileEntityConfiguration : BaseEntityConfiguration<BaseFileEntity>
    {
        public override void Configure(EntityTypeBuilder<BaseFileEntity> builder)
        {
            base.Configure(builder);

            builder
                .ToTable(nameof(BaseFileEntity)
                    .Replace("Entity", string.Empty, StringComparison.InvariantCultureIgnoreCase)
                    .Replace("Base", string.Empty, StringComparison.InvariantCultureIgnoreCase));

            builder
             .Property(e => e.Type)
             .HasConversion(
                 v => v.ToString(),
                 v => (FileType)Enum.Parse(typeof(FileType), v));

            builder
                .HasDiscriminator<FileType>(nameof(BaseFileEntity.Type))
                .HasValue<CoverFileEntity>(FileType.TrackCover)
                .HasValue<AudioFileEntity>(FileType.AudioFile)
                .HasValue<AvatarFileEntity>(FileType.Avatar);

            builder
                .Property(p => p.Type)
                .IsRequired();

            builder
                .Property(f => f.Container)
                .IsRequired();

            builder
                .Property(f => f.FileName)
                .IsRequired();

            builder
                .Property(f => f.Url)
                .IsRequired();
        }
    }
}
