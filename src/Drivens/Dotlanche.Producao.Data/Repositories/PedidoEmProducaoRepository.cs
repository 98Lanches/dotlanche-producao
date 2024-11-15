using Dotlanche.Producao.Data.Exceptions;
using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Dotlanche.Producao.Data.Repositories
{
    public class PedidoEmProducaoRepository : IPedidoEmProducaoRepository
    {
        public const string pedidosCollection = "PedidosEmProducao";

        private readonly IMongoCollection<PedidoEmProducao> _pedidosCollection;

        public PedidoEmProducaoRepository(IMongoDatabase database)
        {
            _pedidosCollection = database.GetCollection<PedidoEmProducao>(pedidosCollection);
        }

        public async Task<PedidoEmProducao> Add(PedidoEmProducao novoPedido)
        {
            var queueKeyForPedido = await GetNextQueueKey();
            novoPedido.UpdateQueueKey(queueKeyForPedido);

            try
            {
                await _pedidosCollection.InsertOneAsync(novoPedido);
            }
            catch (Exception e)
            {
                throw new DatabaseException("An error occurred while adding new PedidoEmProducao", e);
            }

            return novoPedido;
        }

        public async Task<PedidoEmProducao?> GetById(Guid idPedido)
        {
            var items = await _pedidosCollection.FindAsync(x => x.Id == idPedido);
            return items.SingleOrDefault();
        }

        public async Task<IEnumerable<PedidoEmProducao>> GetPedidosQueue()
        {
            var queueStatusIds = new[] { StatusProducaoPedido.Pronto, StatusProducaoPedido.EmPreparo, StatusProducaoPedido.Recebido };

            var filter = Builders<PedidoEmProducao>.Filter.In(p => p.Status, queueStatusIds);
            var sort = Builders<PedidoEmProducao>.Sort
                .Descending(p => p.Status)
                .Ascending(p => p.CreationTime);

            var pedidos = await _pedidosCollection
                .Find(filter)
                .Sort(sort)
                .ToListAsync();

            return pedidos;
        }

        public async Task<PedidoEmProducao> Update(PedidoEmProducao pedido)
        {
            var filter = Builders<PedidoEmProducao>.Filter.Eq(p => p.Id, pedido.Id);
            var result = await _pedidosCollection.ReplaceOneAsync(filter, pedido);

            if (result.MatchedCount == 0)
                throw new EntityNotFoundException();

            return pedido;
        }

        private async Task<int> GetNextQueueKey()
        {
            var lastKey = await _pedidosCollection
                .Find(_ => true)
                .SortByDescending(p => p.QueueKey)
                .Limit(1)
                .Project(x => x.QueueKey)
                .FirstOrDefaultAsync();
                
            return lastKey + 1;
        }
    }
}