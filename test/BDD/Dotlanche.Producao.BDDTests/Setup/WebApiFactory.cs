using Dotlanche.Producao.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using MongoDB.Driver;

namespace Dotlanche.Producao.BDDTests.Setup
{
    public class WebApiFactory : WebApplicationFactory<Program>
    {
        private MongoDbRunner? mongoDbRunner;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
            });
            builder.ConfigureAppConfiguration(cfgBuilder =>
            {
                cfgBuilder.AddEnvironmentVariables();

                var appsettingsFilePath = Path.Combine(Environment.CurrentDirectory, "appsettings.bdd.json");
                cfgBuilder.AddJsonFile(appsettingsFilePath);
            });

            builder.UseEnvironment("Development");
        }

        private IServiceCollection SetupMongoDbForTests(IServiceCollection services)
        {
            //var mongoClientServiceDescriptor = services.Single(d => d.ServiceType == typeof(IMongoClient));
            //services.Remove(mongoClientServiceDescriptor);

            //var mongoDatabaseServiceDescriptor = services.Single(d => d.ServiceType == typeof(IMongoDatabase));
            //services.Remove(mongoDatabaseServiceDescriptor);

            //mongoDbRunner = MongoDbRunner.Start();
            //services.AddSingleton(provider => new MongoClient(mongoDbRunner.ConnectionString));
            //services.AddSingleton(provider => provider.GetRequiredService<MongoClient>().GetDatabase("dotlanche-produto"));

            return services;
        }

        protected override void Dispose(bool disposing)
        {
            mongoDbRunner?.Dispose();
            base.Dispose(disposing);
        }
    }
}