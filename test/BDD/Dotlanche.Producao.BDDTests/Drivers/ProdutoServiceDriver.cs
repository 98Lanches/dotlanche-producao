using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.Integrations.Adapters.ProdutoService.Responses;
using System.Text.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Dotlanche.Producao.BDDTests.Drivers
{
    public static class ProdutoServiceDriver
    {
        private static WireMockServer? produtoServiceServerMock;

        public static void SetWiremockServer(WireMockServer wireMockServer)
        {
            produtoServiceServerMock = wireMockServer;
        }

        public static void SetupGetProdutosByIdMock(IEnumerable<Guid> ids, IEnumerable<Produto> produtos)
        {
            if (produtoServiceServerMock == null || !produtoServiceServerMock.IsStarted)
                throw new InvalidOperationException("Wiremock server needs to be set before setting it up!");

            var produtosServiceResponse = produtos.Select(x =>
                new GetProdutosByIdsResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Categoria = new()
                    {
                        Id = 1,
                        Name = x.Name,
                    }
                }
            );
            var responseJson = JsonSerializer.Serialize(produtosServiceResponse);

            produtoServiceServerMock
                .Given(
                    Request.Create()
                    .WithPath("/produto")
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