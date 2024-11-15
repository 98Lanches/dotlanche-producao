using Dotlanche.Producao.Application.UseCases.Interfaces;
using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.Domain.Repositories;

namespace Dotlanche.Producao.Application.UseCases
{
    public class ObterFilaPedidosUseCase : IObterFilaPedidosUseCase
    {
        private readonly IPedidoEmProducaoRepository repository;

        public ObterFilaPedidosUseCase(IPedidoEmProducaoRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<PedidoEmProducao>> ExecuteAsync()
        {
            return await repository.GetPedidosQueue();
        }
    }
}
