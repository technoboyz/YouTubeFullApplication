using YouTubeFullApplication.Shared;

namespace YouTubeFullApplication.Dto
{
    public class AbbinamentoDto : Entity<Guid>
    {
        public ClasseDto Classe { get; set; } = default!;
        public DocenteDto Docente { get; set; } = default!;
        public MateriaDto Materia { get; set; } = default!;
    }

    public class AbbinamentoListDto : Entity<Guid>
    {
        public Guid Classe_Id { get; set; }
        public string Classe { get; set; } = string.Empty;
        public Guid Docente_Id { get; set; }
        public string Docente { get; set; } = string.Empty;
        public Guid Materia_Id { get; set; }
        public string Materia { get; set; } = string.Empty;
    }

    public class AbbinamentoDetailsDto : Entity<Guid>
    {
        public ClasseDto Classe { get; set; } = default!;
        public DocenteDto Docente { get; set; } = default!;
        public MateriaDto Materia { get; set; } = default!;
    }

    public class AbbinamentoPostDto
    {
        public Guid? Classe_Id { get; set; }
        public Guid? Docente_Id { get; set; }
        public Guid? Materia_Id { get; set; }
    }

    public class AbbinamentoPutDto : Entity<Guid>
    {
        public Guid? Classe_Id { get; set; }
        public Guid? Docente_Id { get; set; }
        public Guid? Materia_Id { get; set; }
    }

    public class AbbinamentoRequestDto : RequestBaseDto
    {
        public string? Classe { get; set; }
        public string? Docente { get; set; }
        public string? Materia { get; set; }
        public AbbinamentoRequestDto() : base("classe.nome asc, materia.nome asc")
        {
        }
    }
}
