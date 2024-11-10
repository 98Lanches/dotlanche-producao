using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.WebApi.DTOs;

namespace Dotlanche.Producao.WebApi.Mappers
{
    public static class StartProducaoPedidoResponseMapper
    {
        public static StartProducaoPedidoResponse ToResponse(this PedidoEmProducao pedidoEmProducao)
        {
            var response = new StartProducaoPedidoResponse()
            {
                Success = true,
                QueueKey = pedidoEmProducao.QueueKey,
                Status = pedidoEmProducao.Status,
                Combos = pedidoEmProducao.Combos
                .Select(x => new ComboDTO()
                {
                    Id = x.Id,
                    ProdutoIds = x.Produtos.Select(y => y.Id)
                }),
            };

            return response;
        }
    }
}
