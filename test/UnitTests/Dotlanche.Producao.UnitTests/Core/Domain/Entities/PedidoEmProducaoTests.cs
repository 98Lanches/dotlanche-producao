using AutoBogus;
using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.Domain.Exceptions;
using Dotlanche.Producao.Domain.ValueObjects;
using FluentAssertions;

namespace Dotlanche.Producao.UnitTests.Core.Domain.Entities
{
    public class PedidoEmProducaoTests
    {
        [Test]
        public void Constructor_WhenCalled_ShouldSetDefaultValueToProperties()
        {
            // Arrange
            var combosProdutos = new AutoFaker<ComboProdutos>().Generate(2);
            var pedidoConfirmado = new PedidoConfirmado(
                id: Guid.NewGuid(),
                combos: combosProdutos.Select(x => new ComboAceito(x.Id, x.Produtos.Select(x => x.Id)))
             );

            // Act
            var pedidoEmProducao = new PedidoEmProducao(pedidoConfirmado, combosProdutos);

            // Assert
            pedidoEmProducao.Status.Should().Be(StatusProducaoPedido.Recebido);
            pedidoEmProducao.CreationTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            pedidoEmProducao.LastUpdateTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Test]
        public void UpdateQueueKey_WhenNewQueueKeyIsSmallerThanCurrentQueueKey_ShouldThrowBusinessException()
        {
            // Arrange
            var combosProdutos = new AutoFaker<ComboProdutos>().Generate(2);
            var pedidoConfirmado = new PedidoConfirmado(
                id: Guid.NewGuid(),
                combos: combosProdutos.Select(x => new ComboAceito(x.Id, x.Produtos.Select(x => x.Id)))
             );

            var pedidoEmProducao = new PedidoEmProducao(pedidoConfirmado, combosProdutos);
            pedidoEmProducao.UpdateQueueKey(5);

            // Act
            var update = () => pedidoEmProducao.UpdateQueueKey(1);

            // Assert
            update
                .Should()
                .Throw<BusinessException>()
                .WithMessage("Cannot use old queue key for this pedido!");
        }

        [TestCase(StatusProducaoPedido.Recebido, StatusProducaoPedido.EmPreparo)]
        [TestCase(StatusProducaoPedido.EmPreparo, StatusProducaoPedido.Pronto)]
        [TestCase(StatusProducaoPedido.Pronto, StatusProducaoPedido.Finalizado)]
        public void AdvanceToNextStatus_WhenStatusHasNext_ShouldGoToExpectedStatus(
            StatusProducaoPedido currentStatus,
            StatusProducaoPedido expectedStatus)
        {
            // Arrange
            var combosProdutos = new AutoFaker<ComboProdutos>().Generate(2);
            var pedidoConfirmado = new PedidoConfirmado(
                id: Guid.NewGuid(),
                combos: combosProdutos.Select(x => new ComboAceito(x.Id, x.Produtos.Select(x => x.Id)))
             );

            var pedidoEmProducao = new PedidoEmProducao(pedidoConfirmado, combosProdutos);
            // Advance until current status
            for (int i = 0; i < (int)currentStatus; i++)
                pedidoEmProducao.AdvanceToNextStatus();

            // Act
            pedidoEmProducao.AdvanceToNextStatus();

            // Assert
            pedidoEmProducao.Status.Should().Be(expectedStatus);
        }

        [TestCase(StatusProducaoPedido.Cancelado)]
        [TestCase(StatusProducaoPedido.Finalizado)]
        public void AdvanceToNextStatus_WhenStatusHasNoNext_ShouldThrowException(StatusProducaoPedido status)
        {
            // Arrange
            var combosProdutos = new AutoFaker<ComboProdutos>().Generate(2);
            var pedidoConfirmado = new PedidoConfirmado(
                id: Guid.NewGuid(),
                combos: combosProdutos.Select(x => new ComboAceito(x.Id, x.Produtos.Select(x => x.Id)))
             );

            var pedidoEmProducao = new PedidoEmProducao(pedidoConfirmado, combosProdutos);
            switch (status)
            {
                case StatusProducaoPedido.Finalizado:
                    // Advance until finalizado status
                    for (int i = 0; i < (int)StatusProducaoPedido.Finalizado; i++)
                        pedidoEmProducao.AdvanceToNextStatus();
                    break;

                case StatusProducaoPedido.Cancelado:
                    pedidoEmProducao.Cancel();
                    break;
            }

            // Act
            var call = () => pedidoEmProducao.AdvanceToNextStatus();

            // Assert
            call.Should()
                .Throw<BusinessException>()
                .WithMessage($"There is no subsequent status for {status}");
        }

        [Test]
        public void Cancel_WhenStatusIsFinalizado_ShouldThrowException()
        {
            // Arrange
            var combosProdutos = new AutoFaker<ComboProdutos>().Generate(2);
            var pedidoConfirmado = new PedidoConfirmado(
                id: Guid.NewGuid(),
                combos: combosProdutos.Select(x => new ComboAceito(x.Id, x.Produtos.Select(x => x.Id)))
             );

            var pedidoEmProducao = new PedidoEmProducao(pedidoConfirmado, combosProdutos);
            for (int i = 0; i < (int)StatusProducaoPedido.Finalizado; i++)
                pedidoEmProducao.AdvanceToNextStatus();

            // Act
            var call = () => pedidoEmProducao.Cancel();

            // Assert
            call.Should()
                .Throw<BusinessException>()
                .WithMessage("Cannot cancel a pedido finalizado!");
        }
    }
}