﻿using Dotlanche.Producao.Data.Repositories;
using Dotlanche.Producao.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Dotlanche.Producao.Data.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(provider => new MongoClient(configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton(provider => provider.GetRequiredService<MongoClient>().GetDatabase(configuration.GetSection("MongoDb:DatabaseName").Value));

            RegisterConventions();

            services.AddScoped<IPedidoEmProducaoRepository, PedidoEmProducaoRepository>();

            return services;
        }

        private static void RegisterConventions()
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeSerializer(DateTimeKind.Local));
        }
    }
}