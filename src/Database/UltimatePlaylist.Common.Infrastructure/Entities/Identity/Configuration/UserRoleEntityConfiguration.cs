#region Usings

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Company.Configuration
{
    public class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRole>
    {
        #region Build User

        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder
                .HasOne(p => p.User)
                .WithMany(p => p.Roles)
                .HasForeignKey(p => p.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(p => p.Role)
                .WithMany(p => p.Users)
                .HasForeignKey(p => p.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }

        #endregion
    }
}