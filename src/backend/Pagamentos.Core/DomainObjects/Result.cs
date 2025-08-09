namespace Pagamentos.Core.DomainObjects
{
    public class Result
    {
        public bool Success { get; }
        public string Message { get; }

        private Result(bool success, string message = null)
        {
            Success = success;
            Message = message;
        }

        public static Result Ok() => new Result(true);
        public static Result Fail(string message) => new Result(false, message);
    }

    public class Result<T>
    {
        public bool Success { get; }
        public string? Message { get; }
        public T? Data { get; }

        private Result(bool success, T? data, string? message = null)
        {
            Success = success;
            Data = data;
            Message = message;
        }

        public static Result<T> Ok(T data)
        {
            return new Result<T>(true, data);
        }

        public static Result<T> Fail(string message)
        {
            return new Result<T>(false, default, message);
        }
    }
}
