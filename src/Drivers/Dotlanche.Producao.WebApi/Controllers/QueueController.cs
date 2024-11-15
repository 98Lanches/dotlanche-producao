using Dotlanche.Producao.Application.UseCases.Interfaces;
using Dotlanche.Producao.WebApi.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dotlanche.Producao.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private readonly IObterFilaPedidosUseCase obterFilaPedidosUseCase;

        public QueueController(IObterFilaPedidosUseCase obterFilaPedidosUseCase)
        {
            this.obterFilaPedidosUseCase = obterFilaPedidosUseCase;
        }

        /// <summary>
        /// Obtém a fila de pedidos em produção
        /// </summary>
        /// <returns seecref="StartProducaoPedidoResponse">Fila de pedidos em produção ordenada</returns>
        [HttpGet]
        [ProducesResponseType<GetPedidosQueueResponse>(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetPedidosQueueResponse>> GetPedidosEmProducaoQueue()
        {
            var queue = await obterFilaPedidosUseCase.ExecuteAsync();

            var response = new GetPedidosQueueResponse()
            {
                Items = queue.Select(x => new PedidoQueueItem()
                {
                    QueueKey = x.QueueKey,
                    Status = x.Status
                })
            };
            return Ok(response);
        }
    }
}