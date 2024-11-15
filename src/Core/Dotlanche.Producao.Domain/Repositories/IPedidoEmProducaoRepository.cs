using Dotlanche.Producao.Domain.Entities;

namespace Dotlanche.Producao.Domain.Repositories
{
    public interface IPedidoEmProducaoRepository
    {
        Task<PedidoEmProducao> Add(PedidoEmProducao novoPedido);
        Task<IEnumerable<PedidoEmProducao>> GetPedidosQueue();
    }
}