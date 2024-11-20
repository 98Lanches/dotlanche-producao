using Dotlanche.Producao.Application.Exceptions;
using Dotlanche.Producao.Application.UseCases.Interfaces;
using Dotlanche.Producao.Data.Exceptions;
using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.Domain.Exceptions;
using Dotlanche.Producao.Domain.ValueObjects;
using Dotlanche.Producao.WebApi.DTOs;
using Dotlanche.Producao.WebApi.DTOs.Requests;
using Dotlanche.Producao.WebApi.DTOs.Responses;
using Dotlanche.Producao.WebApi.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace Dotlanche.Producao.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducaoController : ControllerBase
    {
        private readonly IIniciarProducaoPedidoUseCase iniciarProducaoPedidoUseCase;
        private readonly IAtualizarStatusPedidoUseCase atualizarStatusPedidoUseCase;

        public ProducaoController(
            IIniciarProducaoPedidoUseCase iniciarProducaoPedidoUseCase,
            IAtualizarStatusPedidoUseCase atualizarStatusPedidoUseCase)
        {
            this.iniciarProducaoPedidoUseCase = iniciarProducaoPedidoUseCase;
            this.atualizarStatusPedidoUseCase = atualizarStatusPedidoUseCase;
        }

        /// <summary>
        /// Inicia a produção de um pedido
        /// </summary>
        /// <param name="request">Dados do pedido aceito</param>
        /// <returns seecref="StartProducaoPedidoResponse">Dados do pedido em produção</returns>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     POST /producao
        ///     {
        ///         "PedidoId": "da753f8c-3871-49a1-a55f-c0404bc609bb",
        ///         "Combos": [
        ///             {
        ///                 "Id": "16d8ddb9-b0d7-4b5e-939f-5ca529a7aaff",
        ///                 "ProdutoIds": [
        ///                     "e8cb19fa-bc00-4f66-83fc-b60fe9b5c9f7",
        ///                     "d9785d54-a0ea-4b99-a8c5-acb627f94c36",
        ///                     "04e5850f-3cdd-40ad-9a63-2876fe270e6b"
        ///                 ]
        ///             }
        ///         ]
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [ProducesResponseType<StartProducaoPedidoResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StartProducaoPedidoResponse>> StartProducaoPedido(StartProducaoPedidoRequest request)
        {
            var requestIsValid = request.Validate(out var errors);
            if (!requestIsValid)
                return Problem(title: "Invalid request!",
                               statusCode: StatusCodes.Status400BadRequest,
                               detail: string.Join(",", errors));

            var pedidoConfirmado = new PedidoConfirmado(
                request.PedidoId,
                request.Combos.Select(x => new ComboAceito(x.Id, x.ProdutoIds))
            );

            try
            {
                var pedidoEmProducao = await iniciarProducaoPedidoUseCase.ExecuteAsync(pedidoConfirmado);
                return Created(string.Empty, pedidoEmProducao.ToResponse());
            }
            catch (UseCaseException ex)
            {
                return Problem(title: "Invalid request!",
                               statusCode: StatusCodes.Status400BadRequest,
                               detail: ex.Message);
            }
            catch (DatabaseException)
            {
                return Problem(title: "Server Error",
                               statusCode: StatusCodes.Status500InternalServerError,
                               detail: "It was not possible to start Producao Pedido. Please try again later.");
            }
        }

        /// <summary>
        /// Atualiza o status de um pedido
        /// </summary>
        /// <param name="request">Requisiçao com id do pedido e novo status</param>
        /// <returns seecref="PedidoEmProducao">Pedido Atualizado</returns>
        [HttpPut("status")]
        [ProducesResponseType<UpdateStatusPedidoResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StartProducaoPedidoResponse>> UpdateStatusPedido([FromBody] UpdateStatusPedidoRequest request)
        {
            var result = await atualizarStatusPedidoUseCase.ExecuteAsync(request.IdPedido, request.Status);
            if (!result.IsSuccess)
            {
                var error = result as ErrorResult;
                return NotFound(error!.Message);
            }

            var updatedPedido = ((ValueResult<PedidoEmProducao>)result).Value;
            var response = new UpdateStatusPedidoResponse()
            {
                Id = updatedPedido.Id,
                QueueKey = updatedPedido.QueueKey,
                Status = updatedPedido.Status,
                CreationTime = updatedPedido.CreationTime,
                LastUpdateTime = updatedPedido.LastUpdateTime,
            };
            return Ok(response);
        }
    }
}