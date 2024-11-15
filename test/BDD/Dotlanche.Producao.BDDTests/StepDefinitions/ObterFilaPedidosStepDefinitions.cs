using AutoBogus;
using Dotlanche.Producao.BDDTests.DataTableObjects;
using Dotlanche.Producao.BDDTests.Setup;
using Dotlanche.Producao.Data.Repositories;
using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.Domain.ValueObjects;
using Dotlanche.Producao.WebApi.DTOs.Responses;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dotlanche.Producao.BDDTests.StepDefinitions
{
    [Binding]
    public sealed class ObterFilaPedidosStepDefinitions : IDisposable
    {
        private readonly HttpClient apiClient;
        private readonly IServiceScope scope;

        private readonly JsonSerializerOptions jsonOptions;
        private GetPedidosQueueResponse? response;

        public ObterFilaPedidosStepDefinitions(WebApiFactory apiFactory)
        {
            apiClient = apiFactory.CreateClient();
            scope = apiFactory.Services.CreateScope();

            jsonOptions = new() { PropertyNameCaseInsensitive = true };
            jsonOptions.Converters.Add(new JsonStringEnumConverter());

            var db = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
            db.DropCollection(PedidoEmProducaoRepository.pedidosCollection);
        }

        [Given("os seguintes pedidos estão cadastrados:")]
        public async Task GivenTheFollowingPedidosAreRegistered(DataTable pedidosDataTable)
        {
            var existingPedidos = pedidosDataTable.CreateSet<PedidoCadastradoDataTableObject>();

            var produtosFaker = new AutoFaker<Produto>();
            var produtoList = produtosFaker.Generate(2);

            var pedidosToInsert = existingPedidos.Select(existing =>
                new PedidoEmProducao(existing.PedidoId,
                                     [new ComboProdutos(Guid.NewGuid(), produtoList)],
                                     existing.CreationTime,
                                     existing.QueueKey,
                                     existing.Status,
                                     existing.CreationTime)
            );

            var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
            var collection = database.GetCollection<PedidoEmProducao>(PedidoEmProducaoRepository.pedidosCollection);

            if(pedidosToInsert.Any())
                await collection.InsertManyAsync(pedidosToInsert);
        }

        [When("eu solicitar a fila de pedidos em producao")]
        public async Task WhenIRequestGetPedidosQueue()
        {
            const string getPedidosQueueRoute = "/api/queue";
            var httpResponse = await apiClient.GetAsync(getPedidosQueueRoute);

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK, because: "a requisição não pode falhar");

            if (httpResponse.IsSuccessStatusCode)
                response = await httpResponse.Content.ReadFromJsonAsync<GetPedidosQueueResponse>(jsonOptions);
        }

        [Then("a fila deve ser retornada na seguinte ordem:")]
        public void ThenTheQueueShouldHaveTheFollowingOrder(DataTable queueItemsDataTable)
        {
            var expectedQueue = queueItemsDataTable.CreateSet<QueueItemDataTableObject>();

            response!.Items.Should()
                .BeEquivalentTo(expectedQueue, opt => opt.WithStrictOrdering(),
                because: "Fila de pedidos precisa seguir a ordem esperada");
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}