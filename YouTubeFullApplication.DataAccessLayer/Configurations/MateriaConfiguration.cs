using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouTubeFullApplication.Domain;

namespace YouTubeFullApplication.DataAccessLayer.Configurations
{
    internal class MateriaConfiguration : IEntityTypeConfiguration<Materia>
    {
        public void Configure(EntityTypeBuilder<Materia> builder)
        {
            builder.ToTable("Materie").HasKey(x => x.Id);
            builder.HasIndex(e => e.Nome).IsUnique();
            builder.Property(e => e.Nome).HasMaxLength(32).IsRequired();
        }
    }
}
