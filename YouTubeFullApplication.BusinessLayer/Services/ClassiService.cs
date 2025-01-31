using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using YouTubeFullApplication.DataAccessLayer;
using YouTubeFullApplication.Domain;
using YouTubeFullApplication.Dto;
using YouTubeFullApplication.ServiceResult;

namespace YouTubeFullApplication.BusinessLayer.Services
{

    public interface IClassiService : IDataService<
        Guid,
        ClasseDto,
        ClasseListDto,
        ClasseDetailsDto,
        ClassePostDto,
        ClassePutDto,
        ClasseRequestDto>
    {
        Task<Result<IEnumerable<ClasseDto>>> GetItemsAsync();
    }

    internal class ClassiService : DataService<
        Guid,
        Classe,
        ClasseDto,
        ClasseListDto,
        ClasseDetailsDto,
        ClassePostDto,
        ClassePutDto,
        ClasseRequestDto>, IClassiService
    {
        public ClassiService(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override async Task<ValidationError?> PrePostAsync(Classe entity, ClassePostDto model)
        {
            var exist = await context.Set<Classe>()
                .AsNoTracking()
                .Where(x => x.Nome.ToUpper() == model.Nome.ToUpper())
                .AnyAsync();
            if (exist) return new ValidationError("Nome", "Nome già presente");
            return null;
        }

        protected override async Task<ValidationError?> PrePutAsync(Classe entity, ClassePutDto model)
        {
            var exist = await context.Set<Classe>()
                 .AsNoTracking()
                 .Where(x => x.Id != model.Id && x.Nome.ToUpper() == model.Nome.ToUpper())
                 .AnyAsync();
            if (exist) return new ValidationError("Nome", "Nome già presente");
            return null;
        }

        public async Task<Result<IEnumerable<ClasseDto>>> GetItemsAsync()
        {
            var data = await context.Set<Classe>()
                .AsNoTracking()
                .OrderBy(x => x.Nome)
                .ProjectTo<ClasseDto>(mapper.ConfigurationProvider)
                .ToListAsync();
            return Result<IEnumerable<ClasseDto>>.Ok(data);
        }
    }
}
