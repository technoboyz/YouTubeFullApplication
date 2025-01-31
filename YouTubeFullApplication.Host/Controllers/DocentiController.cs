using Microsoft.AspNetCore.Mvc;
using YouTubeFullApplication.BusinessLayer.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Host.Controllers
{
    public class DocentiController : ArchiveableDataController<
        Guid,
        DocenteDto,
        DocenteListDto,
        DocenteDetailsDto,
        DocentePostDto,
        DocentePutDto,
        DocenteRequestDto>
    {
        public DocentiController(IDocentiService service) : base(service)
        {
        }

        [HttpGet("Items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetItems()
        {
            var result = await (service as IDocentiService)!.GetItemsAsync();
            return Ok(result.Content);
        }
    }
}
