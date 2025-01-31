using YouTubeFullApplication.Shared;

namespace YouTubeFullApplication.Dto
{
    public class ClasseDto : Entity<Guid>
    {
        public string Nome { get; set; } = string.Empty;
    }

    public class ClasseListDto : Entity<Guid>
    {
        public string Nome { get; set; } = string.Empty;
    }

    public class ClasseDetailsDto : Entity<Guid>
    {
        public string Nome { get; set; } = string.Empty;
    }

    public class ClassePostDto
    {
        private string nome = string.Empty;
        public string Nome
        {
            get => nome;
            set => nome = value.Trim().ToUpper();
        }
    }

    public class ClassePutDto : Entity<Guid>
    {
        private string nome = string.Empty;
        public string Nome
        {
            get => nome;
            set => nome = value.Trim().ToUpper();
        }
    }

    public class ClasseRequestDto : RequestBaseDto
    {
        public ClasseRequestDto() : base("nome asc")
        {
        }
    }
}
