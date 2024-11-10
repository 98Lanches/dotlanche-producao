using Dotlanche.Producao.Domain.Entities;
using System.Text.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Dotlanche.Producao.BDDTests.Drivers
{
    public sealed class ProdutoServiceDriver : IDisposable
    {
        private WireMockServer produtoServiceServerMock;

        public const int ServerPort = 8082;

        public ProdutoServiceDriver()
        {
            produtoServiceServerMock = WireMockServer.Start(ServerPort);
        }

        public void SetupGetProdutosByIdMock(IEnumerable<Guid> ids, IEnumerable<Produto> produtos)
        {
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

        public void Dispose()
        {
            produtoServiceServerMock.Dispose();
        }
    }
}