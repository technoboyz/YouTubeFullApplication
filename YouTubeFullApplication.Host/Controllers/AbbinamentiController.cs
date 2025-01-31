using YouTubeFullApplication.BusinessLayer.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Host.Controllers
{
    public class AbbinamentiController : DataController<
        Guid,
        AbbinamentoDto,
        AbbinamentoListDto,
        AbbinamentoDetailsDto,
        AbbinamentoPostDto,
        AbbinamentoPutDto,
        AbbinamentoRequestDto>
    {
        public AbbinamentiController(IAbbinamentiService service) : base(service)
        {
        }
    }
}
