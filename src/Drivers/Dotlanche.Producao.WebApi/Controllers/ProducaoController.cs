using Dotlanche.Producao.Application.UseCases;
using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.Domain.ValueObjects;
using Dotlanche.Producao.WebApi.DTOs;
using Dotlanche.Producao.WebApi.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace Dotlanche.Producao.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducaoController : ControllerBase
    {
        private readonly IIniciarProducaoPedidoUseCase iniciarProducaoPedidoUseCase;

        public ProducaoController(IIniciarProducaoPedidoUseCase iniciarProducaoPedidoUseCase)
        {
            this.iniciarProducaoPedidoUseCase = iniciarProducaoPedidoUseCase;
        }

        /// <summary>
        /// Inicia a produção de um pedido
        /// </summary>
        /// <param name="pedido">Dados do pedido aceito</param>
        /// <returns see cref="StartProducaoPedidoResponse">Dados do pedido em produção</returns>
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

            var pedidoEmProducao = await iniciarProducaoPedidoUseCase.ExecuteAsync(pedidoConfirmado);

            return Created(string.Empty, pedidoEmProducao.ToResponse());
        }
    }
}