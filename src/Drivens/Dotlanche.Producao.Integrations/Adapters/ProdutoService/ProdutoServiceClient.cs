using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.Domain.Ports;
using Dotlanche.Producao.Integrations.Adapters.ProdutoService.Responses;
using System.Text.Json;

namespace Dotlanche.Producao.Integrations.Adapters.ProdutoService
{
    internal class ProdutoServiceClient : IProdutoServiceClient
    {
        private readonly HttpClient client;
        private readonly JsonSerializerOptions defaultJsonOptions = new() { PropertyNameCaseInsensitive = true };

        public ProdutoServiceClient(HttpClient client)
        {
            this.client = client;
        }

        public async Task<IEnumerable<Produto>> GetByIds(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            var response = await client.GetAsync($"/produto?orderList={string.Join(',', ids)}", cancellationToken);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var produtoResponse = JsonSerializer.Deserialize<IEnumerable<GetProdutosByIdsResponse>>(content, defaultJsonOptions) ?? [];

            var produtos = produtoResponse.Select(x => new Produto(x.Id, x.Name, x.Categoria ?? string.Empty, x.Price ?? 0));

            return produtos;
        }
    }
}