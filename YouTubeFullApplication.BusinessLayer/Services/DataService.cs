using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using YouTubeFullApplication.Dto;
using YouTubeFullApplication.ServiceResult;
using YouTubeFullApplication.Shared;

namespace YouTubeFullApplication.BusinessLayer.Services
{
    public interface IDataService<TKey, TModel, TList, TDetails, TPost, TPut, TRequest>
        where TKey : struct
        where TModel : IEntity<TKey>
        where TDetails : IEntity<TKey>
        where TPut : IEntity<TKey>
        where TRequest : RequestBaseDto
    {
        Task<Result<PagedResultDto<TList>>> GetAllAsync(TRequest request);
        Task<Result<TModel>> GetByIdAsync(TKey id);
        Task<Result<TDetails>> GetDetailsByIdAsync(TKey id);
        Task<Result<TModel>> PostAsync(TPost model);
        Task<Result> PutAsync(TPut model);
        Task<Result<TModel>> DeleteByIdAsync(TKey id);
    }

    internal abstract class DataService<TKey, TEntity, TModel, TList, TDetails, TPost, TPut, TRequest> :
        IDataService<TKey, TModel, TList, TDetails, TPost, TPut, TRequest>
        where TKey : struct
        where TEntity : Entity<TKey>
        where TModel : IEntity<TKey>
        where TDetails : IEntity<TKey>
        where TPut : IEntity<TKey>
        where TRequest : RequestBaseDto
    {
        protected readonly DbContext context;
        protected readonly IMapper mapper;

        public DataService(DbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Result<PagedResultDto<TList>>> GetAllAsync(TRequest request)
        {
            var queryable = context.Set<TEntity>().AsNoTracking().AsQueryable();
            if (typeof(TEntity).GetInterfaces().Contains(typeof(IArchiveableEntity<TKey>)))
            {
                queryable = queryable.Where("isdeleted = @0", request.RetriveDeleted);
            }
            queryable = AddFilters(queryable, request);
            var pagedResult = queryable
                .OrderBy(request.Order) // using System.Linq.Dynamic.Core;
                .ProjectTo<TList>(mapper.ConfigurationProvider)
                .PageResult(request.Page, request.PageSize);
            var dto = new PagedResultDto<TList>()
            {
                Page = pagedResult.CurrentPage,
                PageSize = request.PageSize,
                PageCount = pagedResult.PageCount,
                TotalCount = pagedResult.RowCount,
                Items = await pagedResult.Queryable.ToListAsync()
            };
            return Result<PagedResultDto<TList>>.Ok(dto);
        }

        protected virtual IQueryable<TEntity> AddFilters(IQueryable<TEntity> queryable, TRequest request)
        {
            return queryable;
        }

        public async Task<Result<TModel>> GetByIdAsync(TKey id)
        {
            var dto = await context.Set<TEntity>()
                .AsNoTracking()
                .Where(x => x.Id.Equals(id))
                .ProjectTo<TModel>(mapper.ConfigurationProvider) // AutoMapper
                .SingleOrDefaultAsync();
            if (dto == null) return Result<TModel>.Fail(FailureReasons.NotFound, "Id", $"Id {typeof(TEntity).Name} non trovato");
            return Result<TModel>.Ok(dto);
        }

        public async Task<Result<TDetails>> GetDetailsByIdAsync(TKey id)
        {
            var dto = await context.Set<TEntity>()
                .AsNoTracking()
                .AsSplitQuery()
                .Where(x => x.Id.Equals(id))
                .ProjectTo<TDetails>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
            if (dto == null) return Result<TDetails>.Fail(FailureReasons.NotFound, "Id", $"Id {typeof(TEntity).Name} non trovato");
            return Result<TDetails>.Ok(dto);
        }

        public async Task<Result<TModel>> PostAsync(TPost model)
        {
            var entity = mapper.Map<TEntity>(model);

            var error = await PrePostAsync(entity, model);
            if (error != null) return Result<TModel>.Fail(FailureReasons.BadRequest, error);

            context.Add(entity);
            try
            {
                _ = await context.SaveChangesAsync();

                var dto = mapper.Map<TModel>(entity);

                return Result<TModel>.Ok(dto);
            }
            catch (Exception ex)
            {
                return Result<TModel>.Fail(FailureReasons.DatabaseError, ex, new ValidationError("DatabaseError", ex.InnerException?.Message ?? ex.Message));
            }
        }

        public async Task<Result> PutAsync(TPut model)
        {
            var entity = await context.Set<TEntity>()
                .Where(x => x.Id.Equals(model.Id))
                .SingleOrDefaultAsync();
            if (entity == null) return Result.Fail(FailureReasons.NotFound, "Id", $"Id {typeof(TEntity).Name} non trovato");

            var error = await PrePutAsync(entity, model);
            if (error != null) return Result.Fail(FailureReasons.BadRequest, error);

            mapper.Map(model, entity); // AutoMapper
            try
            {
                _ = await context.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(FailureReasons.DatabaseError, ex, new ValidationError("DatabaseError", ex.InnerException?.Message ?? ex.Message));
            }
        }

        public async Task<Result<TModel>> DeleteByIdAsync(TKey id)
        {
            //string query = context.Set<TEntity>()
            //     .Where(x => x.Id.Equals(id))
            //     .ToQueryString();
            var entity = await context.Set<TEntity>()
                 .Where(x => x.Id.Equals(id))
                 .SingleOrDefaultAsync();
            if (entity == null) return Result<TModel>.Fail(FailureReasons.NotFound, "Id", $"Id {typeof(TEntity).Name} non trovato");

            var error = await PreDeleteAsync(entity);
            if (error != null) return Result<TModel>.Fail(FailureReasons.BadRequest, error);

            if (entity is ArchiveableEntity<TKey>)
            {
                (entity as ArchiveableEntity<TKey>)!.IsDeleted = true;
            }
            else
            {
                context.Remove(entity);
            }
            try
            {
                _ = await context.SaveChangesAsync();
                var dto = mapper.Map<TModel>(entity); // AutoMapper
                return Result<TModel>.Ok(dto);
            }
            catch (Exception ex)
            {
                return Result<TModel>.Fail(FailureReasons.DatabaseError, ex, new ValidationError("DatabaseError", ex.InnerException?.Message ?? ex.Message));
            }
        }

        protected virtual Task<ValidationError?> PrePostAsync(TEntity entity, TPost model)
        {
            return Task.FromResult((ValidationError?)null);
        }

        protected virtual Task<ValidationError?> PrePutAsync(TEntity entity, TPut model)
        {
            return Task.FromResult((ValidationError?)null);
        }

        protected virtual Task<ValidationError?> PreDeleteAsync(TEntity entity)
        {
            return Task.FromResult((ValidationError?)null);
        }
    }

    public interface IArchiveableDataService<TKey, TModel, TList, TDetails, TPost, TPut, TRequest> :
        IDataService<TKey, TModel, TList, TDetails, TPost, TPut, TRequest>
        where TKey : struct
        where TModel : IEntity<TKey>
        where TDetails : IEntity<TKey>
        where TPut : IEntity<TKey>
        where TRequest : RequestBaseDto
    {
        Task<Result> UndeleteByIdAsync(TKey id);
    }

    internal abstract class ArchiveableDataService<TKey, TEntity, TModel, TList, TDetails, TPost, TPut, TRequest> :
        DataService<TKey, TEntity, TModel, TList, TDetails, TPost, TPut, TRequest>,
        IArchiveableDataService<TKey, TModel, TList, TDetails, TPost, TPut, TRequest>
        where TKey : struct
        where TEntity : ArchiveableEntity<TKey>
        where TModel : IEntity<TKey>
        where TDetails : IEntity<TKey>
        where TPut : IEntity<TKey>
        where TRequest : RequestBaseDto
    {
        public ArchiveableDataService(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<Result> UndeleteByIdAsync(TKey id)
        {
            TEntity? entity = await context.Set<TEntity>().Where(x => x.Id!.Equals(id)).FirstOrDefaultAsync();
            if (entity == null) return Result.Fail(FailureReasons.NotFound, new ValidationError("Id", "Id non trovato"));
            entity.IsDeleted = false;
            _ = await context.SaveChangesAsync();
            return Result.Ok();
        }
    }
}
