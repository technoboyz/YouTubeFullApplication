using YouTubeFullApplication.Shared;

namespace YouTubeFullApplication.Dto
{
    public class DocenteDto : Entity<Guid>
    {
        public string Nome { get; set; } = string.Empty;
        public string Cognome { get; set; } = string.Empty;
        public string CodiceFiscale { get; set; } = string.Empty;
        public DateOnly DataNascita { get; set; }
    }

    public class DocenteListDto : Entity<Guid>
    {
        public string CognomeNome { get; set; } = string.Empty;
        public string CodiceFiscale { get; set; } = string.Empty;
        public DateOnly DataNascita { get; set; }
        public int AbbinamentiCount { get; set; }
    }

    public class DocenteDetailsDto : Entity<Guid>
    {
        public string Nome { get; set; } = string.Empty;
        public string Cognome { get; set; } = string.Empty;
        public string CodiceFiscale { get; set; } = string.Empty;
        public DateOnly DataNascita { get; set; }
        public int AbbinamentiCount { get; set; }
    }

    public class DocentePostDto
    {
        private string nome = string.Empty;
        public string Nome
        {
            get => nome;
            set => nome = value.Trim().AllFirstUpper();
        }
        private string cognome = string.Empty;
        public string Cognome
        {
            get => cognome;
            set => cognome = value.Trim().AllFirstUpper();
        }
        private string codiceFiscale = string.Empty;
        public string CodiceFiscale
        {
            get => codiceFiscale;
            set => codiceFiscale = value.Trim().ToUpper();
        }
        public DateOnly DataNascita { get; set; }
    }

    public class DocentePutDto : Entity<Guid>
    {
        private string nome = string.Empty;
        public string Nome
        {
            get => nome;
            set => nome = value.Trim().AllFirstUpper();
        }
        private string cognome = string.Empty;
        public string Cognome
        {
            get => cognome;
            set => cognome = value.Trim().AllFirstUpper();
        }
        private string codiceFiscale = string.Empty;
        public string CodiceFiscale
        {
            get => codiceFiscale;
            set => codiceFiscale = value.Trim().ToUpper();
        }
        public DateOnly DataNascita { get; set; }
    }

    public class DocenteRequestDto : RequestBaseDto
    {
        public string? CognomeNome { get; set; }
        public string? CodiceFiscale { get; set; }
        public DocenteRequestDto() : base("cognomenome asc")
        {
        }
    }
}
