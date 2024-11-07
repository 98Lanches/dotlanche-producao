using Dotlanche.Producao.Domain.ValueObjects;

namespace Dotlanche.Producao.Domain.Entities
{
    public class PedidoAceito
    {
        public PedidoAceito(Guid id, IEnumerable<ComboAceito> combos)
        {
            Id = id;
            Combos = combos;
        }

        public readonly Guid Id;

        public readonly IEnumerable<ComboAceito> Combos;
    }
}