using YouTubeFullApplication.BusinessLayer.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Host.Controllers
{
    public class FrequenzeController : DataController<
        Guid,
        FrequenzaDto,
        FrequenzaListDto,
        FrequenzaDetailsDto,
        FrequenzaPostDto,
        FrequenzaPutDto,
        FrequenzaRequestDto>
    {
        public FrequenzeController(IFrequenzeService service) : base(service)
        {
        }
    }
}
