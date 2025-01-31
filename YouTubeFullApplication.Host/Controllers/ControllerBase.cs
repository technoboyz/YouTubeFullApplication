using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net.Mime;

namespace YouTubeFullApplication.Host.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public abstract class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        protected string BaseUrl => $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";

        protected IActionResult CreateBadRequest(ModelStateDictionary modelState, ServiceResult.IResult result)
        {
            foreach (var error in result.Errors!) modelState.AddModelError(error.Name, error.Message);
            return BadRequest(new ValidationProblemDetails(modelState));
        }

        protected IActionResult CreateNotFound(ModelStateDictionary modelState, ServiceResult.IResult result)
        {
            foreach (var error in result.Errors!) modelState.AddModelError(error.Name, error.Message);
            return NotFound(new ValidationProblemDetails(modelState));
        }
    }
}
