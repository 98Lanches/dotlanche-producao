using Dotlanche.Producao.Application.Ports;
using Dotlanche.Producao.Checkout.Adapters;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Dotlanche.Producao.Checkout.DependencyInjection
{
    [ExcludeFromCodeCoverage(Justification = "DI Class with no business logic")]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFakeCheckoutProvider(this IServiceCollection services)
        {
            services.AddScoped<IQrCodeProvider, FakeCheckoutQrCodeProvider>();

            return services;
        }
    }
}
