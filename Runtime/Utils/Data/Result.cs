namespace LuckiusDev.Utils
{
    public class Result
    {
        public bool IsSuccess => string.IsNullOrWhiteSpace(Error);

        public string Error { get; protected set; }

        public static Result Success() => new();
        public static Result Failure(string error) => new() { Error = error };

        public static implicit operator bool(Result res) => res.IsSuccess;
    }

    public class Result<T>
    {
        public bool IsSuccess => string.IsNullOrWhiteSpace(Error);

        public string Error { get; protected set; }
        public T Value { get; protected set; }

        public static Result<T> Success(T value) => new() { Value = value };
        public static Result<T> Failure(string error) => new() { Error = error };

        public static implicit operator bool(Result<T> res) => res.IsSuccess;
        public static implicit operator T(Result<T> res) => res.Value;
    }
}
