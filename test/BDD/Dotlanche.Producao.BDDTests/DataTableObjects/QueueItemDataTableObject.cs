using Dotlanche.Producao.Domain.Entities;

namespace Dotlanche.Producao.BDDTests.DataTableObjects
{
    internal class QueueItemDataTableObject
    {
        public Guid PedidoId { get; set; }
        public int QueueKey { get; set; }
        public StatusProducaoPedido Status { get; set; }
    }
}