using MediatR;
using YouTubeFullApplication.ServiceResult;
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

    public class StudentePostDto : IRequest<Result<StudenteDto>>
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

    public class StudentePutDto : Entity<Guid>, IRequest<Result>
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

    public class StudenteRequestDto : RequestBaseDto, IRequest<Result<PagedResultDto<StudenteListDto>>>
    {
        public string? CognomeNome { get; set; }
        public string? CodiceFiscale { get; set; }
        public StudenteRequestDto() : base("cognomenome asc")
        {
        }
    }
    
    public class StudenteByIdRequest
    {
        public Guid Id { get; set; }
    }

    public class ModelListRequest<TRequest, TList> : IRequest<Result<PagedResultDto<TList>>> {
        public TRequest Request { get; set; } = default!;
    }
    public class ModelByIdRequest<Tkey, TModel> : Entity<Tkey>, IRequest<Result<TModel>> { }
    public class ModelDetailsByIdRequest<Tkey, TModel> : Entity<Tkey>, IRequest<Result<TModel>> { }
    public class ModelDeleteByIdRequest<Tkey, TModel> : Entity<Tkey>, IRequest<Result<TModel>> { }
    public class ModelUndeleteByIdRequest<Tkey, TModel> : Entity<Tkey>, IRequest<Result> { }
    public record SuggestRequest<TModel>(string Text) : IRequest<Result<IEnumerable<TModel>>>;
    public class ItemsRequest<TModel> : IRequest<Result<IEnumerable<TModel>>> { }
}
