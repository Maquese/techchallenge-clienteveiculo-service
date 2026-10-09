using Application.Interfaces;
using Application.Models.Requests;
using Application.Models.Responses;
using Domain.Exceptions;
using Domain.VOs;

namespace Application.UseCases.Clientes;

public class AtualizarVeiculoClienteHandler
{
    private readonly VeiculoRepository _veiculoRepository;

    public AtualizarVeiculoClienteHandler(VeiculoRepository veiculoRepository)
    {
        _veiculoRepository = veiculoRepository;
    }    

    public async Task<BaseResponse> Handle(UpdateVeiculoRequest veiculoModel)
    {
        var veiculo = await _veiculoRepository.ObterPorId(veiculoModel.Id);
        if (veiculo == null)
        {
            throw new DomainException("Veículo não encontrado");
        }

        if(!veiculo.EstaAtivo())
            throw new DomainException("Veiculo inativo");

        veiculo.Atualizar(new PlacaVO(veiculoModel.Placa), veiculoModel.Modelo, veiculoModel.Marca, veiculoModel.Ano);

        await _veiculoRepository.Atualizar(veiculo);

        return new BaseResponse
        {
            Success = true,
            Message = "Veiculo atualizado com sucesso",
            Data  = veiculo.Id
        };
    }
}
