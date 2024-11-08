using Dotlanche.Producao.Application.Exceptions;
using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.Domain.Ports;
using Dotlanche.Producao.Domain.Repositories;
using Dotlanche.Producao.Domain.ValueObjects;

namespace Dotlanche.Producao.Application.UseCases
{
    public class IniciarProducaoPedidoUseCase : IIniciarProducaoPedidoUseCase
    {
        private readonly IPedidoEmProducaoRepository repository;
        private readonly IProdutoServiceClient produtoClient;

        public IniciarProducaoPedidoUseCase(IPedidoEmProducaoRepository repository,
                                            IProdutoServiceClient produtoClient)
        {
            this.repository = repository;
            this.produtoClient = produtoClient;
        }

        public async Task<PedidoEmProducao> ExecuteAsync(PedidoConfirmado pedidoConfirmado)
        {
            var nextKeyTask = repository.GetNextKey();
            var getCombosTask = GetCombosProdutosFromPedidoAceito(pedidoConfirmado);

            await Task.WhenAll(getCombosTask, nextKeyTask);

            var nextKey = await nextKeyTask;
            var combos = await getCombosTask;

            var newProdutoEmProducao = new PedidoEmProducao(pedidoConfirmado, combos, nextKey);
            newProdutoEmProducao = await repository.Add(newProdutoEmProducao);

            return newProdutoEmProducao;
        }

        private async Task<IEnumerable<ComboProdutos>> GetCombosProdutosFromPedidoAceito(PedidoConfirmado pedidoConfirmado)
        {
            var allProdutoIds = pedidoConfirmado.Combos.SelectMany(x => x.ProdutoIds).Distinct();
            var produtos = await produtoClient.GetByIds(allProdutoIds);

            var combos = new List<ComboProdutos>();

            foreach (var combo in pedidoConfirmado.Combos)
            {
                var produtosInCombo = new List<Produto>();
                foreach (var idProduto in combo.ProdutoIds)
                {
                    var produto = produtos.FirstOrDefault(x => x.Id == idProduto) ??
                        throw new UseCaseException($"Produto {idProduto} not found!");

                    produtosInCombo.Add(produto);
                }

                combos.Add(new ComboProdutos(produtosInCombo));
            }

            return combos;
        }
    }
}