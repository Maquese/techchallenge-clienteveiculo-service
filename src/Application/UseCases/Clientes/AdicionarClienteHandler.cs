
using Application.Interfaces;
using Application.Models.Requests;
using Application.Models.Responses;
using Domain.Aggregates;
using Domain.Exceptions;
using Domain.VOs;


namespace Application.UseCases.Clientes;

public class AdicionarClienteHandler
{
    private readonly ClienteRepository _clienteRepository;

    public AdicionarClienteHandler(ClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<BaseResponse> Handle(AddClienteRequest clienteRequest)
    {
        try
        {
            var cliente = new Cliente
            (
                new DocumentoVO(clienteRequest.Documento),
                clienteRequest.Nome,
                new EmailVO(clienteRequest.Email),
                new CelularVO(clienteRequest.Celular)
            );

            await _clienteRepository.Adicionar(cliente);
            return new BaseResponse
            {
                Success = true,
                Message = "Cliente adicionado com sucesso.",
                Data = cliente.Id
            };
        }
        catch (DomainException ex)
        {
            throw ex;
        }
    }
}
