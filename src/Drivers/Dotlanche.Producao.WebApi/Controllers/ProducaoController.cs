using Dotlanche.Producao.Application.UseCases;
using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.Domain.ValueObjects;
using Dotlanche.Producao.WebApi.DTOs;
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
        public async Task<IActionResult> StartProducaoPedido(StartProducaoPedidoRequest request)
        {
            if (!request.Combos.Any())
            {
                return BadRequest(new ValidationProblemDetails()
                {
                    Title = "Invalid Pedido!",
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Combos", ["A pedido must have combos with produtos!"] }
                    }
                });
            }

            var pedidoAceito = new PedidoConfirmado(
                request.PedidoId,
                request.Combos.Select(x => new ComboAceito(x.Id, x.ProdutoGuids))
            );

            var pedidoEmProducao = await iniciarProducaoPedidoUseCase.ExecuteAsync(pedidoAceito);

            var response = new StartProducaoPedidoResponse()
            {
                Success = true,
                QueueKey = pedidoEmProducao.QueueKey,
                Status = pedidoEmProducao.Status,
                Combos = pedidoEmProducao.Combos
                .Select(x => new ComboDTO()
                {
                    Id = x.Id,
                    ProdutoGuids = x.Produtos.Select(y => y.Id)
                }),
            };

            return Created(string.Empty, response);
        }
    }
}