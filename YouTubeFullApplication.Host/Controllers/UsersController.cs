using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YouTubeFullApplication.BusinessLayer.Services;
using YouTubeFullApplication.Dto;
using YouTubeFullApplication.ServiceResult;
using YouTubeFullApplication.Validation;

namespace YouTubeFullApplication.Host.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly IUsersService service;

        public UsersController(IUsersService service)
        {
            this.service = service;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(typeof(UserLoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var result = await service.LoginAsync(request);
            if (result.Success) return Ok(result.Content);
            return CreateBadRequest(ModelState, result);
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDto<UserListDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] UserRequestDto request)
        {
            var result = await service.GetAllAsync(request);
            return Ok(result.Content);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await service.GetByIdAsync(id);
            if(result.Success) return Ok(result.Content);
            return CreateNotFound(ModelState, result);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await service.DeleteAsync(id);
            if (result.Success) return Ok();
            return CreateNotFound(ModelState, result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(UserRegisterRequestDto model)
        {
            var result = await service.RegisterAsync(model);
            if (result.Success) return Ok(result.Content);
            if (result.FailureReason == FailureReasons.NotFound)
                return CreateNotFound(ModelState, result);
            else
                return CreateBadRequest(ModelState, result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(UserPutDto model)
        {
            var result = await service.PutAsync(model);
            if (result.Success) return Ok();
            if (result.FailureReason == FailureReasons.NotFound)
                return CreateNotFound(ModelState, result);
            else
                return CreateBadRequest(ModelState, result);
        }

        [AllowAnonymous]
        [HttpGet("RefreshToken")]
        [ProducesResponseType(typeof(UserLoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromQuery] UserRefreshTokenRequest request)
        {
            var result = await service.RefreshTokenAsync(request);
            if (result.Success) return Ok(result.Content);
            return CreateBadRequest(ModelState, result);
        }

        [HttpGet("Roles")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRoles()
        {
            var result = await service.GetRolesAsync();
            return Ok(result.Content);
        }
    }
}
