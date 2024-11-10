using AutoBogus;
using Dotlanche.Producao.Application.Exceptions;
using Dotlanche.Producao.Application.UseCases;
using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.Domain.Ports;
using Dotlanche.Producao.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace Dotlanche.Producao.UnitTests.Core.Application.UseCases
{
    public class IniciarProducaoPedidoUseCaseTests
    {
        [Test]
        public async Task ExecuteAsync_WhenPedidoConfirmadoHasValidProducts_ShouldReturnAPedidoEmProducao()
        {
            // Arrange
            var pedidoConfirmado = new AutoFaker<PedidoConfirmado>().Generate();

            var repositoryMock = new Mock<IPedidoEmProducaoRepository>();
            var generatedQueueKey = 35;
            repositoryMock
                .Setup(x => x.Add(It.IsAny<PedidoEmProducao>()))
                .Callback<PedidoEmProducao>(pedidoEmProducao =>
                {
                    pedidoEmProducao.UpdateQueueKey(generatedQueueKey);
                })
                .Returns<PedidoEmProducao>(x => Task.FromResult(x));

            var produtoIds = pedidoConfirmado.Combos.SelectMany(x => x.ProdutoIds).Distinct().ToList();
            var produtosBuilder = new AutoFaker<Produto>();
            var produtos = produtoIds.Select(produtoId =>
            {
                return produtosBuilder
                .RuleFor(x => x.Id, produtoId)
                .Generate();
            });
            var serviceClientMock = new Mock<IProdutoServiceClient>();
            serviceClientMock
                .Setup(x => x.GetByIds(produtoIds, It.IsAny<CancellationToken>()))
                .ReturnsAsync(produtos);

            var useCase = new IniciarProducaoPedidoUseCase(repositoryMock.Object, serviceClientMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(pedidoConfirmado);

            // Assert
            result.Status.Should().Be(StatusProducaoPedido.Recebido);
            result.QueueKey.Should().Be(generatedQueueKey);
            result.CreationTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            result.LastUpdateTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));

            result.Combos.Should().AllSatisfy(combo =>
            {
                var correspondingCombo = pedidoConfirmado.Combos.Single(x => x.Id == combo.Id);
                combo.Produtos.Select(x => x.Id).Should().BeEquivalentTo(correspondingCombo.ProdutoIds);
            });
        }

        [Test]
        public async Task ExecuteAsync_WhenPedidoConfirmadoHasValidProducts_ShouldCallProdutoClientWithDistinctProductIds()
        {
            // Arrange
            var pedidoConfirmado = new AutoFaker<PedidoConfirmado>().Generate();

            var repositoryMock = new Mock<IPedidoEmProducaoRepository>();

            var produtoIds = pedidoConfirmado.Combos.SelectMany(x => x.ProdutoIds).Distinct().ToList();
            var produtosBuilder = new AutoFaker<Produto>();
            var produtos = produtoIds.Select(produtoId =>
            {
                return produtosBuilder
                .RuleFor(x => x.Id, produtoId)
                .Generate();
            });
            var serviceClientMock = new Mock<IProdutoServiceClient>();
            serviceClientMock
                .Setup(x => x.GetByIds(produtoIds, It.IsAny<CancellationToken>()))
                .ReturnsAsync(produtos);

            var useCase = new IniciarProducaoPedidoUseCase(repositoryMock.Object, serviceClientMock.Object);

            // Act
            await useCase.ExecuteAsync(pedidoConfirmado);

            // Assert
            serviceClientMock.Verify(x => x.GetByIds(produtoIds, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ExecuteAsync_WhenPedidoConfirmadoHasAProductThatDoesNotExist_ShouldThrowUseCaseException()
        {
            // Arrange
            var pedidoConfirmado = new AutoFaker<PedidoConfirmado>().Generate();

            var repositoryMock = new Mock<IPedidoEmProducaoRepository>();

            var produtoIds = pedidoConfirmado.Combos.SelectMany(x => x.ProdutoIds).Distinct().ToList();
            var produtosBuilder = new AutoFaker<Produto>();

            var produtos = new List<Produto>();
            var inexistentProduto = produtoIds.First();
            foreach (var produtoId in produtoIds)
            {
                if (produtoId == inexistentProduto)
                    continue;

                produtos.Add(produtosBuilder.RuleFor(x => x.Id, produtoId).Generate());
            }
            var serviceClientMock = new Mock<IProdutoServiceClient>();
            serviceClientMock
                .Setup(x => x.GetByIds(produtoIds, It.IsAny<CancellationToken>()))
                .ReturnsAsync(produtos);

            var useCase = new IniciarProducaoPedidoUseCase(repositoryMock.Object, serviceClientMock.Object);

            // Act
            var call = () => useCase.ExecuteAsync(pedidoConfirmado);

            // Assert
            await call.Should()
                .ThrowAsync<UseCaseException>()
                .WithMessage($"Produto {inexistentProduto} not found!");
        }
    }
}