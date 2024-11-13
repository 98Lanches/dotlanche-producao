namespace Dotlanche.Producao.Integrations.Adapters.ProdutoService.Responses
{
    internal class GetProdutosByIdsResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? Categoria { get; set; }
    }
}