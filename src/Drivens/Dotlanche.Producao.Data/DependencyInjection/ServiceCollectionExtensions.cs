using Dotlanche.Producao.Data.Repositories;
using Dotlanche.Producao.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Dotlanche.Producao.Data.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(provider => new MongoClient(configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton(provider => provider.GetRequiredService<MongoClient>().GetDatabase("dotlanche-produto"));

            RegisterConventions();

            services.AddScoped<IRepository, EmptyRepository>();

            return services;
        }

        private static void RegisterConventions()
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

            var pack = new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String),
            };
            ConventionRegistry.Register("DotlancheConventions", pack, t => true);
        }
    }
}