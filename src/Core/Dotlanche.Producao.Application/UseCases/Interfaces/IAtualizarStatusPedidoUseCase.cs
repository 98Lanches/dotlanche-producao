using Dotlanche.Producao.Domain;
using Dotlanche.Producao.Domain.Entities;

namespace Dotlanche.Producao.Application.UseCases.Interfaces
{
    public interface IAtualizarStatusPedidoUseCase
    {
        Task<Result> ExecuteAsync(Guid idPedido, StatusProducaoPedido newStatus);
    }
}