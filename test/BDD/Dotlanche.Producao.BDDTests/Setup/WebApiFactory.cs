using Dotlanche.Producao.WebApi;
using EphemeralMongo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Dotlanche.Producao.BDDTests.Setup
{
    public class WebApiFactory : WebApplicationFactory<Program>, IDisposable
    {
        private IMongoRunner? mongoRunner;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services = ConfigureInMemoryMongoDb(services);
            });
            builder.ConfigureAppConfiguration(cfgBuilder =>
            {
                cfgBuilder.AddEnvironmentVariables();

                var appsettingsFilePath = Path.Combine(Environment.CurrentDirectory, "appsettings.bdd.json");
                cfgBuilder.AddJsonFile(appsettingsFilePath);
            });

            builder.UseEnvironment("Development");
        }

        private IServiceCollection ConfigureInMemoryMongoDb(IServiceCollection services)
        {
            var mongoClientServiceDescriptor = services.Single(x => x.ServiceType == typeof(MongoClient));
            var mongoDatabaseServiceDescriptor = services.Single(x => x.ServiceType == typeof(IMongoDatabase));
            services.Remove(mongoClientServiceDescriptor);
            services.Remove(mongoDatabaseServiceDescriptor);

            mongoRunner = MongoRunner.Run();

            services.AddSingleton(provider => new MongoClient(mongoRunner.ConnectionString));
            services.AddSingleton(provider => provider.GetRequiredService<MongoClient>().GetDatabase("dotlanche-produto"));

            return services;
        }

        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}