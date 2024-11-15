using AutoBogus;
using Dotlanche.Producao.Domain.Entities;
using Dotlanche.Producao.Domain.ValueObjects;

namespace Dotlanche.Producao.UnitTests.Builders
{
    internal class PedidoEmProducaoBuilder : AutoFaker<PedidoEmProducao>
    {
        public PedidoEmProducaoBuilder()
        {
            CustomInstantiator(f =>
                new PedidoEmProducao(
                    id: Guid.NewGuid(),
                    combosProdutos: new AutoFaker<ComboProdutos>().Generate(f.Random.Int(1, 3)),
                    creationTime: f.Date.Recent(),
                    queueKey: 3,
                    status: StatusProducaoPedido.Recebido,
                    lastUpdateTime: f.Date.Recent()
                    )
             );
        }

        public PedidoEmProducaoBuilder WithPedidoId(Guid pedidoId)
        {
            RuleFor(x => x.Id, pedidoId);
            return this;
        }

        public PedidoEmProducaoBuilder WithStatus(StatusProducaoPedido status)
        {
            RuleFor(x => x.Status, status);
            return this;
        }
    }
}