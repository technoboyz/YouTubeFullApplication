using Microsoft.AspNetCore.Mvc;
using YouTubeFullApplication.BusinessLayer.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Host.Controllers
{
    public class MaterieController : DataController<
        Guid,
        MateriaDto,
        MateriaListDto,
        MateriaDetailsDto,
        MateriaPostDto,
        MateriaPutDto,
        MateriaRequestDto>
    {
        public MaterieController(IMaterieService service) : base(service)
        {
        }

        [HttpGet("Items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetItems()
        {
            var result = await (service as IMaterieService)!.GetItemsAsync();
            return Ok(result.Content);
        }
    }
}
