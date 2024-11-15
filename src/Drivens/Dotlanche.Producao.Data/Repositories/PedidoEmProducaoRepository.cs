﻿using Dotlanche.Producao.Data.DTOs;
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

        private async Task<int> GetNextQueueKey()
        {
            var lastKey = await _keysCollection.AsQueryable().FirstOrDefaultAsync()
                ?? QueueControl.GetInitialQueueKey();

            return lastKey.QueueKey + 1;
        }
    }
}