namespace Dotlanche.Producao.Data.Exceptions
{
    public class DatabaseException(string message, Exception? inner = null) : Exception(message, inner)
    {
    }
}
