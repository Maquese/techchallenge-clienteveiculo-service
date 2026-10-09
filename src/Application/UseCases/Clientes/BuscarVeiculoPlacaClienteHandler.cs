using Application.Interfaces;
using Application.Models.Responses;
using Domain.Exceptions;

namespace Application.UseCases.Clientes;

public class BuscarVeiculoPlacaClienteHandler
{
    private readonly VeiculoRepository _veiculoRepository;

    public BuscarVeiculoPlacaClienteHandler(VeiculoRepository veiculoRepository)
    {
        _veiculoRepository = veiculoRepository; 
    }

    public async Task<BaseResponse> Handle(string placa)
    {
        var veiculo = await _veiculoRepository.BuscarPorPlaca(placa);
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
                Id = veiculo.Id,
                Placa = veiculo.Placa.Valor,
                Modelo = veiculo.Modelo,
                Marca = veiculo.Marca,
                Ano = veiculo.Ano
            }
        };
    }
}
