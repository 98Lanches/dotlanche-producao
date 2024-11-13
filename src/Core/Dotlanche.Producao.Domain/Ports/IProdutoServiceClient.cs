using Dotlanche.Producao.Domain.Entities;

namespace Dotlanche.Producao.Domain.Ports
{
    public interface IProdutoServiceClient
    {
        Task<IEnumerable<Produto>> GetByIds(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    }
}
