using Dotlanche.Producao.Application.Exceptions;
using Dotlanche.Producao.Domain.Ports;
using Dotlanche.Producao.Integrations.Adapters.ProdutoService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dotlanche.Producao.Integrations.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceIntegrations(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IProdutoServiceClient, ProdutoServiceClient>();
            services.AddHttpClient<IProdutoServiceClient, ProdutoServiceClient>(client =>
            {
                client.BaseAddress = new Uri(config["Integrations:ProdutoService:BaseAddress"] ?? 
                    throw new MisconfigurationException("Integrations:ProdutoService:BaseAddress"));
            });

            return services;
        }
    }
}
