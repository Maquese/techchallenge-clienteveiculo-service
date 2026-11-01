using Application.Interfaces;
using Application.Models.Responses;
using Domain.Exceptions;

namespace Application.UseCases.Clientes;

public class VerificaCadastroClienteHandler
{
    private readonly ClienteRepository _clienteRepository;

    public VerificaCadastroClienteHandler(ClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<BaseResponse> Handle(string documento)
    {
        var cliente = await _clienteRepository.ObterPorDocumento(documento);
        if (cliente == null)
        {
            throw new DomainException("Cliente não encontrado");
        }

        if(!cliente.EstaAtivo())
        {
            throw new DomainException("Cliente inativo");
        }

        return new BaseResponse
        {
            Message = "Encontrado com sucesso",
            Success = true,
            Data = cliente.Id
        };
    }
}
