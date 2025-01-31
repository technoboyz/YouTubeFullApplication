using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YouTubeFullApplication.DataAccessLayer;
using YouTubeFullApplication.Domain;
using YouTubeFullApplication.Dto;
using YouTubeFullApplication.ServiceResult;

namespace YouTubeFullApplication.BusinessLayer.Services
{
    public interface IAbbinamentiService : IDataService<
        Guid,
        AbbinamentoDto,
        AbbinamentoListDto,
        AbbinamentoDetailsDto,
        AbbinamentoPostDto,
        AbbinamentoPutDto,
        AbbinamentoRequestDto>
    {
    }

    internal class AbbinamentiService : DataService<
        Guid,
        Abbinamento,
        AbbinamentoDto,
        AbbinamentoListDto,
        AbbinamentoDetailsDto,
        AbbinamentoPostDto,
        AbbinamentoPutDto,
        AbbinamentoRequestDto>, IAbbinamentiService
    {
        public AbbinamentiService(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override IQueryable<Abbinamento> AddFilters(IQueryable<Abbinamento> queryable, AbbinamentoRequestDto request)
        {
            return queryable
                .AddFilter(request.Materia, "materia.nome")
                .AddFilter(request.Classe, "classe.nome")
                .AddFilter(request.Docente, "docente.cognomenome");

        }

        protected override async Task<ValidationError?> PrePostAsync(Abbinamento entity, AbbinamentoPostDto model)
        {
            var classe = await context.Set<Classe>()
                .AsNoTracking()
                .Where(x => x.Id == model.Classe_Id)
                .AnyAsync();
            if (!classe) return new ValidationError("Classe_Id", $"Classe con Id {model.Classe_Id} non trovata");
            var materia = await context.Set<Materia>()
                .AsNoTracking()
                .Where(x => x.Id == model.Materia_Id)
                .AnyAsync();
            if (!materia) return new ValidationError("Materia_Id", $"Materia con Id {model.Materia_Id} non trovata");
            var docente = await context.Set<Docente>()
                .AsNoTracking()
                .Where(x => x.Id == model.Docente_Id)
                .AnyAsync();
            if (!docente) return new ValidationError("Docente_Id", $"Docente con Id {model.Docente_Id} non trovato");
            return null;
        }

        protected override async Task<ValidationError?> PrePutAsync(Abbinamento entity, AbbinamentoPutDto model)
        {
            var classe = await context.Set<Classe>()
                .AsNoTracking()
                .Where(x => x.Id == model.Classe_Id)
                .AnyAsync();
            if (!classe) return new ValidationError("Classe_Id", $"Classe con Id {model.Classe_Id} non trovata");
            var materia = await context.Set<Materia>()
                .AsNoTracking()
                .Where(x => x.Id == model.Materia_Id)
                .AnyAsync();
            if (!materia) return new ValidationError("Materia_Id", $"Materia con Id {model.Materia_Id} non trovata");
            var docente = await context.Set<Docente>()
                .AsNoTracking()
                .Where(x => x.Id == model.Docente_Id)
                .AnyAsync();
            if (!docente) return new ValidationError("Docente_Id", $"Docente con Id {model.Docente_Id} non trovato");
            return null;
        }
    }
}
