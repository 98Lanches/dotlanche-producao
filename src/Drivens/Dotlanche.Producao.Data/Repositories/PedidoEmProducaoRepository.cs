using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.Domain.Repositories;
using MongoDB.Driver;

namespace Dotlanche.Producao.Data.Repositories
{
    internal class PedidoEmProducaoRepository : IPedidoEmProducaoRepository
    {
        private readonly IMongoCollection<PedidoEmProducao> _entityCollection;

        public PedidoEmProducaoRepository(IMongoDatabase database)
        {
            _entityCollection = database.GetCollection<PedidoEmProducao>("PedidosEmProducao");
        }

        public async Task<PedidoEmProducao> Add(PedidoEmProducao novoPedido)
        {
            await _entityCollection.InsertOneAsync(novoPedido);
            return novoPedido;
        }

        public Task<string> GetNextKey()
        {
            throw new NotImplementedException();
        }

        public async Task<PedidoEmProducao> Update(PedidoEmProducao e)
        {
            var filter = Builders<PedidoEmProducao>.Filter.Eq(p => p.Id, e.Id);
            var result = await _entityCollection.ReplaceOneAsync(filter, e);

            if (result.MatchedCount == 0)
                throw new Exception("Not found");

            return e;
        }

        public async Task<PedidoEmProducao?> GetById(Guid id)
        {
            var e = await _entityCollection
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();

            return e;
        }
    }
}