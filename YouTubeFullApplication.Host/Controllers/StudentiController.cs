using Microsoft.AspNetCore.Mvc;
using YouTubeFullApplication.BusinessLayer.Services;
using YouTubeFullApplication.Dto;
using YouTubeFullApplication.ServiceResult;

namespace YouTubeFullApplication.Host.Controllers
{
    //public class StudentiController : ControllerBase
    //{
    //    private readonly IStudentiService service;

    //    public StudentiController(IStudentiService service)
    //    {
    //        this.service = service;
    //    }

    //    [HttpGet]
    //    [ProducesResponseType(typeof(PagedResultDto<StudenteListDto>), StatusCodes.Status200OK)]
    //    public async Task<IActionResult> GetAll([FromQuery] StudenteRequestDto request)
    //    {
    //        var result = await service.GetAllAsync(request);
    //        return Ok(result);
    //    }

    //    [HttpGet("{id}")]
    //    [ProducesResponseType(typeof(StudenteDto), StatusCodes.Status200OK)]
    //    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //    public async Task<IActionResult> Get(Guid id)
    //    {
    //        var result = await service.GetByIdAsync(id);
    //        if (result.Success) return Ok(result);
    //        return CreateNotFound(ModelState, result);
    //    }

    //    [HttpGet("{id}/Details")]
    //    [ProducesResponseType(typeof(StudenteDto), StatusCodes.Status200OK)]
    //    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //    public async Task<IActionResult> GetDetails(Guid id)
    //    {
    //        var result = await service.GetDetailsByIdAsync(id);
    //        if (result.Success) return Ok(result);
    //        return CreateNotFound(ModelState, result);
    //    }

    //    [HttpPost]
    //    [ProducesResponseType(typeof(StudenteDto), StatusCodes.Status201Created)]
    //    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    //    public async Task<IActionResult> Post([FromBody] StudentePostDto model)
    //    {
    //        var result = await service.PostAsync(model)!;
    //        if (result.Success)
    //        {
    //            string url = $"{BaseUrl}\\Studenti\\{result.Content.Id}";
    //            return Created(url, result.Content);
    //        }
    //        if (result.FailureReason == FailureReasons.BadRequest)
    //        {
    //            return CreateBadRequest(ModelState, result);
    //        }
    //        else
    //        {
    //            return StatusCode(StatusCodes.Status406NotAcceptable, result.ErrorMessage);
    //        }
    //    }

    //    [HttpPut]
    //    [ProducesResponseType(StatusCodes.Status204NoContent)]
    //    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //    public async Task<IActionResult> Put([FromBody] StudentePutDto model)
    //    {
    //        var result = await service.PutAsync(model)!;
    //        if (result.Success) return NoContent();
    //        return CreateBadRequest(ModelState, result);
    //    }

    //    [HttpDelete]
    //    [ProducesResponseType(typeof(StudenteDto), StatusCodes.Status200OK)]
    //    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //    public async Task<IActionResult> Delete([FromQuery] Guid id)
    //    {
    //        var result = await service.DeleteByIdAsync(id);
    //        if (result.Success) return Ok(result.Content);
    //        if (result.FailureReason == FailureReasons.NotFound)
    //        {
    //            return CreateNotFound(ModelState, result);
    //        }
    //        else
    //        {
    //            return CreateBadRequest(ModelState, result);
    //        }
    //    }
    //}

    public class StudentiController : ArchiveableDataController<
        Guid,
        StudenteDto,
        StudenteListDto,
        StudenteDetailsDto,
        StudentePostDto,
        StudentePutDto,
        StudenteRequestDto>
    {
        public StudentiController(IStudentiService service) : base(service)
        {
        }

        [HttpGet("Suggest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Suggest(string q)
        {
            var result = await (service as IStudentiService)!.SuggestAsync(q);
            return Ok(result.Content);
        }
    }
}
