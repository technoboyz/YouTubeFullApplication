using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouTubeFullApplication.Domain;

namespace YouTubeFullApplication.DataAccessLayer.Configurations
{
    internal class FrequenzaConfiguration : IEntityTypeConfiguration<Frequenza>
    {
        public void Configure(EntityTypeBuilder<Frequenza> builder)
        {
            builder.ToTable("Frequenze").HasKey(x => x.Id);
            builder
                .HasOne(e => e.Studente)
                .WithMany(s => s.Frequenze)
                .HasForeignKey(e => e.Studente_Id)
                .IsRequired();
            builder
                .HasOne(e => e.Classe)
                .WithMany(s => s.Frequenze)
                .HasForeignKey(e => e.Classe_Id)
                .IsRequired();
        }
    }
}
