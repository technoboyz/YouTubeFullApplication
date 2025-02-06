using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using YouTubeFullApplication.DataAccessLayer;
using YouTubeFullApplication.Domain;
using YouTubeFullApplication.Dto;
using YouTubeFullApplication.ServiceResult;

namespace YouTubeFullApplication.BusinessLayer.Services
{
    public interface IStudentiReadService :
        IDataReadService<Guid, StudenteDto, StudenteListDto, StudenteDetailsDto, StudenteRequestDto>
    {
        Task<Result<IEnumerable<StudenteDto>>> SuggestAsync(string text);
    }

    public interface IStudentiWriteService :
        IArchiveableDataWriteService<Guid, StudenteDto, StudentePostDto, StudentePutDto>
    {

    }

    internal class StudentiReadService :
        DataReadService<Guid, Studente, StudenteDto, StudenteListDto, StudenteDetailsDto, StudenteRequestDto>,
        IStudentiReadService
    {
        public StudentiReadService(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<Result<IEnumerable<StudenteDto>>> SuggestAsync(string text)
        {
            var data = await context.Set<Studente>()
                .AsNoTracking()
                .Where(x => x.IsDeleted == false)
                .Where(x => x.CognomeNome.ToLower().Contains(text.ToLower()) || x.CodiceFiscale.ToLower().Contains(text.ToLower()))
                //.Where(x => EF.Functions.Like(x.CognomeNome, $"%{text}%") || EF.Functions.Like(x.CodiceFiscale, $"%{text}%"))
                .OrderBy(x => x.CognomeNome)
                .ProjectTo<StudenteDto>(mapper.ConfigurationProvider)
                .ToListAsync();
            return Result<IEnumerable<StudenteDto>>.Ok(data);
        }
    }

    internal class StudentiWriteService :
        ArchiveableDataWriteService<Guid, Studente, StudenteDto, StudentePostDto, StudentePutDto>,
        IStudentiWriteService
    {
        public StudentiWriteService(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }


    // Service Non spittato in Read e Write
    /*
    public interface IStudentiService : IArchiveableDataService<
        Guid,
        StudenteDto,
        StudenteListDto,
        StudenteDetailsDto,
        StudentePostDto,
        StudentePutDto,
        StudenteRequestDto>
    {
        Task<Result<IEnumerable<StudenteDto>>> SuggestAsync(string text);
    }

    internal class StudentiService : ArchiveableDataService<
        Guid,
        Studente,
        StudenteDto,
        StudenteListDto,
        StudenteDetailsDto,
        StudentePostDto,
        StudentePutDto,
        StudenteRequestDto>, IStudentiService
    {
        public StudentiService(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override IQueryable<Studente> AddFilters(IQueryable<Studente> queryable, StudenteRequestDto request)
        {
            //if (!string.IsNullOrWhiteSpace(request.CognomeNome))
            //{
            //    queryable = queryable.Where("cognomenome.contains(@0)", request.CognomeNome);
            //}
            //if (!string.IsNullOrWhiteSpace(request.CodiceFiscale))
            //{
            //    queryable = queryable.Where("codicefiscale.contains(@0)", request.CodiceFiscale);
            //}
            //return queryable;
            return queryable
                .AddFilter(request.CognomeNome, "cognomenome")
                .AddFilter(request.CodiceFiscale, "codicefiscale");

        }

        protected override async Task<ValidationError?> PrePostAsync(Studente entity, StudentePostDto model)
        {
            var exist = await context.Set<Studente>()
                .AsNoTracking()
                .Where(x => x.CodiceFiscale.ToUpper() == model.CodiceFiscale.ToUpper())
                .AnyAsync();
            if (exist) return new("CodiceFiscale", "Codice Fiscale già presente");
            return null;
        }

        protected override async Task<ValidationError?> PrePutAsync(Studente entity, StudentePutDto model)
        {
            var exist = await context.Set<Studente>()
                .AsNoTracking()
                .Where(x => x.Id != model.Id && x.CodiceFiscale.ToUpper() == model.CodiceFiscale.ToUpper())
                .AnyAsync();
            if (exist) return new("CodiceFiscale", "Codice Fiscale già presente");
            return null;
        }

        public async Task<Result<IEnumerable<StudenteDto>>> SuggestAsync(string text)
        {
            var data = await context.Set<Studente>()
                .AsNoTracking()
                .Where(x => x.IsDeleted == false)
                .Where(x => x.CognomeNome.ToLower().Contains(text.ToLower()) || x.CodiceFiscale.ToLower().Contains(text.ToLower()))
                //.Where(x => EF.Functions.Like(x.CognomeNome, $"%{text}%") || EF.Functions.Like(x.CodiceFiscale, $"%{text}%"))
                .OrderBy(x => x.CognomeNome)
                .ProjectTo<StudenteDto>(mapper.ConfigurationProvider)
                .ToListAsync();
            return Result<IEnumerable<StudenteDto>>.Ok(data);
        }
    }
    */
}
