using System.Diagnostics.CodeAnalysis;

namespace YouTubeFullApplication.ServiceResult
{
    public interface IResult
    {
        IEnumerable<ValidationError>? Errors { get; }
    }

    public class Result : IResult
    {
        public bool Success { get; }
        public int FailureReason { get; }

        public Exception? Exception { get; }

        private readonly string? errorMessage;
        public string? ErrorMessage => errorMessage ?? Exception?.InnerException?.Message ?? Exception?.Message;

        public IEnumerable<ValidationError>? Errors { get; }

        internal Result(bool success = true, int failureReason = FailureReasons.None, string? message = null, Exception? error = null, IEnumerable<ValidationError>? errors = null)
        {
            Success = success;
            FailureReason = failureReason;
            errorMessage = message;
            Errors = errors;
        }

        public static Result Ok() => new Result();
        public static Result Fail(int failureReason, string name, string message)
            => Result.Fail(failureReason, new ValidationError(name, message));
        public static Result Fail(int failureReason, ValidationError error)
            => new Result(success: false, failureReason: failureReason, errors: [error]);
        public static Result Fail(int failureReason, IEnumerable<ValidationError> errors)
            => new Result(success: false, failureReason: failureReason, errors: errors);
        public static Result Fail(int failureReason, Exception? error, ValidationError validationError)
            => new Result(success: false, failureReason: failureReason, error: error, errors: [validationError]);
    }

    public class Result<T> : IResult
    {
        [MemberNotNullWhen(true, nameof(Content))]
        public bool Success { get; }

        public T? Content { get; }

        public int FailureReason { get; }

        public Exception? Exception { get; }

        private readonly string? errorMessage;
        public string? ErrorMessage => errorMessage ?? Exception?.InnerException?.Message ?? Exception?.Message;

        public IEnumerable<ValidationError>? Errors { get; }

        internal Result(bool success = true, T? content = default, int failureReason = FailureReasons.None, string? message = null, Exception? error = null, IEnumerable<ValidationError>? errors = null)
        {
            Success = success;
            Content = content;
            FailureReason = failureReason;
            errorMessage = message;
            Errors = errors;
        }

        public static Result<T> Ok(T? content = default) => new Result<T>(content: content);

        public static Result<T> Fail(int failureReason, string name, string message)
            => Result<T>.Fail(failureReason, new ValidationError(name, message));

        public static Result<T> Fail(int failureReason, ValidationError error)
            => new Result<T>(success: false, failureReason: failureReason, errors: [error]);

        public static Result<T> Fail(int failureReason, IEnumerable<ValidationError> errors)
            => new Result<T>(success: false, failureReason: failureReason, errors: errors);

        public static Result<T> Fail(int failureReason, Exception? error, ValidationError validationError)
            => new Result<T>(success: false, failureReason: failureReason, error: error, errors: [validationError]);

        public static implicit operator Result<T>(T value)
            => Ok(value);
    }
}
