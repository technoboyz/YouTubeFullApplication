using Microsoft.AspNetCore.Mvc;
using YouTubeFullApplication.BusinessLayer.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Host.Controllers
{
    public class ClassiController : DataController<
        Guid,
        ClasseDto,
        ClasseListDto,
        ClasseDetailsDto,
        ClassePostDto,
        ClassePutDto,
        ClasseRequestDto>
    {
        public ClassiController(IClassiService service) : base(service)
        {
        }

        [HttpGet("Items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetItems()
        {
            var result = await (service as IClassiService)!.GetItemsAsync();
            return Ok(result.Content);
        }
    }
}
