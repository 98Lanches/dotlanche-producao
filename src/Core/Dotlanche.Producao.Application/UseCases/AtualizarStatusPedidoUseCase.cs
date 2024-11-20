using Dotlanche.Producao.Application.UseCases.Interfaces;
using Dotlanche.Producao.Domain;
using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.Domain.Repositories;
using Dotlanche.Producao.Domain.ValueObjects;

namespace Dotlanche.Producao.Application.UseCases
{
    public class AtualizarStatusPedidoUseCase : IAtualizarStatusPedidoUseCase
    {
        private readonly IPedidoEmProducaoRepository repository;

        public AtualizarStatusPedidoUseCase(IPedidoEmProducaoRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Result> ExecuteAsync(Guid idPedido, StatusProducaoPedido newStatus)
        {
            var existingPedido = await repository.GetById(idPedido);
            if (existingPedido == null)
                return new ErrorResult("Pedido does not exist");

            existingPedido.UpdateStatus(newStatus);

            var updated = await repository.Update(existingPedido);
            return new ValueResult<PedidoEmProducao>(true, updated);
        }
    }
}