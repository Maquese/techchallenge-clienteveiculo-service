using Application.Interfaces;
using Application.Models.Requests;
using Application.Models.Responses;
using Domain.Exceptions;
using Domain.VOs;

namespace Application.UseCases.Clientes;

public class AtualizarClienteHandler
{
    private readonly ClienteRepository _clienteRepository;

    public AtualizarClienteHandler(ClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<BaseResponse> Handle(UpdateClienteRequest clienteModel)
    {
        var cliente = await _clienteRepository.ObterPorId(clienteModel.Id);
        if (cliente == null)
        {
            throw new DomainException("Cliente não encontrado");
        }

        if(!cliente.EstaAtivo())
            throw new DomainException("Cliente inativo");

        try
        {
            cliente.AtualizarComDocumento(
                new DocumentoVO(clienteModel.Documento),
                clienteModel.Nome,
                new EmailVO(clienteModel.Email),
                new CelularVO(clienteModel.Celular)
            );

            await _clienteRepository.Atualizar(cliente);
        }
        catch (DomainException ex)
        {
            throw ex;
        }

        return new BaseResponse
        {
            Data = cliente.Id,
            Message = "Atualizado com sucesso",
            Success = true
        };
    }
}
