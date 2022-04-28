#region Usings

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Company.Configuration
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        #region Build User

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .Property(c => c.ExternalId)
                .HasDefaultValueSql("newsequentialid()");

            builder
                .HasMany(t => t.Roles)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .IsRequired();

            builder
                .HasMany(t => t.Dsps)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId);

            builder
                .HasMany(t => t.UserSongsHistory)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId);

            builder
                .HasMany(t => t.UserPlaylists)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId);

            builder
               .HasOne(t => t.AvatarFile)
               .WithOne(t => t.User)
               .HasForeignKey<User>(t => t.AvatarFileId)
               .OnDelete(DeleteBehavior.NoAction);
        }

        #endregion
    }
}