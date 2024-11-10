using AutoBogus;
using Dotlanche.Producao.WebApi.DTOs;
using FluentAssertions;

namespace Dotlanche.Producao.UnitTests.Drivers.WebApi.DTOs
{
    public class StartProducaoPedidoRequestTests
    {
        [Test]
        public void Validate_WhenComboListIsEmpty_ShouldReturnFalseWithErrorList()
        {
            // Arrange
            var request = new StartProducaoPedidoRequest()
            {
                PedidoId = Guid.NewGuid(),
                Combos = [],
            };

            // Act
            var isValid = request.Validate(out var errors);

            // Assert
            isValid.Should().BeFalse();
            errors.Should().Contain($"Combos: Pedido must have a combo to start producao!");
        }

        [Test]
        public void Validate_WhenRequestIsValid_ShouldReturnTrueWithEmptyErrorList()
        {
            // Arrange
            var request = new StartProducaoPedidoRequest()
            {
                PedidoId = Guid.NewGuid(),
                Combos = new AutoFaker<ComboDto>().Generate(3),
            };

            // Act
            var isValid = request.Validate(out var errors);

            // Assert
            isValid.Should().BeTrue();
            errors.Should().BeEmpty();
        }
    }
}