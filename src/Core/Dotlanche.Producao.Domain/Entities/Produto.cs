namespace Dotlanche.Producao.Domain.Entities
{
    public class Produto
    {
        public Produto(Guid id, string name, string categoria)
        {
            Id = id;
            Name = name;
            Categoria = categoria;
        }

        public readonly Guid Id;

        public readonly string Name;

        public readonly string Categoria;
    }
}