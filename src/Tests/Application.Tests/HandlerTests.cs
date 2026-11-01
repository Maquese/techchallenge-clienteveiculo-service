using Application.Interfaces;
using Application.Models.Requests;
using Application.UseCases.Clientes;
using Domain.Aggregates;
using Domain.Entidades;
using Domain.Exceptions;
using Domain.VOs;
using System.Text.Json;

namespace Application.Tests;

public class HandlerTests
{
    [Fact]
    public async Task AdicionarCliente_DevePersistirCliente()
    {
        var repository = new FakeClienteRepository();
        var handler = new AdicionarClienteHandler(repository);

        var response = await handler.Handle(ClienteRequest());

        Assert.True(response.Success);
        Assert.Equal("Cliente adicionado com sucesso.", response.Message);
        Assert.Equal(0, response.Data);
        Assert.IsType<Cliente>(repository.Added);
        Assert.Equal("52998224725", repository.Added!.Documento.Numero);
    }

    [Fact]
    public async Task AdicionarCliente_DevePropagarDocumentoInvalido()
    {
        var handler = new AdicionarClienteHandler(new FakeClienteRepository());
        var request = ClienteRequest();
        request.Documento = "invalido";

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(request));
    }

    [Theory]
    [InlineData("email")]
    [InlineData("celular")]
    public async Task AdicionarCliente_DevePropagarOutrasValidacoesInvalidas(string campo)
    {
        var request = ClienteRequest();
        if (campo == "email")
            request.Email = "invalido";
        else
            request.Celular = "123";

        var handler = new AdicionarClienteHandler(new FakeClienteRepository());

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(request));
    }

    [Fact]
    public async Task AtualizarCliente_DeveAtualizarClienteAtivo()
    {
        var cliente = Cliente();
        var repository = new FakeClienteRepository { Entity = cliente };
        var handler = new AtualizarClienteHandler(repository);
        var request = ClienteUpdateRequest();

        var response = await handler.Handle(request);

        Assert.True(response.Success);
        Assert.Equal(cliente.Id, response.Data);
        Assert.Equal("Maria", cliente.Nome);
        Assert.Equal("52998224725", cliente.Documento.Numero);
        Assert.Equal("maria@dominio.com", cliente.Email.Endereco);
        Assert.Equal("11988888888", cliente.Celular.Numero);
        Assert.Equal(1, repository.UpdateCalls);
    }

    [Fact]
    public async Task AtualizarCliente_DeveRejeitarClienteInexistenteOuInativo()
    {
        var handler = new AtualizarClienteHandler(new FakeClienteRepository());

        var inexistente = await Assert.ThrowsAsync<DomainException>(
            () => handler.Handle(ClienteUpdateRequest()));
        Assert.Equal("Cliente não encontrado", inexistente.Message);

        var cliente = Cliente();
        cliente.Inativar();
        var inativoHandler = new AtualizarClienteHandler(new FakeClienteRepository { Entity = cliente });
        var inativo = await Assert.ThrowsAsync<DomainException>(
            () => inativoHandler.Handle(ClienteUpdateRequest()));
        Assert.Equal("Cliente inativo", inativo.Message);
    }

    [Fact]
    public async Task AtualizarCliente_DevePropagarDadosInvalidos()
    {
        var handler = new AtualizarClienteHandler(new FakeClienteRepository { Entity = Cliente() });
        var request = ClienteUpdateRequest();
        request.Email = "invalido";

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(request));
    }

    [Theory]
    [InlineData("documento")]
    [InlineData("celular")]
    public async Task AtualizarCliente_DevePropagarDocumentoOuCelularInvalido(string campo)
    {
        var request = ClienteUpdateRequest();
        if (campo == "documento")
            request.Documento = "invalido";
        else
            request.Celular = "123";

        var handler = new AtualizarClienteHandler(new FakeClienteRepository { Entity = Cliente() });

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(request));
    }

    [Fact]
    public async Task InativarCliente_DeveInativarClienteExistente()
    {
        var cliente = Cliente();
        var repository = new FakeClienteRepository { Entity = cliente };
        var handler = new InativarClienteHandler(repository);

        var response = await handler.Handle(1);

        Assert.True(response.Success);
        Assert.False(cliente.EstaAtivo());
        Assert.Equal(1, repository.InactivateCalls);
    }

    [Fact]
    public async Task InativarCliente_DeveRejeitarClienteInexistente()
    {
        var handler = new InativarClienteHandler(new FakeClienteRepository());

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(1));
    }

    [Fact]
    public async Task VerificaCadastroCliente_DeveRetornarClienteAtivo()
    {
        var repository = new FakeClienteRepository { Entity = Cliente() };
        var handler = new VerificaCadastroClienteHandler(repository);

        var response = await handler.Handle("52998224725");

        Assert.True(response.Success);
        Assert.Equal("Encontrado com sucesso", response.Message);
    }

    [Fact]
    public async Task VerificaCadastroCliente_DeveRejeitarClienteInexistenteOuInativo()
    {
        var handler = new VerificaCadastroClienteHandler(new FakeClienteRepository());

        var inexistente = await Assert.ThrowsAsync<DomainException>(
            () => handler.Handle("52998224725"));
        Assert.Equal("Cliente não encontrado", inexistente.Message);

        var cliente = Cliente();
        cliente.Inativar();
        var inativoHandler = new VerificaCadastroClienteHandler(new FakeClienteRepository { Entity = cliente });
        var inativo = await Assert.ThrowsAsync<DomainException>(
            () => inativoHandler.Handle("52998224725"));
        Assert.Equal("Cliente inativo", inativo.Message);
    }

    [Fact]
    public async Task AdicionarVeiculo_DevePersistirParaClienteAtivo()
    {
        var veiculoRepository = new FakeVeiculoRepository();
        var handler = new AdicionarVeiculoClienteHandler(
            veiculoRepository,
            new FakeClienteRepository { Entity = Cliente() });

        var response = await handler.Handle(VeiculoRequest());

        Assert.True(response.Success);
        Assert.Equal("Veículo adicionado com sucesso.", response.Message);
        Assert.Equal(0, response.Data);
        Assert.Equal("ABC-1234", veiculoRepository.Added!.Placa.Valor);
    }

    [Fact]
    public async Task AdicionarVeiculo_DevePropagarPlacaInvalida()
    {
        var handler = new AdicionarVeiculoClienteHandler(
            new FakeVeiculoRepository(),
            new FakeClienteRepository { Entity = Cliente() });
        var request = VeiculoRequest();
        request.Placa = "inválida";

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(request));
    }

    [Fact]
    public async Task AdicionarVeiculo_DeveRejeitarClienteInexistenteOuInativo()
    {
        var missingHandler = new AdicionarVeiculoClienteHandler(
            new FakeVeiculoRepository(),
            new FakeClienteRepository());
        var missing = await Assert.ThrowsAsync<DomainException>(
            () => missingHandler.Handle(VeiculoRequest()));
        Assert.Equal("Cliente não encontrado", missing.Message);

        var cliente = Cliente();
        cliente.Inativar();
        var inactiveHandler = new AdicionarVeiculoClienteHandler(
            new FakeVeiculoRepository(),
            new FakeClienteRepository { Entity = cliente });
        var inactive = await Assert.ThrowsAsync<DomainException>(
            () => inactiveHandler.Handle(VeiculoRequest()));
        Assert.Equal("Cliente inativo", inactive.Message);
    }

    [Fact]
    public async Task AtualizarVeiculo_DeveAtualizarVeiculoAtivo()
    {
        var veiculo = Veiculo();
        var repository = new FakeVeiculoRepository { Entity = veiculo };
        var handler = new AtualizarVeiculoClienteHandler(repository);
        var request = VeiculoUpdateRequest();

        var response = await handler.Handle(request);

        Assert.True(response.Success);
        Assert.Equal(veiculo.Id, response.Data);
        Assert.Equal("XYZ1D23", veiculo.Placa.Valor);
        Assert.Equal("Corolla", veiculo.Modelo);
        Assert.Equal("Toyota", veiculo.Marca);
        Assert.Equal(2022, veiculo.Ano);
        Assert.Equal(1, repository.UpdateCalls);
    }

    [Fact]
    public async Task AtualizarVeiculo_DeveRejeitarVeiculoInexistenteOuInativo()
    {
        var handler = new AtualizarVeiculoClienteHandler(new FakeVeiculoRepository());

        var missing = await Assert.ThrowsAsync<DomainException>(
            () => handler.Handle(VeiculoUpdateRequest()));
        Assert.Equal("Veículo não encontrado", missing.Message);

        var veiculo = Veiculo();
        veiculo.Inativar();
        var inactiveHandler = new AtualizarVeiculoClienteHandler(
            new FakeVeiculoRepository { Entity = veiculo });
        var inactive = await Assert.ThrowsAsync<DomainException>(
            () => inactiveHandler.Handle(VeiculoUpdateRequest()));
        Assert.Equal("Veiculo inativo", inactive.Message);
    }

    [Fact]
    public async Task AtualizarVeiculo_DevePropagarPlacaInvalida()
    {
        var handler = new AtualizarVeiculoClienteHandler(
            new FakeVeiculoRepository { Entity = Veiculo() });
        var request = VeiculoUpdateRequest();
        request.Placa = "inválida";

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(request));
    }

    [Fact]
    public async Task InativarVeiculo_DeveInativarVeiculoExistente()
    {
        var veiculo = Veiculo();
        var repository = new FakeVeiculoRepository { Entity = veiculo };
        var handler = new InativarVeiculoClienteHandler(repository);

        var response = await handler.Handle(1);

        Assert.True(response.Success);
        Assert.False(veiculo.EstaAtivo());
        Assert.Equal(1, repository.InactivateCalls);
    }

    [Fact]
    public async Task InativarVeiculo_DeveRejeitarVeiculoInexistente()
    {
        var handler = new InativarVeiculoClienteHandler(new FakeVeiculoRepository());

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(1));
    }

    [Fact]
    public async Task BuscarVeiculoPorId_DeveRetornarVeiculoAtivo()
    {
        var handler = new BuscarVeiculoIdClienteHandler(
            new FakeVeiculoRepository { Entity = Veiculo() });

        var response = await handler.Handle(1);

        Assert.True(response.Success);
        Assert.Equal("Veiculo encontrado com sucesso", response.Message);
    }

    [Fact]
    public async Task BuscarVeiculoPorId_DeveRejeitarVeiculoInexistenteOuInativo()
    {
        var handler = new BuscarVeiculoIdClienteHandler(new FakeVeiculoRepository());

        var missing = await Assert.ThrowsAsync<DomainException>(() => handler.Handle(1));
        Assert.Equal("Veiculo não encontrado", missing.Message);

        var veiculo = Veiculo();
        veiculo.Inativar();
        var inactiveHandler = new BuscarVeiculoIdClienteHandler(
            new FakeVeiculoRepository { Entity = veiculo });
        var inactive = await Assert.ThrowsAsync<DomainException>(() => inactiveHandler.Handle(1));
        Assert.Equal("Veiculo inativo", inactive.Message);
    }

    [Fact]
    public async Task BuscarVeiculoPorPlaca_DeveRetornarDadosDoVeiculoAtivo()
    {
        var handler = new BuscarVeiculoPlacaClienteHandler(
            new FakeVeiculoRepository { Entity = Veiculo() });

        var response = await handler.Handle("ABC-1234");

        Assert.True(response.Success);
        Assert.Equal("Veiculo encontrado com sucesso", response.Message);
        var data = JsonSerializer.SerializeToElement(response.Data);
        Assert.Equal(0, data.GetProperty("Id").GetInt32());
        Assert.Equal("ABC-1234", data.GetProperty("Placa").GetString());
        Assert.Equal("Civic", data.GetProperty("Modelo").GetString());
        Assert.Equal("Honda", data.GetProperty("Marca").GetString());
        Assert.Equal(2020, data.GetProperty("Ano").GetInt32());
    }

    [Fact]
    public async Task BuscarVeiculoPorPlaca_DeveRejeitarVeiculoInexistenteOuInativo()
    {
        var handler = new BuscarVeiculoPlacaClienteHandler(new FakeVeiculoRepository());

        var missing = await Assert.ThrowsAsync<DomainException>(
            () => handler.Handle("ABC-1234"));
        Assert.Equal("Veiculo não encontrado", missing.Message);

        var veiculo = Veiculo();
        veiculo.Inativar();
        var inactiveHandler = new BuscarVeiculoPlacaClienteHandler(
            new FakeVeiculoRepository { Entity = veiculo });
        var inactive = await Assert.ThrowsAsync<DomainException>(
            () => inactiveHandler.Handle("ABC-1234"));
        Assert.Equal("Veiculo inativo", inactive.Message);
    }

    [Fact]
    public async Task BuscarVeiculosDoCliente_DeveRetornarIds()
    {
        var repository = new FakeVeiculoRepository { Entities = [Veiculo(), Veiculo()] };
        var handler = new BuscarVeiculosDoClienteHandler(repository);

        var response = await handler.Handle(1);

        Assert.True(response.Success);
        Assert.Equal(2, Assert.IsAssignableFrom<IEnumerable<int>>(response.Data).Count());
    }

    [Fact]
    public async Task BuscarVeiculosDoCliente_DeveAceitarListaVazia()
    {
        var handler = new BuscarVeiculosDoClienteHandler(new FakeVeiculoRepository());

        var response = await handler.Handle(1);

        Assert.True(response.Success);
        Assert.Empty(Assert.IsAssignableFrom<IEnumerable<int>>(response.Data));
    }

    private static AddClienteRequest ClienteRequest() => new()
    {
        Documento = "52998224725",
        Nome = "João",
        Email = "joao@dominio.com",
        Celular = "11999999999"
    };

    private static UpdateClienteRequest ClienteUpdateRequest() => new()
    {
        Id = 1,
        Documento = "52998224725",
        Nome = "Maria",
        Email = "maria@dominio.com",
        Celular = "11988888888"
    };

    private static AddVeiculoRequest VeiculoRequest() => new()
    {
        ClienteId = 1,
        Placa = "ABC-1234",
        Modelo = "Civic",
        Marca = "Honda",
        Ano = 2020
    };

    private static UpdateVeiculoRequest VeiculoUpdateRequest() => new()
    {
        Id = 1,
        Placa = "XYZ1D23",
        Modelo = "Corolla",
        Marca = "Toyota",
        Ano = 2022
    };

    private static Cliente Cliente() => new(
        new DocumentoVO("52998224725"),
        "João",
        new EmailVO("joao@dominio.com"),
        new CelularVO("11999999999"));

    private static Veiculo Veiculo() => new(
        new PlacaVO("ABC-1234"),
        "Civic",
        "Honda",
        2020,
        1);
}

internal abstract class FakeRepository<T> : BaseRepository<T> where T : IEntity
{
    public T? Entity { get; set; }
    public List<T> Entities { get; set; } = [];
    public T? Added { get; private set; }
    public int UpdateCalls { get; private set; }
    public int InactivateCalls { get; private set; }

    public Task Adicionar(T entity)
    {
        Added = entity;
        Entities.Add(entity);
        return Task.CompletedTask;
    }

    public Task Atualizar(T entity)
    {
        UpdateCalls++;
        return Task.CompletedTask;
    }

    public Task Inativar(T entity)
    {
        InactivateCalls++;
        entity.Inativar();
        return Task.CompletedTask;
    }

    public Task<List<T>> ListarAtivos() =>
        Task.FromResult(Entities.Where(entity => entity.EstaAtivo()).ToList());

    public Task<T> ObterPorId(int id) => Task.FromResult(Entity!);

    public Task<List<T>> ListarPorIds(List<int> ids) => Task.FromResult(Entities);

    public Task Remover(T entity) => Task.CompletedTask;
}

internal sealed class FakeClienteRepository : FakeRepository<Cliente>, ClienteRepository
{
    public Task<Cliente> ObterPorDocumento(string documento) => Task.FromResult(Entity!);
}

internal sealed class FakeVeiculoRepository : FakeRepository<Veiculo>, VeiculoRepository
{
    public Task<Veiculo> BuscarPorPlaca(string placa) => Task.FromResult(Entity!);

    public Task<IList<Veiculo>> BuscarPorCliente(int idCliente) =>
        Task.FromResult<IList<Veiculo>>(Entities);
}
