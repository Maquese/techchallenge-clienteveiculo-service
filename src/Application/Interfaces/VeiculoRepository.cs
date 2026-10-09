using Domain.Entidades;
namespace Application.Interfaces; 

public interface VeiculoRepository : BaseRepository<Veiculo>
{
    Task<Veiculo> BuscarPorPlaca(string placa);
}
