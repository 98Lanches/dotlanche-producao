using Dotlanche.Producao.Domain.Entities;

namespace Dotlanche.Producao.WebApi.DTOs.Responses
{
    public record StartProducaoPedidoResponse
    {
        public required bool Success { get; set; }
        public required StatusProducaoPedido Status { get; set; }
        public required int QueueKey { get; set; }
        public required IEnumerable<ComboDto> Combos { get; set; }
    }
}