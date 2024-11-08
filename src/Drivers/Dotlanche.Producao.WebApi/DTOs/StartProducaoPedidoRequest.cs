namespace Dotlanche.Producao.WebApi.DTOs
{
    public record StartProducaoPedidoRequest
    {
        public required Guid PedidoId { get; set; }

        public required IEnumerable<ComboDTO> Combos { get; set; }
    }
}