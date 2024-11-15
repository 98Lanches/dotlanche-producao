using Dotlanche.Producao.Application.Exceptions;
using Dotlanche.Producao.Application.UseCases.Interfaces;
using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.Domain.Ports;
using Dotlanche.Producao.Domain.Repositories;
using Dotlanche.Producao.Domain.ValueObjects;

namespace Dotlanche.Producao.Application.UseCases;

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
        var pedidoInDabatase = await repository.GetById(pedidoConfirmado.Id);
        if (pedidoInDabatase is not null)
            throw new UseCaseException($"Pedido {pedidoConfirmado.Id} already started");

        var combos = await GetCombosProdutosFromPedidoAceito(pedidoConfirmado);

        var newProdutoEmProducao = PedidoEmProducao.StartNew(pedidoConfirmado, combos);
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

            combos.Add(new ComboProdutos(combo.Id, produtosInCombo));
        }

        return combos;
    }
}