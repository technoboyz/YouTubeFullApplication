using YouTubeFullApplication.Shared;

namespace YouTubeFullApplication.Domain
{
    public class Materia : Entity<Guid>
    {
        public string Nome { get; set; } = string.Empty;
        public virtual ICollection<Abbinamento>? Abbinamenti { get; set; }
    }
}
