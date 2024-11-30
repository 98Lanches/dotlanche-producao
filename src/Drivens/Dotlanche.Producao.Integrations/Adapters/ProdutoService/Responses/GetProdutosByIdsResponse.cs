namespace Dotlanche.Producao.Integrations.Adapters.ProdutoService.Responses
{
    internal class GetProdutosByIdsResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public CategoriaObject? Categoria { get; set; }
    }

    internal class CategoriaObject
    {
        public int Id { get; set; }

        public string? Name { get; set; }
    }
}