using Dotlanche.Producao.Data.DTOs;
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
        public const string keysCollection = "QueueKeyControl";

        private readonly IMongoCollection<PedidoEmProducao> _pedidosCollection;
        private readonly IMongoCollection<QueueControl> _keysCollection;
        private readonly IMongoDatabase database;

        public PedidoEmProducaoRepository(IMongoDatabase database)
        {
            _pedidosCollection = database.GetCollection<PedidoEmProducao>(pedidosCollection);
            _keysCollection = database.GetCollection<QueueControl>(keysCollection);
            this.database = database;
        }

        public async Task<PedidoEmProducao> Add(PedidoEmProducao novoPedido)
        {
            var queueKeyForPedido = await GetNextQueueKey();
            novoPedido.UpdateQueueKey(queueKeyForPedido);

            await _keysCollection.InsertOneAsync(new QueueControl(novoPedido.QueueKey));
            await _pedidosCollection.InsertOneAsync(novoPedido);

            return novoPedido;
        }

        public async Task<PedidoEmProducao> Update(PedidoEmProducao e)
        {
            var filter = Builders<PedidoEmProducao>.Filter.Eq(p => p.Id, e.Id);
            var result = await _pedidosCollection.ReplaceOneAsync(filter, e);

            if (result.MatchedCount == 0)
                throw new DatabaseException($"{e.Id} was not found!");

            return e;
        }

        public async Task<PedidoEmProducao?> GetById(Guid id)
        {
            var e = await _pedidosCollection
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();

            return e;
        }

        private async Task<int> GetNextQueueKey()
        {
            var lastKey = await _keysCollection.AsQueryable().FirstOrDefaultAsync()
                ?? QueueControl.GetInitialQueueKey();

            return lastKey.QueueKey + 1;
        }
    }
}