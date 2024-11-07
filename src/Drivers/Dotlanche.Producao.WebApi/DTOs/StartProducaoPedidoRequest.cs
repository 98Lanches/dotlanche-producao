#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
namespace Dotlanche.Producao.WebApi.DTOs
{
    public record StartProducaoPedidoRequest
    {
        public Guid PedidoId { get; set; }

        public IEnumerable<RequestCombo> Combos { get; set; }
    }

    public record RequestCombo
    {
        public Guid Id { get; set; }

        public IEnumerable<Guid> ProdutoGuids { get; set; }
    }
}