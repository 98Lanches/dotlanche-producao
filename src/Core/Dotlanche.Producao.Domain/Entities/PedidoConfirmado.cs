using Dotlanche.Producao.Domain.ValueObjects;

namespace Dotlanche.Producao.Domain.Entities
{
    public class PedidoConfirmado
    {
        public PedidoConfirmado(Guid id, IEnumerable<ComboAceito> combos)
        {
            Id = id;
            Combos = combos;
        }

        public readonly Guid Id;

        public readonly IEnumerable<ComboAceito> Combos;
    }
}