namespace Dotlanche.Producao.WebApi.DTOs
{
    public record ComboDTO
    {
        public required Guid Id { get; set; }

        public required IEnumerable<Guid> ProdutoIds { get; set; }
    }
}