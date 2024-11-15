using Dotlanche.Producao.BDDTests.DataTableObjects;
using Dotlanche.Producao.BDDTests.Drivers;
using Dotlanche.Producao.BDDTests.Setup;
using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.WebApi.DTOs;
using Dotlanche.Producao.WebApi.DTOs.Responses;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dotlanche.Producao.BDDTests.StepDefinitions
{
    [Binding]
    [Scope(Feature = "Iniciar Producao Pedido")]
    public sealed class IniciarProducaoPedidoStepDefinitions : IDisposable
    {
        private readonly HttpClient apiClient;
        private readonly IServiceScope scope;

        private readonly JsonSerializerOptions jsonOptions;
        private StartProducaoPedidoRequest request;
        private StartProducaoPedidoResponse? response;

        public IniciarProducaoPedidoStepDefinitions(WebApiFactory apiFactory)
        {
            apiClient = apiFactory.CreateClient();
            scope = apiFactory.Services.CreateScope();

            jsonOptions = new() { PropertyNameCaseInsensitive = true };
            jsonOptions.Converters.Add(new JsonStringEnumConverter());

            request = new StartProducaoPedidoRequest()
            {
                PedidoId = Guid.NewGuid(),
                Combos = []
            };
        }

        [Given("um pedido confirmado com id (.*)")]
        public void GivenAPedidoConfirmadoWithId(Guid idPedidoConfirmado)
        {
            request.PedidoId = idPedidoConfirmado;
        }

        [Given("os seguintes produtos existam no servico de produto:")]
        public void GivenTheFollowingProductsExistInProductService(DataTable produtosTable)
        {
            var existingProdutos = produtosTable.CreateSet<Produto>();
            ProdutoServiceDriver.SetupGetProdutosByIdMock(request.Combos.SelectMany(x => x.ProdutoIds).Distinct(), existingProdutos);
        }

        [Given("o pedido possui os seguintes combos:")]
        public void GivenPedidoHasComboWithThisIdAndThisProducts(DataTable combosTable)
        {
            var combos = combosTable
                            .CreateSet<ComboDataTableObject>()
                            .GroupBy(x => x.ComboId)
                            .Select(x => new ComboDto
                            {
                                Id = x.Key,
                                ProdutoIds = x.Select(y => y.ProdutoId)
                            });

            request.Combos = combos;
        }

        [When(@"eu solicitar o inicio da producao do pedido")]
        public async Task WhenIRequestStartProducaoPedido()
        {
            const string startProducaoPedidoRoute = "/api/producao";
            var httpResponse = await apiClient.PostAsJsonAsync(startProducaoPedidoRoute, request);

            if (httpResponse.IsSuccessStatusCode)
                response = await httpResponse.Content.ReadFromJsonAsync<StartProducaoPedidoResponse>(jsonOptions);
        }

        [Then(@"a producao do pedido deve ser iniciada com os produtos do pedido confirmado")]
        public void ThenPedidoEmProducaoMustBeCreatedWithThePedidoConfirmadoProducts()
        {
            response.Should().NotBeNull(because: "resposta da criação do pedido deve ser de sucesso");
            response!.Success.Should().BeTrue();
            response.Combos.Should().BeEquivalentTo(request.Combos);
        }

        [Then(@"deve gerar uma senha")]
        public void ThenItShouldGenerateAKey()
        {
            response.Should().NotBeNull(because: "resposta da criação do pedido deve ser de sucesso");
            response!.QueueKey.Should().NotBe(0);
        }

        [Then(@"o pedido deve ter o status Recebido")]
        public void ThenThePedidoEmProducaoMustHaveRecebidoStatus()
        {
            response.Should().NotBeNull(because: "resposta da criação do pedido deve ser de sucesso");
            response!.Status.Should().Be(StatusProducaoPedido.Recebido);
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}