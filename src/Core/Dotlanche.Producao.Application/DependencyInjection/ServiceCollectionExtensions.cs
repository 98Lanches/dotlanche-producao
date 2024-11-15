using Dotlanche.Producao.Application.UseCases;
using Dotlanche.Producao.Application.UseCases.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Dotlanche.Producao.Application.DependencyInjection
{
    [ExcludeFromCodeCoverage(Justification = "DI Class with no business logic")]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<IIniciarProducaoPedidoUseCase, IniciarProducaoPedidoUseCase>();
            services.AddScoped<IObterFilaPedidosUseCase, ObterFilaPedidosUseCase>();
            services.AddScoped<IAtualizarStatusPedidoUseCase, AtualizarStatusPedidoUseCase>();

            return services;
        }
    }
}
