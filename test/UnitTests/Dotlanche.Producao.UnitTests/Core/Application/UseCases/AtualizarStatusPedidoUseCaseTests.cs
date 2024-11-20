using Dotlanche.Producao.Application.UseCases;
using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.Domain.Repositories;
using Dotlanche.Producao.Domain.ValueObjects;
using Dotlanche.Producao.UnitTests.Builders;
using FluentAssertions;
using Moq;

namespace Dotlanche.Producao.UnitTests.Core.Application.UseCases
{
    public class AtualizarStatusPedidoUseCaseTests
    {
        [Test]
        public async Task ExecuteAsync_WhenPedidoExists_ShouldCallUpdateWithNewStatus()
        {
            // Arrange
            var idPedido = Guid.NewGuid();
            var newStatus = StatusProducaoPedido.Pronto;

            var repositoryMock = new Mock<IPedidoEmProducaoRepository>();
            var existingPedido =
                new PedidoEmProducaoBuilder()
                .WithPedidoId(idPedido)
                .WithStatus(StatusProducaoPedido.EmPreparo)
                .Generate();
            repositoryMock.Setup(x => x.GetById(idPedido)).ReturnsAsync(existingPedido);

            var useCase = new AtualizarStatusPedidoUseCase(repositoryMock.Object);

            // Act
            await useCase.ExecuteAsync(idPedido, newStatus);

            // Assert
            repositoryMock
                .Verify(repo =>
                    repo.Update(It.Is<PedidoEmProducao>(pedido =>
                        pedido.Id == idPedido
                        && pedido.Status == newStatus
                )), Times.Once());
        }

        [Test]
        public async Task ExecuteAsync_WhenPedidoExists_ShouldReturnValueResultWithUpdatedPedido()
        {
            // Arrange
            var idPedido = Guid.NewGuid();
            var newStatus = StatusProducaoPedido.EmPreparo;

            var repositoryMock = new Mock<IPedidoEmProducaoRepository>();
            var existingPedido =
                new PedidoEmProducaoBuilder()
                .WithPedidoId(idPedido)
                .WithStatus(StatusProducaoPedido.Recebido)
                .Generate();
            repositoryMock.Setup(x => x.GetById(idPedido)).ReturnsAsync(existingPedido);

            var updated = new PedidoEmProducao(existingPedido.Id,
                                               existingPedido.Combos,
                                               existingPedido.CreationTime,
                                               existingPedido.QueueKey,
                                               newStatus,
                                               existingPedido.LastUpdateTime.AddSeconds(2));
            repositoryMock.Setup(x => x.Update(existingPedido)).ReturnsAsync(updated);

            var useCase = new AtualizarStatusPedidoUseCase(repositoryMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(idPedido, newStatus);

            // Assert
            result.Should().BeOfType<ValueResult<PedidoEmProducao>>();
            var valueResult = (ValueResult<PedidoEmProducao>)result;

            valueResult.IsSuccess.Should().BeTrue();
            valueResult.Value.Should().Be(updated);
        }

        [Test]
        public async Task ExecuteAsync_WhenPedidoDoesNotExist_ShouldReturnErrorResult()
        {
            // Arrange
            var idPedido = Guid.NewGuid();
            var newStatus = StatusProducaoPedido.Pronto;

            var repositoryMock = new Mock<IPedidoEmProducaoRepository>();
            repositoryMock.Setup(x => x.GetById(idPedido)).ReturnsAsync((PedidoEmProducao?)null);

            var useCase = new AtualizarStatusPedidoUseCase(repositoryMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(idPedido, newStatus);

            // Assert
            result
                .Should()
                .BeOfType<ErrorResult>()
                .Which
                .Message.Should().Be("Pedido does not exist");
        }
    }
}