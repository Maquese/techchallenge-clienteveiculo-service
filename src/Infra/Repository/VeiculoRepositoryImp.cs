using Application.Interfaces;
using Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Infra;

public class VeiculoRepositoryImp : BaseRepositoryImp<Veiculo>, VeiculoRepository
{
    public VeiculoRepositoryImp(EFContext context) : base(context)
    {
        
    }

    public async Task<Veiculo> BuscarPorPlaca(string placa)
    {
        return await _context.Veiculos.FirstOrDefaultAsync(v => v.Placa.Valor == new Domain.VOs.PlacaVO(placa).Valor);
    }
}

