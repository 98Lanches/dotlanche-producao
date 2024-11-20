namespace Dotlanche.Producao.Data.Exceptions
{
    public class EntityNotFoundException(string? message = null) : Exception(message ?? "Entity not found!")
    {
    }
}