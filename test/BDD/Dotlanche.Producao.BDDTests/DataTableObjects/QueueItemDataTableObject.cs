using Dotlanche.Producao.Domain.Entities;

namespace Dotlanche.Producao.BDDTests.DataTableObjects
{
    internal class QueueItemDataTableObject
    {
        public int QueueKey { get; set; }
        public StatusProducaoPedido Status { get; set; }
    }
}
