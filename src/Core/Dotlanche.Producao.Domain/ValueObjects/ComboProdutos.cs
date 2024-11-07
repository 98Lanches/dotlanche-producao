using Dotlanche.Producao.Domain.Entities;

namespace Dotlanche.Producao.Domain.ValueObjects
{
    public record ComboProdutos
    {
        public ComboProdutos(IEnumerable<Produto> produtos)
        {
            Produtos = produtos.ToList();
        }

        public readonly List<Produto> Produtos = [];
    }
}