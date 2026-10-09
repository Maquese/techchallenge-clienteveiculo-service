using Application.Interfaces;
using Application.Models.Responses;
using Domain.Exceptions;

namespace Application.UseCases.Clientes;

public class InativarClienteHandler
{
    private readonly ClienteRepository _clienteRepository;

    public InativarClienteHandler(ClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }   

    public async Task<BaseResponse> Handle(int id)
    {
        var cliente = await _clienteRepository.ObterPorId(id);
        if (cliente == null)
        {
            throw new DomainException("Cliente não encontrado");
        }

        await _clienteRepository.Inativar(cliente);
        return new BaseResponse
        {
            Message = "Cliente inativado com sucesso",
            Success = true,
            Data = cliente.Id
        };
    }
}
