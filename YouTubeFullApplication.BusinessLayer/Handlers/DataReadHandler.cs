using MediatR;
using YouTubeFullApplication.BusinessLayer.Services;
using YouTubeFullApplication.Dto;
using YouTubeFullApplication.ServiceResult;
using YouTubeFullApplication.Shared;

namespace YouTubeFullApplication.BusinessLayer.Handlers
{
    internal abstract class DataReadHandler<TKey, TModel, TList, TDetails, TRequest> :
        IRequestHandler<ModelListRequest<TRequest, TList>, Result<PagedResultDto<TList>>>,
        IRequestHandler<ModelByIdRequest<TKey, TModel>, Result<TModel>>,
        IRequestHandler<ModelDetailsByIdRequest<TKey, TDetails>, Result<TDetails>>
        where TKey : struct
        where TModel : IEntity<TKey>
        where TDetails : IEntity<TKey>
        where TRequest : RequestBaseDto
    {
        protected readonly IDataReadService<TKey, TModel, TList, TDetails, TRequest> service;

        public DataReadHandler(IDataReadService<TKey, TModel, TList, TDetails, TRequest> service)
        {
            this.service = service;
        }

        public Task<Result<PagedResultDto<TList>>> Handle(ModelListRequest<TRequest, TList> request, CancellationToken cancellationToken)
        {
            return service.GetAllAsync(request.Request);
        }


        public Task<Result<TModel>> Handle(ModelByIdRequest<TKey, TModel> request, CancellationToken cancellationToken)
        {
            return service.GetByIdAsync(request.Id);
        }

        public Task<Result<TDetails>> Handle(ModelDetailsByIdRequest<TKey, TDetails> request, CancellationToken cancellationToken)
        {
            return service.GetDetailsByIdAsync(request.Id);
        }
    }


    internal abstract class DataDeleteHandler<TKey, TModel, TPost, TPut> :
        IRequestHandler<ModelDeleteByIdRequest<TKey, TModel>, Result<TModel>>
        where TKey : struct
        where TModel : IEntity<TKey>
         where TPut : IEntity<TKey>
    {
        protected readonly IDataWriteService<TKey, TModel, TPost, TPut> service;

        protected DataDeleteHandler(IDataWriteService<TKey, TModel, TPost, TPut> service)
        {
            this.service = service;
        }

        public Task<Result<TModel>> Handle(ModelDeleteByIdRequest<TKey, TModel> request, CancellationToken cancellationToken)
        {
            return service.DeleteByIdAsync(request.Id);
        }
    }

    internal abstract class DataWriteHandler<TKey, TModel, TPost, TPut> :
        DataDeleteHandler<TKey, TModel, TPost, TPut>,
        IRequestHandler<TPost, Result<TModel>>,
        IRequestHandler<TPut, Result>
        where TKey : struct
        where TModel : IEntity<TKey>
        where TPost : IRequest<Result<TModel>>
        where TPut : IEntity<TKey>, IRequest<Result>
    {

        protected DataWriteHandler(IDataWriteService<TKey, TModel, TPost, TPut> service) : base(service) { }

        public Task<Result<TModel>> Handle(TPost request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result> Handle(TPut request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    internal abstract class ArchiveableDataWriteHandler<TKey, TModel, TPost, TPut> :
        DataWriteHandler<TKey, TModel, TPost, TPut>,
        IRequestHandler<ModelUndeleteByIdRequest<TKey, TModel>, Result>
        where TKey : struct
        where TModel : IEntity<TKey>
        where TPost : IRequest<Result<TModel>>
        where TPut : IEntity<TKey>, IRequest<Result>
    {
        protected ArchiveableDataWriteHandler(IArchiveableDataWriteService<TKey, TModel, TPost, TPut> service) : base(service)
        {
        }

        public Task<Result> Handle(ModelUndeleteByIdRequest<TKey, TModel> request, CancellationToken cancellationToken)
        {
            return (service as IArchiveableDataWriteService<TKey, TModel, TPost, TPut>)!.UndeleteByIdAsync(request.Id);
        }
    }
}
