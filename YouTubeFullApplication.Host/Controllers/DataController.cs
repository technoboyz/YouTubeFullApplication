using Microsoft.AspNetCore.Mvc;
using YouTubeFullApplication.BusinessLayer.Services;
using YouTubeFullApplication.Dto;
using YouTubeFullApplication.ServiceResult;
using YouTubeFullApplication.Shared;

namespace YouTubeFullApplication.Host.Controllers
{
    public abstract class DataController<TKey, TModel, TList, TDetails, TPost, TPut, TRequest> :
        ControllerBase
        where TKey : struct
        where TModel : IEntity<TKey>
        where TDetails : IEntity<TKey>
        where TPut : IEntity<TKey>
        where TRequest : RequestBaseDto
    {
        protected readonly IDataService<TKey, TModel, TList, TDetails, TPost, TPut, TRequest> service;

        public DataController(IDataService<TKey, TModel, TList, TDetails, TPost, TPut, TRequest> service)
        {
            this.service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] TRequest request)
        {
            var result = await service.GetAllAsync(request);
            return Ok(result.Content);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(TKey id)
        {
            var result = await service.GetByIdAsync(id);
            if (result.Success) return Ok(result.Content);
            return CreateNotFound(ModelState, result);
        }

        [HttpGet("{id}/Details")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetails(TKey id)
        {
            var result = await service.GetDetailsByIdAsync(id);
            if (result.Success) return Ok(result.Content);
            return CreateNotFound(ModelState, result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public async Task<IActionResult> Post([FromBody] TPost model)
        {
            var result = await service.PostAsync(model)!;
            if (result.Success)
            {
                string url = $"{BaseUrl}{HttpContext.Request.Path}/{result.Content.Id}";
                return Created(url, result.Content);
            }
            if (result.FailureReason == FailureReasons.BadRequest)
            {
                return CreateBadRequest(ModelState, result);
            }
            else
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, result.ErrorMessage);
            }
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] TPut model)
        {
            var result = await service.PutAsync(model)!;
            if (result.Success) return NoContent();
            return CreateBadRequest(ModelState, result);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromQuery] TKey id)
        {
            var result = await service.DeleteByIdAsync(id);
            if (result.Success) return Ok(result.Content);
            if (result.FailureReason == FailureReasons.NotFound)
            {
                return CreateNotFound(ModelState, result);
            }
            else
            {
                return CreateBadRequest(ModelState, result);
            }
        }
    }

    public abstract class ArchiveableDataController<TKey, TModel, TList, TDetails, TPost, TPut, TRequest> :
        DataController<TKey, TModel, TList, TDetails, TPost, TPut, TRequest>
        where TKey : struct
        where TModel : IEntity<TKey>
        where TDetails : IEntity<TKey>
        where TPut : IEntity<TKey>
        where TRequest : RequestBaseDto
    {
        protected ArchiveableDataController(IArchiveableDataService<TKey, TModel, TList, TDetails, TPost, TPut, TRequest> service) : base(service)
        {
        }

        [HttpDelete("Undelete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Undelete([FromQuery] TKey id)
        {
            var result = await (service as IArchiveableDataService<TKey, TModel, TList, TDetails, TPost, TPut, TRequest>)!.UndeleteByIdAsync(id);
            if (result.Success) return NoContent();
            return CreateNotFound(ModelState, result);
        }
    }
}
