using Dotlanche.Producao.Domain.Entities;

namespace Dotlanche.Producao.Application.UseCases.Interfaces;

public interface IObterFilaPedidosUseCase
{
    Task<IEnumerable<PedidoEmProducao>> ExecuteAsync();
}