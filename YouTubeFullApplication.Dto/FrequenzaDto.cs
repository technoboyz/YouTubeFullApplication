using YouTubeFullApplication.Shared;

namespace YouTubeFullApplication.Dto
{
    public class FrequenzaDto : Entity<Guid>
    {
        public StudenteDto Studente { get; set; } = default!;
        public ClasseDto Classe { get; set; } = default!;
        public int AnnoScolastico { get; set; }
        public int Esito { get; set; }
    }

    public class FrequenzaListDto : Entity<Guid>
    {
        public Guid Studente_Id { get; set; }
        public string Studente_CognomeNome { get; set; } = string.Empty;
        public Guid Classe_Id { get; set; }
        public string Classe_Nome { get; set; } = string.Empty;
        public int AnnoScolastico { get; set; }
        public int Esito { get; set; }
    }

    public class FrequenzaDetailsDto : Entity<Guid>
    {
        public StudenteDto Studente { get; set; } = default!;
        public ClasseDto Classe { get; set; } = default!;
        public int AnnoScolastico { get; set; }
        public int Esito { get; set; }
    }

    public class FrequenzaPostDto
    {
        public Guid? Studente_Id { get; set; }
        public Guid? Classe_Id { get; set; }
        public int AnnoScolastico { get; set; }
        public int Esito { get; set; }
    }

    public class FrequenzaPutDto : Entity<Guid>
    {
        public Guid? Studente_Id { get; set; }
        public Guid? Classe_Id { get; set; }
        public int AnnoScolastico { get; set; }
        public int Esito { get; set; }
    }

    public class FrequenzaRequestDto : RequestBaseDto
    {
        public string? Studente { get; set; }
        public string? Classe { get; set; }
        public FrequenzaRequestDto() : base("studente.cognomenome asc")
        {
        }
    }

    public class FrequenzaStudenteDto : Entity<Guid>
    {
        public ClasseDto Classe { get; set; } = default!;
        public int AnnoScolastico { get; set; }
        public int Esito { get; set; }
    }
}
