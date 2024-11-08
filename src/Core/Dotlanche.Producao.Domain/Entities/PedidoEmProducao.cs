using Dotlanche.Producao.Domain.Exceptions;
using Dotlanche.Producao.Domain.ValueObjects;

namespace Dotlanche.Producao.Domain.Entities
{
    public class PedidoEmProducao
    {
        public PedidoEmProducao(PedidoConfirmado pedidoConfirmado,
                                IEnumerable<ComboProdutos> produtos)
        {
            Id = pedidoConfirmado.Id;
            Combos = produtos;
            Status = StatusProducaoPedido.Recebido;

            CreationTime = DateTime.Now;
            LastUpdateTime = DateTime.Now;
        }

        public readonly Guid Id;

        public readonly IEnumerable<ComboProdutos> Combos;

        public readonly DateTime CreationTime;

        public int QueueKey { get; private set; }

        public StatusProducaoPedido Status { get; private set; }

        public DateTime LastUpdateTime { get; private set; }

        public void AdvanceToNextStatus()
        {
            var currentStatus = Status;

            Status = currentStatus switch
            {
                StatusProducaoPedido.Recebido => StatusProducaoPedido.EmPreparo,
                StatusProducaoPedido.EmPreparo => StatusProducaoPedido.Pronto,
                StatusProducaoPedido.Pronto => StatusProducaoPedido.Finalizado,
                StatusProducaoPedido.Pronto
                or StatusProducaoPedido.Cancelado => throw new BusinessException($"There is no subsequent status for {currentStatus}"),
                _ => throw new DomainValidationException(nameof(Status))
            };

            LastUpdateTime = DateTime.Now;
        }

        public void UpdateQueueKey(int newQueueKey)
        {
            if(newQueueKey < QueueKey)
                throw new BusinessException("Cannot Use old queue key for this pedido!");

            QueueKey = newQueueKey;
        }
    }
}