using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.Domain.Repositories;
using MongoDB.Driver;

namespace Dotlanche.Producao.Data.Repositories
{
    internal class EmptyRepository : IRepository
    {
        private readonly IMongoCollection<Entity> _entityCollection;

        public EmptyRepository(IMongoDatabase database)
        {
            _entityCollection = database.GetCollection<Entity>("TBD");
        }

        public async Task Add(Entity pagamento)
        {
            await _entityCollection.InsertOneAsync(pagamento);
        }

        public async Task<Entity> Update(Entity e)
        {
            var filter = Builders<Entity>.Filter.Eq(p => p.Id, e.Id);
            var result = await _entityCollection.ReplaceOneAsync(filter, e);

            if (result.MatchedCount == 0)
                throw new Exception("Not found");

            return e;
        }

        public async Task<Entity?> GetById(Guid id)
        {
            var e = await _entityCollection
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();

            return e;
        }
    }
}