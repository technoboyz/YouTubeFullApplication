using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouTubeFullApplication.Domain;

namespace YouTubeFullApplication.DataAccessLayer.Configurations
{
    internal class ClasseConfiguration : IEntityTypeConfiguration<Classe>
    {
        public void Configure(EntityTypeBuilder<Classe> builder)
        {
            builder.ToTable("Classi").HasKey(x => x.Id);
            builder.HasIndex(e => e.Nome).IsUnique();
            builder.Property(e => e.Nome).HasMaxLength(16).IsRequired();
        }
    }
}
