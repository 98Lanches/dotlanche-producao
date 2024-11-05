namespace Dotlanche.Producao.Application.UseCases
{
    public class EmptyUseCase : IUseCase
    {
        public EmptyUseCase()
        {
        }

        public object Execute()
        {
            throw new NotImplementedException();
        }
    }
}