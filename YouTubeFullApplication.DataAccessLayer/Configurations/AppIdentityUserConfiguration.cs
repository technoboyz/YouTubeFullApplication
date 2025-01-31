using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouTubeFullApplication.Domain;

namespace YouTubeFullApplication.DataAccessLayer.Configurations
{
    internal class AppIdentityUserConfiguration : IEntityTypeConfiguration<AppIdentityUser>
    {
        public void Configure(EntityTypeBuilder<AppIdentityUser> builder)
        {
            builder.Property(m => m.Nome).HasMaxLength(32).IsRequired();
            builder.Property(m => m.Cognome).HasMaxLength(32).IsRequired();
            builder.Property(m => m.RefreshToken).HasMaxLength(512).IsRequired(false);
            builder.Property(m => m.RefreshTokenExpirationDate).HasColumnType("datetime").IsRequired(false);
        }
    }
}
