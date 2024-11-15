using Dotlanche.Producao.Domain.Entities;

namespace Dotlanche.Producao.WebApi.DTOs.Requests
{
    public class UpdateStatusPedidoRequest
    {
        public required Guid IdPedido { get; set; }
        public required StatusProducaoPedido Status { get; set; }
    }
}