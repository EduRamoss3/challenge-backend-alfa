

namespace Order.Domain.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public string? Error { get; }
        public T? Value { get; }
        public int StatusCode { get; set; }

        private Result(bool isSuccess, T? value, string? error, int statusCode)
        {
            IsSuccess = isSuccess;
            Error = error;
            Value = value;
            StatusCode = statusCode;
        }

        public static Result<T> Success(T value, int statusCode) => new Result<T>(true, value, null, statusCode);

        public static Result<T> Failure(string error, int statusCode) => new Result<T>(false, default, error, statusCode);
    }
}
