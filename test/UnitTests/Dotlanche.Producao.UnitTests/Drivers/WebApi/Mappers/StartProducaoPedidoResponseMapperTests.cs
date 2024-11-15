using AutoBogus;
using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.Domain.ValueObjects;
using Dotlanche.Producao.WebApi.Mappers;
using FluentAssertions;

namespace Dotlanche.Producao.UnitTests.Drivers.WebApi.Mappers
{
    public class StartProducaoPedidoResponseMapperTests
    {
        [Test]
        public void ToResponse_WhenReceivingAProdutoEmProducao_ShouldMapCorrectly()
        {
            // Arrange
            var produtos = new AutoFaker<Produto>().Generate(3);
            var comboId = Guid.NewGuid();
            var pedidoConfirmado = new PedidoConfirmado(Guid.NewGuid(), [new ComboAceito(comboId, produtos.Select(x => x.Id))]);
            var pedidoEmProducao = new PedidoEmProducao(pedidoConfirmado.Id, [new ComboProdutos(comboId, produtos)]);
            pedidoEmProducao.UpdateQueueKey(4);

            // Act
            var response = pedidoEmProducao.ToResponse();

            // Assert
            response.Success.Should().BeTrue();
            response.QueueKey.Should().Be(pedidoEmProducao.QueueKey);
            response.Status.Should().Be(StatusProducaoPedido.Recebido);
            response.Combos.Should().BeEquivalentTo(pedidoConfirmado.Combos);
        }
    }
}