using Application.Interfaces;
using Application.Models.Responses;
using Domain.Exceptions;

namespace Application.UseCases.Clientes;

public class InativarVeiculoClienteHandler
{
    private readonly VeiculoRepository _veiculoRepository;

    public InativarVeiculoClienteHandler(VeiculoRepository veiculoRepository)
    {
        _veiculoRepository = veiculoRepository;
    }   

    public async Task<BaseResponse> Handle(int id)
    {
        var veiculo = await _veiculoRepository.ObterPorId(id);
        if (veiculo == null)
        {
            throw new DomainException("Veículo não encontrado");
        }

        await _veiculoRepository.Inativar(veiculo);
        return new BaseResponse
        {
            Message = "Veiculo inativado com sucesso",
            Success = true,
            Data = veiculo.Id
        };
    }
}
