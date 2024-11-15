using AutoBogus;
using Dotlanche.Producao.Application.Exceptions;
using Dotlanche.Producao.Application.UseCases.Interfaces;
using Dotlanche.Producao.Data.Exceptions;
using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.Domain.ValueObjects;
using Dotlanche.Producao.UnitTests.Builders;
using Dotlanche.Producao.WebApi.Controllers;
using Dotlanche.Producao.WebApi.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;

namespace Dotlanche.Producao.UnitTests.Drivers.WebApi.Controllers
{
    public class ProducaoControllerTests
    {
        [Test]
        public async Task StartProducaoPedido_WhenRequestValidationFails_ShouldReturn400()
        {
            // Arrange
            var request = new StartProducaoPedidoRequest()
            {
                PedidoId = Guid.NewGuid(),
                Combos = []
            };

            var controller = new ProducaoController(Mock.Of<IIniciarProducaoPedidoUseCase>(), Mock.Of<IAtualizarStatusPedidoUseCase>());

            // Act
            var actionResult = await controller.StartProducaoPedido(request);

            // Assert
            var convertResult = actionResult as IConvertToActionResult;
            var actionResultWithStatusCode = (IStatusCodeActionResult)convertResult.Convert();

            actionResultWithStatusCode.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Test]
        public async Task StartProducaoPedido_WhenRequestIsSuccessful_ShouldReturn201()
        {
            // Arrange
            var request = new AutoFaker<StartProducaoPedidoRequest>();

            var useCaseMock = new Mock<IIniciarProducaoPedidoUseCase>();

            var pedidoEmProducao = new PedidoEmProducaoBuilder().Generate();

            useCaseMock
                .Setup(x => x.ExecuteAsync(It.IsAny<PedidoConfirmado>()))
                .ReturnsAsync(pedidoEmProducao);

            var controller = new ProducaoController(useCaseMock.Object, Mock.Of<IAtualizarStatusPedidoUseCase>());

            // Act
            var actionResult = await controller.StartProducaoPedido(request);

            // Assert
            var convertResult = actionResult as IConvertToActionResult;
            var actionResultWithStatusCode = (IStatusCodeActionResult)convertResult.Convert();

            actionResultWithStatusCode.StatusCode.Should().Be(StatusCodes.Status201Created);
        }

        [Test]
        public async Task StartProducaoPedido_WhenUseCaseExceptionIsThrown_ShouldReturn400()
        {
            // Arrange
            var request = new AutoFaker<StartProducaoPedidoRequest>();

            var useCaseMock = new Mock<IIniciarProducaoPedidoUseCase>();

            var pedidoEmProducao = new PedidoEmProducaoBuilder().Generate();

            useCaseMock
                .Setup(x => x.ExecuteAsync(It.IsAny<PedidoConfirmado>()))
                .ThrowsAsync(new UseCaseException("error"));

            var controller = new ProducaoController(useCaseMock.Object, Mock.Of<IAtualizarStatusPedidoUseCase>());

            // Act
            var actionResult = await controller.StartProducaoPedido(request);

            // Assert
            var convertResult = actionResult as IConvertToActionResult;
            var actionResultWithStatusCode = (IStatusCodeActionResult)convertResult.Convert();

            actionResultWithStatusCode.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Test]
        public async Task StartProducaoPedido_WhenDatabaseExceptionIsThrown_ShouldReturn500()
        {
            // Arrange
            var request = new AutoFaker<StartProducaoPedidoRequest>();

            var useCaseMock = new Mock<IIniciarProducaoPedidoUseCase>();

            var pedidoEmProducao = new PedidoEmProducaoBuilder().Generate();

            useCaseMock
                .Setup(x => x.ExecuteAsync(It.IsAny<PedidoConfirmado>()))
                .ThrowsAsync(new DatabaseException("error"));

            var controller = new ProducaoController(useCaseMock.Object, Mock.Of<IAtualizarStatusPedidoUseCase>());

            // Act
            var actionResult = await controller.StartProducaoPedido(request);

            // Assert
            var convertResult = actionResult as IConvertToActionResult;
            var actionResultWithStatusCode = (IStatusCodeActionResult)convertResult.Convert();

            actionResultWithStatusCode.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}