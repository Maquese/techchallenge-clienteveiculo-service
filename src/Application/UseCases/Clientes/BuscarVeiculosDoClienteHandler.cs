using Application.Interfaces;
using Application.Models.Responses;
using Domain.Exceptions;

namespace Application.UseCases.Clientes;

public class BuscarVeiculosDoClienteHandler
{
    private readonly VeiculoRepository _veiculoRepository;

    public BuscarVeiculosDoClienteHandler(VeiculoRepository veiculoRepository)
    {
        _veiculoRepository = veiculoRepository; 
    }

    public async Task<BaseResponse> Handle(int id)
    {
        var veiculos = await _veiculoRepository.BuscarPorCliente(id);
       
        return new BaseResponse
        {
            Message = "Veiculo encontrado com sucesso",
            Success = true,
            Data = veiculos.Select(
                x => x.Id
            )
        };
    }
}
