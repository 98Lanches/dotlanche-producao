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

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Categoria { get; private set; }

        public decimal Price { get; private set; }
    }
}