using YouTubeFullApplication.Shared;

namespace YouTubeFullApplication.Domain
{
    public class Classe : Entity<Guid>
    {
        public string Nome { get; set; } = string.Empty;
        public virtual ICollection<Abbinamento>? Abbinamenti { get; set; }
        public virtual ICollection<Frequenza>? Frequenze { get; set; }
    }
}
