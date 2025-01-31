using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using YouTubeFullApplication.DataAccessLayer;
using YouTubeFullApplication.Domain;
using YouTubeFullApplication.Dto;
using YouTubeFullApplication.ServiceResult;

namespace YouTubeFullApplication.BusinessLayer.Services
{
    public interface IDocentiService : IArchiveableDataService<
        Guid,
        DocenteDto,
        DocenteListDto,
        DocenteDetailsDto,
        DocentePostDto,
        DocentePutDto,
        DocenteRequestDto>
    {
        Task<Result<IEnumerable<DocenteDto>>> GetItemsAsync();
    }

    internal class DocentiService : ArchiveableDataService<
        Guid,
        Docente,
        DocenteDto,
        DocenteListDto,
        DocenteDetailsDto,
        DocentePostDto,
        DocentePutDto,
        DocenteRequestDto>, IDocentiService
    {
        public DocentiService(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override IQueryable<Docente> AddFilters(IQueryable<Docente> queryable, DocenteRequestDto request)
        {
            return queryable
               .AddFilter(request.CognomeNome, "cognomenome")
               .AddFilter(request.CodiceFiscale, "codicefiscale");
        }

        protected override async Task<ValidationError?> PrePostAsync(Docente entity, DocentePostDto model)
        {
            var exist = await context.Set<Docente>()
                .AsNoTracking()
                .Where(x => x.CodiceFiscale.ToUpper() == model.CodiceFiscale.ToUpper())
                .AnyAsync();
            if (exist) return new("CodiceFiscale", "Codice Fiscale già presente");
            return null;
        }

        protected override async Task<ValidationError?> PrePutAsync(Docente entity, DocentePutDto model)
        {
            var exist = await context.Set<Docente>()
                .AsNoTracking()
                .Where(x => x.Id != model.Id && x.CodiceFiscale.ToUpper() == model.CodiceFiscale.ToUpper())
                .AnyAsync();
            if (exist) return new("CodiceFiscale", "Codice Fiscale già presente");
            return null;
        }

        public async Task<Result<IEnumerable<DocenteDto>>> GetItemsAsync()
        {
            var data = await context.Set<Docente>()
                .AsNoTracking()
                .Where(x => x.IsDeleted == false)
                .OrderBy(x => x.CognomeNome)
                .ProjectTo<DocenteDto>(mapper.ConfigurationProvider)
                .ToListAsync();
            return Result<IEnumerable<DocenteDto>>.Ok(data);
        }
    }
}
