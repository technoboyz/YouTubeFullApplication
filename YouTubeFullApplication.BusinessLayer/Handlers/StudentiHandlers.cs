using MediatR;
using YouTubeFullApplication.BusinessLayer.Services;
using YouTubeFullApplication.Dto;
using YouTubeFullApplication.ServiceResult;

namespace YouTubeFullApplication.BusinessLayer.Handlers
{
    internal class StudentiReadHandler : DataReadHandler<
        Guid,
        StudenteDto,
        StudenteListDto,
        StudenteDetailsDto,
        StudenteRequestDto>,
        IRequestHandler<SuggestRequest<StudenteDto>, Result<IEnumerable<StudenteDto>>>
    {
        public StudentiReadHandler(IStudentiReadService service) : base(service)
        {
        }

        public Task<Result<IEnumerable<StudenteDto>>> Handle(SuggestRequest<StudenteDto> request, CancellationToken cancellationToken)
        {
            return (service as IStudentiReadService)!.SuggestAsync(request.Text);
        }
    }

    internal class StudentiWriteHandler : ArchiveableDataWriteHandler<
        Guid,
        StudenteDto,
        StudentePostDto,
        StudentePutDto>
    {
        public StudentiWriteHandler(IStudentiWriteService service) : base(service)
        {
        }
    }
}
