using YouTubeFullApplication.Shared;

namespace YouTubeFullApplication.Dto
{
    public class MateriaDto : Entity<Guid>
    {
        public string Nome { get; set; } = string.Empty;
    }

    public class MateriaListDto : Entity<Guid>
    {
        public string Nome { get; set; } = string.Empty;
        public int AbbinamentiCount { get; set; }
    }

    public class MateriaDetailsDto : Entity<Guid>
    {
        public string Nome { get; set; } = string.Empty;
        public int AbbinamentiCount { get; set; }
    }

    public class MateriaPostDto
    {
        private string nome = string.Empty;
        public string Nome
        {
            get => nome;
            set => nome = value.Trim().AllFirstUpper();
        }
    }

    public class MateriaPutDto : Entity<Guid>
    {
        private string nome = string.Empty;
        public string Nome
        {
            get => nome;
            set => nome = value.Trim().AllFirstUpper();
        }
    }

    public class MateriaRequestDto : RequestBaseDto
    {
        public MateriaRequestDto() : base("nome asc")
        {
        }
    }
}
