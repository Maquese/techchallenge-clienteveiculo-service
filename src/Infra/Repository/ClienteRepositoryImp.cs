using Application.Interfaces;
using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
namespace Infra.Repository;

public class ClienteRepositoryImp : BaseRepositoryImp<Cliente>, ClienteRepository
{
    public ClienteRepositoryImp(EFContext context) : base(context)
    {
    }

    public async Task<Cliente> ObterPorDocumento(string documento)
    {
        return await _context.Clientes.FirstOrDefaultAsync(c => c.Documento.Numero == documento);
    }

    public override async Task<Cliente> ObterPorId(int id)
    {
        return await _context.Clientes
            .Include(c => c.Veiculos)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}
