namespace Dotlanche.Producao.Domain.ValueObjects

{
    public record ValueResult<T> : Result
    {
        public ValueResult(bool isSuccess, T value, string? message = null)
            : base(isSuccess, message ?? string.Empty)
        {
            Value = value;
        }

        public T Value { get; set; }
    }
}