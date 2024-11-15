﻿using AutoBogus;
using Dotlanche.Producao.Application.UseCases.Interfaces;
using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.Domain.ValueObjects;
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

            var controller = new ProducaoController(Mock.Of<IIniciarProducaoPedidoUseCase>());

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

            PedidoEmProducao pedidoEmProducao = new AutoFaker<PedidoEmProducao>()
                .CustomInstantiator(f => 
                    new PedidoEmProducao(
                        id: f.Random.Guid(), 
                        combosProdutos: new AutoFaker<ComboProdutos>().Generate(f.Random.Int(1,3)), 
                        creationTime: f.Date.Recent(), 
                        queueKey: f.Random.Int(1), 
                        status: f.PickRandom<StatusProducaoPedido>(), 
                        lastUpdateTime: f.Date.Recent()
                        )
                 )
                .Generate();

            useCaseMock
                .Setup(x => x.ExecuteAsync(It.IsAny<PedidoConfirmado>()))
                .ReturnsAsync(pedidoEmProducao);

            var controller = new ProducaoController(useCaseMock.Object);

            // Act
            var actionResult = await controller.StartProducaoPedido(request);

            // Assert
            var convertResult = actionResult as IConvertToActionResult;
            var actionResultWithStatusCode = (IStatusCodeActionResult)convertResult.Convert();

            actionResultWithStatusCode.StatusCode.Should().Be(StatusCodes.Status201Created);
        }
    }
}