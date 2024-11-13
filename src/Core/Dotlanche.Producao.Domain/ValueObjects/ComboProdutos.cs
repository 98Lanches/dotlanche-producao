using Dotlanche.Producao.Domain.Entities;

namespace Dotlanche.Producao.Domain.ValueObjects
{
    public record ComboProdutos
    {
        public ComboProdutos(Guid Id, IEnumerable<Produto> produtos)
        {
            this.Id = Id;
            Produtos = produtos.ToList();
        }

        public readonly IReadOnlyList<Produto> Produtos = [];

        public Guid Id { get; }
    }
}