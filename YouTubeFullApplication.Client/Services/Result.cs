using System.Net;
namespace YouTubeFullApplication.Client.Services
{
    public class Result
    {
        public string? ErrorMessage => Errors?.First().Value.First();
        public bool Success { get; }
        public IDictionary<string, string[]>? Errors { get; }
        public HttpStatusCode StatusCode { get; set; }
        private Result(bool success = false, IDictionary<string, string[]>? errors = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Success = success;
            Errors = errors;
            StatusCode = statusCode;
        }

        public static Result Ok(HttpStatusCode statusCode) => new Result(success: true, statusCode: statusCode);
        public static Result Fail(IDictionary<string, string[]> errors)
            => new Result(errors: errors, statusCode: HttpStatusCode.BadRequest);
        public static Result Fail(IDictionary<string, string[]> errors, HttpStatusCode statusCode)
            => new Result(errors: errors, statusCode: statusCode);
        public static Result Fail(string name, string message)
            => new Result(errors: new Dictionary<string, string[]> { { name, [message] } }, statusCode: HttpStatusCode.BadRequest);
        public static Result Fail(string name, string message, HttpStatusCode statusCode)
            => new Result(errors: new Dictionary<string, string[]> { { name, [message] } }, statusCode: statusCode);
        public static Result Fail(string name, string[] messages)
            => new Result(errors: new Dictionary<string, string[]> { { name, messages } }, statusCode: HttpStatusCode.BadRequest);
    }

    public class Result<T>
    {
        public string? ErrorMessage => Errors?.First().Value.First();
        public T? Content { get; }
        public bool Success { get; }
        public IDictionary<string, string[]>? Errors { get; }
        public HttpStatusCode StatusCode { get; set; }

        private Result(T? content = default, bool success = false, IDictionary<string, string[]>? errors = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Content = content;
            Success = success;
            Errors = errors;
            StatusCode = statusCode;
        }
        public static Result<T> Ok(T content, HttpStatusCode statusCode) => new Result<T>(content: content, success: true, statusCode: statusCode);
        public static Result<T> Fail(IDictionary<string, string[]> errors)
            => new Result<T>(errors: errors, statusCode: HttpStatusCode.BadRequest);
        public static Result<T> Fail(IDictionary<string, string[]> errors, HttpStatusCode statusCode)
            => new Result<T>(errors: errors, statusCode: statusCode);
        public static Result<T> Fail(string name, string message)
            => new Result<T>(errors: new Dictionary<string, string[]> { { name, [message] } }, statusCode: HttpStatusCode.BadRequest);
        public static Result<T> Fail(string name, string message, HttpStatusCode statusCode)
            => new Result<T>(errors: new Dictionary<string, string[]> { { name, [message] } }, statusCode: statusCode);
        public static Result<T> Fail(string name, string[] messages)
            => new Result<T>(errors: new Dictionary<string, string[]> { { name, messages } }, statusCode: HttpStatusCode.BadRequest);
    }

    public class ValidationError
    {
        public IDictionary<string, string[]> Errors { get; set; } = default!;
    }
}
