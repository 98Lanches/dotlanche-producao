namespace Dotlanche.Producao.WebApi.DTOs
{
    public record ComboDto
    {
        public required Guid Id { get; set; }

        public required IEnumerable<Guid> ProdutoIds { get; set; }
    }
}