using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using YouTubeFullApplication.DataAccessLayer;
using YouTubeFullApplication.Domain;
using YouTubeFullApplication.Dto;
using YouTubeFullApplication.ServiceResult;

namespace YouTubeFullApplication.BusinessLayer.Services
{
    //public interface IStudentiService
    //{
    //    Task<Result<PagedResultDto<StudenteListDto>>> GetAllAsync(StudenteRequestDto request);
    //    Task<Result<StudenteDto>> GetByIdAsync(Guid id);
    //    Task<Result<StudenteDetailsDto>> GetDetailsByIdAsync(Guid id);
    //    Task<Result<StudenteDto>> PostAsync(StudentePostDto model);
    //    Task<Result> PutAsync(StudentePutDto model);
    //    Task<Result<StudenteDto>> DeleteByIdAsync(Guid id);
    //}

    //internal class StudentiService : IStudentiService
    //{
    //    private readonly AppDbContext context;
    //    private readonly IMapper mapper;

    //    public StudentiService(AppDbContext context, IMapper mapper)
    //    {
    //        this.context = context;
    //        this.mapper = mapper;
    //    }

    //    public async Task<Result<StudenteDto>> DeleteByIdAsync(Guid id)
    //    {
    //        var entity = await context.Set<Studente>()
    //            .Where(x => x.Id == id)
    //            .SingleOrDefaultAsync();
    //        if (entity == null) return Result<StudenteDto>.Fail(FailureReasons.NotFound, "Id", "Id studente non trovato");
    //        entity.IsDeleted = true;
    //        _ = await context.SaveChangesAsync();

    //        var dto = mapper.Map<StudenteDto>(entity); // AutoMapper

    //        return Result<StudenteDto>.Ok(dto);

    //    }

    //    public async Task<Result<PagedResultDto<StudenteListDto>>> GetAllAsync(StudenteRequestDto request)
    //    {
    //        var pagedResult = context.Set<Studente>()
    //            .AsNoTracking()
    //            .OrderBy(x => x.CognomeNome)
    //            .ProjectTo<StudenteListDto>(mapper.ConfigurationProvider) // AutoMapper
    //            //.Select(x => new StudenteListDto()
    //            //{
    //            //    Id = x.Id,
    //            //    CognomeNome = x.CognomeNome,
    //            //    CodiceFiscale = x.CodiceFiscale,
    //            //    DataNascita = x.DataNascita,
    //            //    Anni = x.Frequenze!.Count()
    //            //})
    //            .PageResult(request.Page, request.PageSize);
    //        var dto = new PagedResultDto<StudenteListDto>()
    //        {
    //            Page = pagedResult.CurrentPage,
    //            PageSize = request.PageSize,
    //            PageCount = pagedResult.PageCount,
    //            TotalCount = pagedResult.RowCount,
    //            Items = await pagedResult.Queryable.ToListAsync()
    //        };
    //        return Result<PagedResultDto<StudenteListDto>>.Ok(dto);
    //    }

    //    public async Task<Result<StudenteDto>> GetByIdAsync(Guid id)
    //    {
    //        var dto = await context.Set<Studente>()
    //            .AsNoTracking()
    //            .Where(x => x.Id == id)
    //            .ProjectTo<StudenteDto>(mapper.ConfigurationProvider) // AutoMapper
    //            .SingleOrDefaultAsync();
    //        if (dto == null) return Result<StudenteDto>.Fail(FailureReasons.NotFound, "Id", "Id studente non trovato");
    //        return Result<StudenteDto>.Ok(dto);
    //    }

    //    public async Task<Result<StudenteDetailsDto>> GetDetailsByIdAsync(Guid id)
    //    {
    //        var dto = await context.Set<Studente>()
    //            .AsNoTracking()
    //            .Where(x => x.Id == id)
    //            .ProjectTo<StudenteDetailsDto>(mapper.ConfigurationProvider)
    //            .SingleOrDefaultAsync();
    //        if (dto == null) return Result<StudenteDetailsDto>.Fail(FailureReasons.NotFound, "Id", "Id studente non trovato");
    //        return Result<StudenteDetailsDto>.Ok(dto);
    //    }

    //    public async Task<Result<StudenteDto>> PostAsync(StudentePostDto model)
    //    {
    //        var exist = await context.Set<Studente>()
    //            .AsNoTracking()
    //            .Where(x => x.CodiceFiscale.ToUpper() == model.CodiceFiscale.ToUpper())
    //            .AnyAsync();
    //        if (exist) return Result<StudenteDto>.Fail(FailureReasons.BadRequest, "CodiceFiscale", "Codice Fiscale già presente");

    //        var entity = mapper.Map<Studente>(model);

    //        context.Add(entity);
    //        try
    //        {
    //            _ = await context.SaveChangesAsync();

    //            var dto = mapper.Map<StudenteDto>(entity);

    //            return Result<StudenteDto>.Ok(dto);
    //        }
    //        catch (Exception ex) {
    //            return Result<StudenteDto>.Fail(FailureReasons.DatabaseError, ex, new ValidationError("DatabaseError", ex.InnerException?.Message ?? ex.Message));
    //        }
    //    }

    //    public async Task<Result> PutAsync(StudentePutDto model)
    //    {
    //        var exist = await context.Set<Studente>()
    //            .AsNoTracking()
    //            .Where(x => x.Id != model.Id && x.CodiceFiscale.ToUpper() == model.CodiceFiscale.ToUpper())
    //            .AnyAsync();
    //        if (exist) return Result.Fail(FailureReasons.BadRequest, "CodiceFiscale", "Codice Fiscale già presente");
    //        var entity = await context.Set<Studente>()
    //            .Where(x => x.Id == model.Id)
    //            .SingleOrDefaultAsync();
    //        if (entity == null) return Result.Fail(FailureReasons.NotFound, "Id", "Id studente non trovato");

    //        mapper.Map(model, entity); // AutoMapper
    //        try
    //        {
    //            _ = await context.SaveChangesAsync();
    //            return Result.Ok();
    //        }
    //        catch (Exception ex)
    //        {
    //            return Result.Fail(FailureReasons.DatabaseError, ex, new ValidationError("DatabaseError", ex.InnerException?.Message ?? ex.Message));
    //        }
    //    }
    //}

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
}
