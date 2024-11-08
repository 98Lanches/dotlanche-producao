using Dotlanche.Producao.BDDTests.DataTableObjects;
using Dotlanche.Producao.BDDTests.Drivers;
using Dotlanche.Producao.BDDTests.Setup;
using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.WebApi.DTOs;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dotlanche.Producao.BDDTests.StepDefinitions
{
    [Binding]
    public sealed class StartProducaoPedidoStepDefinitions : IDisposable
    {
        private readonly HttpClient apiClient;
        private readonly IServiceScope scope;

        private readonly JsonSerializerOptions jsonOptions;
        private readonly ProdutoServiceDriver produtoServiceDriver;
        private StartProducaoPedidoRequest request;
        private StartProducaoPedidoResponse? response;

        public StartProducaoPedidoStepDefinitions(WebApiFactory apiFactory, ProdutoServiceDriver produtoServiceDriver)
        {
            apiClient = apiFactory.CreateClient();
            scope = apiFactory.Services.CreateScope();
            this.produtoServiceDriver = produtoServiceDriver;

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
            produtoServiceDriver.SetupGetProdutosByIdMock(request.Combos.SelectMany(x => x.ProdutoGuids).Distinct(), existingProdutos);
        }

        [Given("o pedido possui os seguintes combos:")]
        public void GivenPedidoHasComboWithThisIdAndThisProducts(DataTable combosTable)
        {
            var combos = combosTable
                            .CreateSet<ComboDataTableObject>()
                            .GroupBy(x => x.ComboId)
                            .Select(x => new ComboDTO
                            {
                                Id = x.Key,
                                ProdutoGuids = x.Select(y => y.ProdutoId)
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
            ClearDatabaseCollection(scope.ServiceProvider);
            scope.Dispose();
        }

        private void ClearDatabaseCollection(IServiceProvider serviceProvider)
        {
            var client = serviceProvider.GetRequiredService<MongoClient>();
            client.DropDatabase("dotlanche-produto");
        }
    }
}