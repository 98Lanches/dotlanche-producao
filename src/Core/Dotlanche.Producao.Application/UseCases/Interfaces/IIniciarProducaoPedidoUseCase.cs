using Dotlanche.Producao.Domain.Entities;

namespace Dotlanche.Producao.Application.UseCases
{
    public interface IIniciarProducaoPedidoUseCase
    {
        public Task<PedidoEmProducao> ExecuteAsync(PedidoAceito pedidoAceito);
    }
}