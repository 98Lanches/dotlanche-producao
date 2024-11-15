using Dotlanche.Producao.Domain.Entities;

namespace Dotlanche.Producao.BDDTests.DataTableObjects
{
    internal class PedidoCadastradoDataTableObject
    {
        public Guid PedidoId { get; set; }
        public int QueueKey { get; set; }
        public StatusProducaoPedido Status { get; set; }
        public DateTime CreationTime { get; set; }
    }
}