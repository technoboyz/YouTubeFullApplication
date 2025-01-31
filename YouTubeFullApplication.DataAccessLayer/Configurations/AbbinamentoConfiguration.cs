using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouTubeFullApplication.Domain;

namespace YouTubeFullApplication.DataAccessLayer.Configurations
{
    internal class AbbinamentoConfiguration : IEntityTypeConfiguration<Abbinamento>
    {
        public void Configure(EntityTypeBuilder<Abbinamento> builder)
        {
            builder.ToTable("Abbinamenti").HasKey(x => x.Id);
            builder
                .HasOne(e => e.Classe)
                .WithMany(p => p.Abbinamenti)
                .HasForeignKey(e => e.Classe_Id)
                .IsRequired();
            builder
                .HasOne(e => e.Materia)
                .WithMany(m => m.Abbinamenti)
                .HasForeignKey(e => e.Materia_Id)
                .IsRequired();
            builder
                .HasOne(e => e.Docente)
                .WithMany(d => d.Abbinamenti)
                .HasForeignKey(e => e.Docente_Id)
                .IsRequired();
        }
    }
}
