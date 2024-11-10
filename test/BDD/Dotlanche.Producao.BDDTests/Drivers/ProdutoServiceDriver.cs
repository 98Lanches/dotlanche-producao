using Dotlanche.Producao.Domain.Entities;
using System.Text.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Dotlanche.Producao.BDDTests.Drivers
{
    public static class ProdutoServiceDriver
    {
        public static WireMockServer? produtoServiceServerMock;

        public static void SetWiremockServer(WireMockServer wireMockServer)
        {
            produtoServiceServerMock = wireMockServer;
        }

        public static void SetupGetProdutosByIdMock(IEnumerable<Guid> ids, IEnumerable<Produto> produtos)
        {
            if (produtoServiceServerMock == null || !produtoServiceServerMock.IsStarted)
                throw new InvalidOperationException("Wiremock server needs to be set before setting it up!");

            var responseJson = JsonSerializer.Serialize(produtos);

            produtoServiceServerMock
                .Given(
                    Request.Create()
                    .WithPath("/produtos")
                 )
                .RespondWith(
                    Response.Create()
                    .WithStatusCode(System.Net.HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(responseJson)
                );
        }
    }
}