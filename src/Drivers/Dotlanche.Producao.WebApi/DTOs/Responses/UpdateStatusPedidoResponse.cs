using Dotlanche.Producao.Domain.Entities;

namespace Dotlanche.Producao.WebApi.DTOs.Responses
{
    public class UpdateStatusPedidoResponse
    {
        public Guid Id { get; set; }

        public DateTime CreationTime { get; set; }

        public int QueueKey { get; set; }

        public StatusProducaoPedido Status { get; set; }

        public DateTime LastUpdateTime { get; set; }
    }
}