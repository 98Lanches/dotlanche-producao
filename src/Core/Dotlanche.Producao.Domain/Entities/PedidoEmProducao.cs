using Dotlanche.Producao.Domain.Exceptions;
using Dotlanche.Producao.Domain.ValueObjects;

namespace Dotlanche.Producao.Domain.Entities
{
    public class PedidoEmProducao
    {
        internal PedidoEmProducao(Guid idPedidoConfirmado, IEnumerable<ComboProdutos> combosProdutos)
        {
            Id = idPedidoConfirmado;
            Combos = combosProdutos;

            ValidateEntity();

            Status = StatusProducaoPedido.Recebido;

            CreationTime = DateTime.Now;
            LastUpdateTime = DateTime.Now;
        }

        internal PedidoEmProducao(Guid id, IEnumerable<ComboProdutos> combosProdutos, DateTime creationTime, int queueKey, StatusProducaoPedido status, DateTime lastUpdateTime)
        {
            Id = id;
            Combos = combosProdutos;
            CreationTime = creationTime;
            QueueKey = queueKey;
            Status = status;
            LastUpdateTime = lastUpdateTime;

            ValidateEntity();
        }

        public Guid Id { get; private set; }

        public IEnumerable<ComboProdutos> Combos { get; private set; }

        public DateTime CreationTime { get; private set; }

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
                StatusProducaoPedido.Finalizado
                or StatusProducaoPedido.Cancelado => throw new BusinessException($"There is no subsequent status for {currentStatus}"),
                _ => throw new DomainValidationException(nameof(Status))
            };

            LastUpdateTime = DateTime.Now;
        }

        public void UpdateStatus(StatusProducaoPedido newStatus)
        {
            Status = newStatus;
            LastUpdateTime = DateTime.Now;
        }

        public void UpdateQueueKey(int newQueueKey)
        {
            if (newQueueKey < QueueKey)
                throw new BusinessException("Cannot use old queue key for this pedido!");

            QueueKey = newQueueKey;
        }

        public void Cancel()
        {
            if (Status == StatusProducaoPedido.Finalizado)
                throw new BusinessException("Cannot cancel a pedido finalizado!");

            Status = StatusProducaoPedido.Cancelado;
        }

        public static PedidoEmProducao StartNew(PedidoConfirmado pedidoConfirmado, IEnumerable<ComboProdutos> comboProdutos)
        {
            return new(pedidoConfirmado.Id, comboProdutos);
        }

        private void ValidateEntity()
        {
            if (!Combos.Any())
                throw new DomainValidationException(nameof(Combos));

            if (Id == Guid.Empty)
                throw new DomainValidationException(nameof(Id));
        }
    }
}