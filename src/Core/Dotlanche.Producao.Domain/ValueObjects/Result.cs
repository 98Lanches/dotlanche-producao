namespace Dotlanche.Producao.Domain.ValueObjects
{
    public class Result<T>
    {
        public Result(bool isSuccess, T value)
        {
            IsSuccess = isSuccess;
            Value = value;
        }

        public bool IsSuccess { get; }

        public T Value { get; set; }
    }
}