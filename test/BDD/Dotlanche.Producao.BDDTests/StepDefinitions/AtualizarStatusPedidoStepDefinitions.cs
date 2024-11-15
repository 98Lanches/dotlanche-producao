using AutoBogus;
using Dotlanche.Producao.BDDTests.DataTableObjects;
using Dotlanche.Producao.BDDTests.Setup;
using Dotlanche.Producao.Data.Repositories;
using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.Domain.ValueObjects;
using Dotlanche.Producao.WebApi.DTOs.Requests;
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
    [Scope(Feature = "Atualizar status do pedido")]
    public sealed class AtualizarStatusPedidoStepDefinitions : IDisposable
    {
        private readonly HttpClient apiClient;
        private readonly IServiceScope scope;

        private readonly JsonSerializerOptions jsonOptions;
        private UpdateStatusPedidoRequest request;
        private HttpResponseMessage? response;
        private IMongoCollection<PedidoEmProducao> dbCollection;

        public AtualizarStatusPedidoStepDefinitions(WebApiFactory apiFactory)
        {
            apiClient = apiFactory.CreateClient();
            scope = apiFactory.Services.CreateScope();

            jsonOptions = new() { PropertyNameCaseInsensitive = true };
            jsonOptions.Converters.Add(new JsonStringEnumConverter());

            request = new AutoFaker<UpdateStatusPedidoRequest>();

            var db = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
            db.DropCollection(PedidoEmProducaoRepository.pedidosCollection);

            dbCollection = db.GetCollection<PedidoEmProducao>(PedidoEmProducaoRepository.pedidosCollection);
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

            if (pedidosToInsert.Any())
                await dbCollection.InsertManyAsync(pedidosToInsert);
        }

        [When(@"eu solicitar a atualização do pedido id (.*) para o status (.*)")]
        public async Task WhenIRequestPedidoStatusUpdate(Guid idPedido, StatusProducaoPedido newStatus)
        {
            const string updatePedidosRoute = "/api/producao/status";
            request = new UpdateStatusPedidoRequest()
            {
                IdPedido = idPedido,
                Status = newStatus,
            };

            var httpResponse = await apiClient.PutAsJsonAsync(updatePedidosRoute, request);
            response = httpResponse;
        }

        [Then("deve atualizar o status do pedido")]
        public async Task ThenItShouldUpdatePedidoStatus()
        {
            var pedidoInDatabase = (await dbCollection.FindAsync(x => x.Id == request.IdPedido)).Single();
            pedidoInDatabase.Status.Should().Be(request.Status);
        }

        [Then("retornar o novo status na resposta")]
        public async Task ThenItShouldReturnNewStatusInResponse()
        {
            response.Should().NotBeNull(because: "response must be set for this assertion");
            response!.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedPedido = await response.Content.ReadFromJsonAsync<UpdateStatusPedidoResponse>(jsonOptions);
            updatedPedido!.Id.Should().Be(request.IdPedido);
            updatedPedido!.Status.Should().Be(request.Status);
        }

        [Then("deve retornar mensagem dizendo que pedido não existe")]
        public async Task ThenItShouldReturnAMessageSayingThePedidoWasNotFound()
        {
            response.Should().NotBeNull(because: "response must be set for this assertion");
            response!.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var responseMessage = await response.Content.ReadAsStringAsync();
            responseMessage.Should().Be("Pedido does not exist");
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}