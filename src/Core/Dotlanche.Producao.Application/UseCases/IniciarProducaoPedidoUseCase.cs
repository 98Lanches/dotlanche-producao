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
        private readonly IProdutoClient produtoClient;

        public IniciarProducaoPedidoUseCase(IPedidoEmProducaoRepository repository,
                                            IProdutoClient produtoClient)
        {
            this.repository = repository;
            this.produtoClient = produtoClient;
        }

        public async Task<PedidoEmProducao> ExecuteAsync(PedidoAceito pedidoAceito)
        {
            var nextKeyTask = repository.GetNextKey();
            var getCombosTask = GetCombosProdutosFromPedidoAceito(pedidoAceito);

            await Task.WhenAll(getCombosTask, nextKeyTask);

            var nextKey = await nextKeyTask;
            var combos = await getCombosTask;

            var newProdutoEmProducao = new PedidoEmProducao(pedidoAceito, combos, nextKey);
            newProdutoEmProducao = await repository.Add(newProdutoEmProducao);

            return newProdutoEmProducao;
        }

        private async Task<IEnumerable<ComboProdutos>> GetCombosProdutosFromPedidoAceito(PedidoAceito pedidoAceito)
        {
            var comboDict = pedidoAceito.Combos.ToDictionary(c => c.Id, c => c.ProdutoIds.ToList());

            var allProdutoIds = comboDict.SelectMany(x => x.Value).Distinct();
            var produtos = await produtoClient.GetByIds(allProdutoIds);

            var combos = new List<ComboProdutos>(capacity: comboDict.Keys.Count);
            foreach (var (comboId, produtoIdList) in comboDict)
            {
                var produtosInCombo = new List<Produto>(capacity: produtoIdList.Count);
                foreach (var idProduto in produtoIdList)
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