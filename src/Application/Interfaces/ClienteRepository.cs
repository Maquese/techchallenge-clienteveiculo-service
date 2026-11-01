using Domain.Aggregates;

namespace Application.Interfaces; 

public interface ClienteRepository : BaseRepository<Cliente>
{
    Task<Cliente> ObterPorDocumento(string documento);
}
