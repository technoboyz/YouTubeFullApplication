using MediatR;
using Microsoft.AspNetCore.Mvc;
using YouTubeFullApplication.Dto;
using YouTubeFullApplication.ServiceResult;
using YouTubeFullApplication.Shared;

namespace YouTubeFullApplication.Host.Controllers
{
    public class MediatrDataReadController<TKey, TModel, TList, TDetails, TRequest> : ControllerBase
        where TKey : struct
        where TModel : IEntity<TKey>
        where TRequest : RequestBaseDto
    {
        protected readonly ISender sender;

        public MediatrDataReadController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] TRequest request)
        {
            var result = await sender.Send(new ModelListRequest<TRequest, TList> { Request = request });
            return Ok(result.Content);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(TKey id)
        {
            var result = await sender.Send(new ModelByIdRequest<TKey, TModel> { Id = id });
            if (result.Success) return Ok(result.Content);
            return CreateNotFound(ModelState, result);
        }

        [HttpGet("{id}/Details")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetails(TKey id)
        {
            var result = await sender.Send(new ModelDetailsByIdRequest<TKey, TDetails> { Id = id });
            if (result.Success) return Ok(result.Content);
            return CreateNotFound(ModelState, result);
        }
    }

    public class MediatrDataWriteController<TKey, TModel, TPost, TPut> : ControllerBase
        where TKey : struct
        where TModel : IEntity<TKey>
        where TPost : class
        where TPut : IEntity<TKey>
    {
        protected readonly ISender sender;

        public MediatrDataWriteController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public async Task<IActionResult> Post([FromBody] TPost model)
        {
            var result = (Result<TModel>)(await sender.Send(model))!;
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
            var result = (Result)(await sender.Send(model))!;
            if (result.Success) return NoContent();
            return CreateBadRequest(ModelState, result);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromQuery] TKey id)
        {
            var result = (Result<TModel>)(await sender.Send(new ModelDeleteByIdRequest<TKey, TModel> { Id = id }))!;
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

    public class MediatrArchiveableDataWriteController<TKey, TModel, TPost, TPut> :
        MediatrDataWriteController<TKey, TModel, TPost, TPut>
        where TKey : struct
        where TModel : IEntity<TKey>
        where TPost : class
        where TPut : IEntity<TKey>
    {
        public MediatrArchiveableDataWriteController(ISender sender) : base(sender)
        {
        }

        [HttpDelete("Undelete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Undelete([FromQuery] TKey id)
        {
            var result = await sender.Send(new ModelUndeleteByIdRequest<TKey, TModel> { Id = id });
            if (result.Success) return NoContent();
            return CreateNotFound(ModelState, result);
        }
    }
}
