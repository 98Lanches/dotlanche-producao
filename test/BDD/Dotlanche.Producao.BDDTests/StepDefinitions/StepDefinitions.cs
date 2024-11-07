using Dotlanche.Producao.BDDTests.Setup;
using Dotlanche.Producao.WebApi.DTOs;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Dotlanche.Producao.BDDTests.StepDefinitions
{
    [Binding]
    public sealed class StepDefinitions : IDisposable
    {
        private readonly HttpClient apiClient;
        private readonly IServiceScope scope;

        private readonly JsonSerializerOptions jsonOptions = new() { PropertyNameCaseInsensitive = true };

        private StartProducaoPedidoRequest request = new();
        private StartProducaoPedidoResponse? response;

        public StepDefinitions(WebApiFactory apiFactory)
        {
            apiClient = apiFactory.CreateClient();
            scope = apiFactory.Services.CreateScope();
        }

        [Given("alguma condição")]
        public void GivenACondition()
        {
            throw new MissingStepDefinitionException();
        }

        [When(@"algo acontecer")]
        public async Task WhenSomethingHappens()
        {
            throw new MissingStepDefinitionException();
        }

        [Then(@"deve fazer alguma coisa")]
        public void ThenShouldDoSomething()
        {
            throw new MissingStepDefinitionException();
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}