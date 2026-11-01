using Application.Models.Requests;

namespace Application.Tests;

public class RequestModelTests
{
    [Fact]
    public void AddClienteRequest_DeveManterIdOpcional()
    {
        var request = new AddClienteRequest { Id = 7 };

        Assert.Equal(7, request.Id);
    }

    [Fact]
    public void AddVeiculoRequest_DeveManterIdOpcional()
    {
        var request = new AddVeiculoRequest { Id = 9 };

        Assert.Equal(9, request.Id);
    }

    [Fact]
    public void UpdateVeiculoRequest_DeveManterClienteId()
    {
        var request = new UpdateVeiculoRequest { ClienteId = 12 };

        Assert.Equal(12, request.ClienteId);
    }
}
