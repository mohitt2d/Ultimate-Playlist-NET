#region Usings

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.File.Configuration
{
    public class AvatarFileEntityConfiguration : IEntityTypeConfiguration<AvatarFileEntity>
    {
        public void Configure(EntityTypeBuilder<AvatarFileEntity> builder)
        {
        }
    }
}
