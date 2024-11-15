using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.WebApi.DTOs;
using Dotlanche.Producao.WebApi.DTOs.Responses;

namespace Dotlanche.Producao.WebApi.Mappers
{
    public static class StartProducaoPedidoResponseMapper
    {
        public static StartProducaoPedidoResponse ToResponse(this PedidoEmProducao pedidoEmProducao)
        {
            var response = new StartProducaoPedidoResponse()
            {
                Success = true,
                PedidoId = pedidoEmProducao.Id,
                QueueKey = pedidoEmProducao.QueueKey,
                Status = pedidoEmProducao.Status,
                Combos = pedidoEmProducao.Combos
                .Select(x => new ComboDto()
                {
                    Id = x.Id,
                    ProdutoIds = x.Produtos.Select(y => y.Id)
                }),
            };

            return response;
        }
    }
}
