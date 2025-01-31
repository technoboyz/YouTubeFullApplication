using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using YouTubeFullApplication.DataAccessLayer;
using YouTubeFullApplication.Domain;
using YouTubeFullApplication.Dto;
using YouTubeFullApplication.ServiceResult;

namespace YouTubeFullApplication.BusinessLayer.Services
{
    public interface IMaterieService : IDataService<
        Guid,
        MateriaDto,
        MateriaListDto,
        MateriaDetailsDto,
        MateriaPostDto,
        MateriaPutDto,
        MateriaRequestDto>
    {
        Task<Result<IEnumerable<MateriaDto>>> GetItemsAsync();
    }

    internal class MaterieService : DataService<
        Guid,
        Materia,
        MateriaDto,
        MateriaListDto,
        MateriaDetailsDto,
        MateriaPostDto,
        MateriaPutDto,
        MateriaRequestDto>, IMaterieService
    {
        public MaterieService(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override async Task<ValidationError?> PrePostAsync(Materia entity, MateriaPostDto model)
        {
            var exist = await context.Set<Materia>()
                .AsNoTracking()
                .Where(x => x.Nome.ToUpper() == model.Nome.ToUpper())
                .AnyAsync();
            if (exist) return new ValidationError("Nome", "Nome già presente");
            return null;
        }

        protected override async Task<ValidationError?> PrePutAsync(Materia entity, MateriaPutDto model)
        {
            var exist = await context.Set<Materia>()
                .AsNoTracking()
                .Where(x => x.Id != model.Id && x.Nome.ToUpper() == model.Nome.ToUpper())
                .AnyAsync();
            if (exist) return new ValidationError("Nome", "Nome già presente");
            return null;
        }

        public async Task<Result<IEnumerable<MateriaDto>>> GetItemsAsync()
        {
            var data = await context.Set<Materia>()
                .AsNoTracking()
                .OrderBy(x => x.Nome)
                .ProjectTo<MateriaDto>(mapper.ConfigurationProvider)
                .ToListAsync();
            return Result<IEnumerable<MateriaDto>>.Ok(data);
        }
    }
}
