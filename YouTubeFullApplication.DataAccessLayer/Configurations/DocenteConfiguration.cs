using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouTubeFullApplication.Domain;

namespace YouTubeFullApplication.DataAccessLayer.Configurations
{
    internal class DocenteConfiguration : IEntityTypeConfiguration<Docente>
    {
        public void Configure(EntityTypeBuilder<Docente> builder)
        {
            builder.ToTable("Docenti").HasKey(x => x.Id);
            builder.HasIndex(e => e.CognomeNome);
            builder.HasIndex(e => e.CodiceFiscale).IsUnique();
            builder.Property(e => e.CodiceFiscale).HasMaxLength(16).IsRequired();
            builder.Property(e => e.Nome).HasMaxLength(32).IsRequired();
            builder.Property(e => e.Cognome).HasMaxLength(32).IsRequired();
            builder.Property(e => e.CognomeNome)
                .HasComputedColumnSql("upper(Cognome || ' ' || Nome)")
                .HasMaxLength(66)
                .IsRequired();
        }
    }
}
