using YouTubeFullApplication.Shared;

namespace YouTubeFullApplication.Dto
{
    public class StudenteDto : Entity<Guid>
    {
        public string Nome { get; set; } = string.Empty;
        public string Cognome { get; set; } = string.Empty;
        public string CodiceFiscale { get; set; } = string.Empty;
        public DateOnly DataNascita { get; set; }
    }

    public class StudenteListDto : Entity<Guid>
    {
        public string CognomeNome { get; set; } = string.Empty;
        public string CodiceFiscale { get; set; } = string.Empty;
        public DateOnly DataNascita { get; set; }
        public int Anni { get; set; }
    }

    public class StudenteDetailsDto : Entity<Guid>
    {
        public string Nome { get; set; } = string.Empty;
        public string Cognome { get; set; } = string.Empty;
        public string CodiceFiscale { get; set; } = string.Empty;
        public DateOnly DataNascita { get; set; }
        public IEnumerable<FrequenzaStudenteDto> Frequenze { get; set; } = [];
    }

    public class StudentePostDto
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

    public class StudentePutDto : Entity<Guid>
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

    public class StudenteRequestDto : RequestBaseDto
    {
        public string? CognomeNome { get; set; }
        public string? CodiceFiscale { get; set; }
        public StudenteRequestDto() : base("cognomenome asc")
        {
        }
    }
}
