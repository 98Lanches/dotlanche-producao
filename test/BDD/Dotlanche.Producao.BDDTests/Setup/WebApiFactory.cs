using Dotlanche.Producao.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dotlanche.Producao.BDDTests.Setup
{
    public class WebApiFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                SetupInMemoryDatabase(services);
            });
            builder.ConfigureAppConfiguration(cfgBuilder =>
            {
                cfgBuilder.AddEnvironmentVariables();

                var appsettingsFilePath = Path.Combine(Environment.CurrentDirectory, "appsettings.bdd.json");
                cfgBuilder.AddJsonFile(appsettingsFilePath);
            });

            builder.UseEnvironment("Testing");
        }

        private static IServiceCollection SetupInMemoryDatabase(IServiceCollection services)
        {
            throw new NotImplementedException();
        }
    }
}