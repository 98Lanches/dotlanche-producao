using Dotlanche.Producao.Data.DTOs;
using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Dotlanche.Producao.Data.Repositories
{
    public class PedidoEmProducaoRepository : IPedidoEmProducaoRepository
    {
        public const string pedidosCollection = "PedidosEmProducao";
        public const string keysCollection = "QueueKeyControl";

        private readonly IMongoCollection<PedidoEmProducao> _pedidosCollection;
        private readonly IMongoCollection<QueueControl> _keysCollection;

        public PedidoEmProducaoRepository(IMongoDatabase database)
        {
            _pedidosCollection = database.GetCollection<PedidoEmProducao>(pedidosCollection);
            _keysCollection = database.GetCollection<QueueControl>(keysCollection);
        }

        public async Task<PedidoEmProducao> Add(PedidoEmProducao novoPedido)
        {
            var queueKeyForPedido = await GetNextQueueKey();
            novoPedido.UpdateQueueKey(queueKeyForPedido);

            await _keysCollection.InsertOneAsync(new QueueControl(novoPedido.QueueKey));
            await _pedidosCollection.InsertOneAsync(novoPedido);

            return novoPedido;
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

        private async Task<int> GetNextQueueKey()
        {
            var lastKey = await _keysCollection.AsQueryable().FirstOrDefaultAsync()
                ?? QueueControl.GetInitialQueueKey();

            return lastKey.QueueKey + 1;
        }
    }
}