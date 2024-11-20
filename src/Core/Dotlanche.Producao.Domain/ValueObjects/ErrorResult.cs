namespace Dotlanche.Producao.Domain.ValueObjects

{
    public record ErrorResult : Result
    {
        public ErrorResult(string message, Exception? exception = null)
            : base(false, message)
        {
            Exception = exception;
        }

        public Exception? Exception { get; set; }
    }
}