using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics;

namespace YouTubeFullApplication.ServiceResult.AspNetCore
{
    public static class ResultExtensions
    {
        public static IActionResult ToResponse(this Result result, HttpContext httpContext, int? successStatusCode = null)
        {
            if (result.Success)
            {
                return new StatusCodeResult(successStatusCode.GetValueOrDefault(StatusCodes.Status204NoContent));
            }

            return Problem(httpContext, result.FailureReason, result.ErrorMessage, result.Errors);
        }

        public static IActionResult ToResponse<T>(this Result<T> result, HttpContext httpContext, string? routeName = null, object? routeValue = null, int? successStatusCode = null)
        {
            if (result.Success)
            {
                if (routeName != null)
                {
                    string requestSheme = httpContext.Request.Scheme;
                    string requestHost = httpContext.Request.Host.Host;
                    string? id = routeValue?.GetType().GetProperty("Id")?.GetValue(routeValue)?.ToString();
                    string location = $"{requestSheme}://{requestHost}{routeName}/{id}";
                    var createdAtActionResult = new CreatedResult(location, result.Content)
                    {
                        StatusCode = successStatusCode.GetValueOrDefault(StatusCodes.Status201Created)
                    };
                    return createdAtActionResult;
                }
                else if (result.Content is StreamFileContent streamFileContent)
                {
                    var fileStreamResult = new FileStreamResult(streamFileContent.Content, streamFileContent.ContentType)
                    {
                        FileDownloadName = streamFileContent.DownloadFileName,
                    };

                    return fileStreamResult;
                }
                else if (result.Content is ByteArrayFileContent byteArrayFileContent)
                {
                    var fileContentResult = new FileContentResult(byteArrayFileContent.Content, byteArrayFileContent.ContentType)
                    {
                        FileDownloadName = byteArrayFileContent.DownloadFileName
                    };

                    return fileContentResult;
                }
                else
                {
                    return new ObjectResult(result.Content)
                    {
                        StatusCode = successStatusCode.GetValueOrDefault(StatusCodes.Status200OK)
                    };
                }
            }

            return Problem(httpContext, result.FailureReason, result.ErrorMessage, result.Errors);
        }


        private static IActionResult Problem(
            HttpContext httpContext,
            int failureReason,
            string? errorMessage,
            IEnumerable<ValidationError>? validationErrors = null)
        {
            int statusCode = failureReason switch
            {
                FailureReasons.BadRequest => StatusCodes.Status400BadRequest,
                FailureReasons.Unauthorized => StatusCodes.Status401Unauthorized,
                FailureReasons.Forbidden => StatusCodes.Status403Forbidden,
                FailureReasons.NotFound => StatusCodes.Status404NotFound,
                FailureReasons.DatabaseError => StatusCodes.Status500InternalServerError,
                FailureReasons.InvalidFile => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status406NotAcceptable
            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Type = $"https://httpstatuses.io/{statusCode}",
                Title = ReasonPhrases.GetReasonPhrase(statusCode),
                Detail = "Ci sono uno o più errori di validazione",
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions.Add("traceId", Activity.Current?.Id ?? httpContext.TraceIdentifier);

            if (validationErrors != null)
            {
                var errors = validationErrors
                    .GroupBy(v => v.Name)
                    .ToDictionary(k => k.Key, v => v.Select(e => e.Message));
                problemDetails.Extensions.Add("errors", errors);
            }

            var problemDetailsResults = new JsonResult(problemDetails)
            {
                StatusCode = statusCode,
                ContentType = "application/problem+json; charset=utf-8"
            };

            return problemDetailsResults;
        }
    }
}
