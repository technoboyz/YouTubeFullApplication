using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YouTubeFullApplication.DataAccessLayer;
using YouTubeFullApplication.Domain;
using YouTubeFullApplication.Dto;
using YouTubeFullApplication.ServiceResult;

namespace YouTubeFullApplication.BusinessLayer.Services
{
    public interface IFrequenzeService : IDataService<
        Guid,
        FrequenzaDto,
        FrequenzaListDto,
        FrequenzaDetailsDto,
        FrequenzaPostDto,
        FrequenzaPutDto,
        FrequenzaRequestDto>
    {
    }

    internal class FrequenzeService : DataService<
        Guid,
        Frequenza,
        FrequenzaDto,
        FrequenzaListDto,
        FrequenzaDetailsDto,
        FrequenzaPostDto,
        FrequenzaPutDto,
        FrequenzaRequestDto>, IFrequenzeService
    {
        public FrequenzeService(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override IQueryable<Frequenza> AddFilters(IQueryable<Frequenza> queryable, FrequenzaRequestDto request)
        {
            return queryable
                .AddFilter(request.Studente, "studente.cognomenome")
                .AddFilter(request.Classe, "classe.nome");
        }

        protected override async Task<ValidationError?> PrePostAsync(Frequenza entity, FrequenzaPostDto model)
        {
            var classe = await context.Set<Classe>()
                .AsNoTracking()
                .Where(x => x.Id == model.Classe_Id)
                .AnyAsync();
            if (!classe) return new("Classe_Id", $"Classe con Id {model.Classe_Id} non trovata");
            var studente = await context.Set<Studente>()
                .AsNoTracking()
                .Where(x => x.Id == model.Studente_Id)
                .AnyAsync();
            if (!studente) return new("Studente_Id", $"Studente con Id {model.Studente_Id} non trovato");
            return null;
        }

        protected override async Task<ValidationError?> PrePutAsync(Frequenza entity, FrequenzaPutDto model)
        {
            var classe = await context.Set<Classe>()
                .AsNoTracking()
                .Where(x => x.Id == model.Classe_Id)
                .AnyAsync();
            if (!classe) return new("Classe_Id", $"Classe con Id {model.Classe_Id} non trovata");
            var studente = await context.Set<Studente>()
                .AsNoTracking()
                .Where(x => x.Id == model.Studente_Id)
                .AnyAsync();
            if (!studente) return new("Studente_Id", $"Studente con Id {model.Studente_Id} non trovato");
            return null;
        }
    }
}
