using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YouTubeFullApplication.BusinessLayer.Services;

namespace YouTubeFullApplication.Host.Controllers
{
    public class InstallController : ControllerBase
    {
        private readonly IInstallService service;

        public InstallController(IInstallService service)
        {
            this.service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Install(string key)
        {
            var result = await service.ExecuteAsync(key);
            if (result.Success) return NoContent();
            return StatusCode(StatusCodes.Status401Unauthorized, result.ErrorMessage);
        }
    }
}
