using Dotlanche.Producao.Domain.Entities;

namespace Dotlanche.Producao.WebApi.DTOs.Responses
{
    public record GetPedidosQueueResponse
    {
        public required IEnumerable<PedidoQueueItem> Items { get; set; }
    }

    public record PedidoQueueItem
    {
        public required Guid PedidoId { get; set; }

        public required int QueueKey { get; set; }

        public required StatusProducaoPedido Status { get; set; }
    }
}