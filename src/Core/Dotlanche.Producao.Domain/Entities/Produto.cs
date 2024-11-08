namespace Dotlanche.Producao.Domain.Entities
{
    public class Produto
    {
        public Produto(Guid id, string name, string categoria, decimal price)
        {
            Id = id;
            Name = name;
            Categoria = categoria;
            Price = price;
        }

        public readonly Guid Id;

        public readonly string Name;

        public readonly string Categoria;

        public readonly decimal Price;
    }
}