using Application.Interfaces;
using Application.Models.Responses;
using Domain.Exceptions;

namespace Application.UseCases.Clientes;

public class BuscarVeiculoIdClienteHandler
{
    private readonly VeiculoRepository _veiculoRepository;

    public BuscarVeiculoIdClienteHandler(VeiculoRepository veiculoRepository)
    {
        _veiculoRepository = veiculoRepository; 
    }

    public async Task<BaseResponse> Handle(int id)
    {
        var veiculo = await _veiculoRepository.ObterPorId(id);
        if (veiculo == null)
        {
            throw new DomainException("Veiculo não encontrado");
        }

        if(!veiculo.EstaAtivo())
            throw new DomainException("Veiculo inativo");

        return new BaseResponse
        {
            Message = "Veiculo encontrado com sucesso",
            Success = true,
            Data = new
            {
                Existe = true
            }
        };
    }
}
