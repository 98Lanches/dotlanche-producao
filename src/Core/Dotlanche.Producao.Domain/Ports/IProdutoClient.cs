using Dotlanche.Producao.Domain.Entities;

namespace Dotlanche.Producao.Domain.Ports
{
    public interface IProdutoClient
    {
        Task<IEnumerable<Produto>> GetByIds(IEnumerable<Guid> ids);
    }
}
