using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouTubeFullApplication.Domain;

namespace YouTubeFullApplication.DataAccessLayer.Configurations
{
    internal class AppIdentityUserRoleConfiguration : IEntityTypeConfiguration<AppIdentityUserRole>
    {
        public void Configure(EntityTypeBuilder<AppIdentityUserRole> builder)
        {
            builder.HasKey(ur => new { ur.UserId, ur.RoleId });
            builder.HasOne(ur => ur.Role).WithMany(r => r.UsersRoles).HasForeignKey(ur => ur.RoleId).IsRequired();
            builder.HasOne(ur => ur.User).WithMany(r => r.UsersRoles).HasForeignKey(ur => ur.UserId).IsRequired();
        }
    }
}
