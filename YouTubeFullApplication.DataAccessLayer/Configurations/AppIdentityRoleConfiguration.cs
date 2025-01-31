using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouTubeFullApplication.Domain;
using YouTubeFullApplication.Shared;

namespace YouTubeFullApplication.DataAccessLayer.Configurations
{
    internal class AppIdentityRoleConfiguration : IEntityTypeConfiguration<AppIdentityRole>
    {
        public void Configure(EntityTypeBuilder<AppIdentityRole> builder)
        {
            builder.HasData(
               new AppIdentityRole { Id = new Guid("3295fcb3-96fb-4d79-9995-7d23199d9f99"), Name = AuthRoles.Admin, NormalizedName = AuthRoles.Admin.ToUpper() },
               new AppIdentityRole { Id = new Guid("761fc818-bc84-48b9-9bf1-651feaff0312"), Name = AuthRoles.User, NormalizedName = AuthRoles.User.ToUpper() }
           );
        }
    }
}
