namespace Dotlanche.Producao.Domain.ValueObjects
{
    public record ComboAceito
    {
        public ComboAceito(Guid id, IEnumerable<Guid> produtoIds)
        {
            Id = id;
            ProdutoIds = produtoIds;
        }

        public readonly IEnumerable<Guid> ProdutoIds;

        public readonly Guid Id;
    }
}