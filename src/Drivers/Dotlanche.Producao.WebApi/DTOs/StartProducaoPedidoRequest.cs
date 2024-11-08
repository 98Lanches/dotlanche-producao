namespace Dotlanche.Producao.WebApi.DTOs
{
    public record StartProducaoPedidoRequest
    {
        public required Guid PedidoId { get; set; }

        public required IEnumerable<RequestCombo> Combos { get; set; }
    }

    public record RequestCombo
    {
        public required Guid Id { get; set; }

        public required IEnumerable<Guid> ProdutoGuids { get; set; }
    }
}