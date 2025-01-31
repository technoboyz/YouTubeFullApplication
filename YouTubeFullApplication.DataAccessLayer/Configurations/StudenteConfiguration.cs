using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouTubeFullApplication.Domain;

namespace YouTubeFullApplication.DataAccessLayer.Configurations
{
    internal class StudenteConfiguration : IEntityTypeConfiguration<Studente>
    {
        public void Configure(EntityTypeBuilder<Studente> builder)
        {
            builder.ToTable("Studenti").HasKey(x => x.Id);
            builder.HasIndex(x => x.CognomeNome);
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
