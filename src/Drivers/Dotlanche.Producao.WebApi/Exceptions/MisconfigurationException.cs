namespace Dotlanche.Producao.WebApi.Exceptions
{
    public class MisconfigurationException : Exception
    {
        private const string messageTemplate = "Invalid Configuration. Missing property: {0}!";

        public MisconfigurationException(string propertyMissing)
            : base(string.Format(messageTemplate, propertyMissing))
        {
        }
    }
}