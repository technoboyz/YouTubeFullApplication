using YouTubeFullApplication.Shared;

namespace YouTubeFullApplication.Domain
{
    public class Abbinamento : Entity<Guid>
    {
        public Guid Classe_Id { get; set; }
        public virtual Classe? Classe { get; set; }

        public Guid Docente_Id { get; set; }
        public virtual Docente? Docente { get; set; }

        public Guid Materia_Id { get; set; }
        public virtual Materia? Materia { get; set; }
    }
}
