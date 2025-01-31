using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YouTubeFullApplication.ServiceResult.AspNetCore
{
    public static class HttpContextExtensions
    {
        public static IActionResult CreateResponse(
            this HttpContext httpContext, 
            Result result, 
            int? successStatusCode = null)
        {
            return result.ToResponse(httpContext, successStatusCode);
        }

        public static IActionResult CreateResponse<T>(
            this HttpContext httpContext, 
            Result<T> result, string? 
            routeName = null, 
            object? routeValue = null, 
            int? successStatusCode = null)
        {
            return result.ToResponse(httpContext, routeName: routeName, routeValue: routeValue, successStatusCode: successStatusCode);
        }
    }
}
