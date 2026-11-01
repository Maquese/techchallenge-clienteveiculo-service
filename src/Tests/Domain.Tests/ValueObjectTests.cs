using Domain.Exceptions;
using Domain.VOs;

namespace Domain.Tests;

public class ValueObjectTests
{
    [Theory]
    [InlineData("11999999999")]
    [InlineData("(11) 99999-9999")]
    public void CelularVO_DeveAceitarNumeroComOnzeDigitos(string numero)
    {
        var celular = new CelularVO(numero);

        Assert.Equal(numero, celular.Numero);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    [InlineData("1199999999")]
    [InlineData("119999999999")]
    public void CelularVO_DeveRejeitarNumeroInvalido(string numero)
    {
        Assert.Throws<DomainException>(() => new CelularVO(numero));
    }

    [Theory]
    [InlineData("529.982.247-25", "52998224725", "CPF")]
    [InlineData("11.222.333/0001-81", "11222333000181", "CNPJ")]
    public void DocumentoVO_DeveNormalizarCpfECnpj(string numero, string esperado, string tipo)
    {
        var documento = new DocumentoVO(numero);

        Assert.Equal(esperado, documento.Numero);
        Assert.Equal(tipo, documento.Tipo);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("-----")]
    [InlineData("111.111.111-11")]
    [InlineData("52998224724")]
    [InlineData("11.111.111/1111-11")]
    [InlineData("11222333000180")]
    public void DocumentoVO_DeveRejeitarDocumentoInvalido(string numero)
    {
        Assert.Throws<DomainException>(() => new DocumentoVO(numero));
    }

    [Theory]
    [InlineData("teste@dominio.com")]
    [InlineData("nome.sobrenome+teste@sub.dominio.com.br")]
    public void EmailVO_DeveAceitarEnderecoValido(string endereco)
    {
        var email = new EmailVO(endereco);

        Assert.Equal(endereco, email.Endereco);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    [InlineData("email-invalido")]
    [InlineData("nome@dominio.com;extra")]
    [InlineData("Nome <nome@dominio.com>")]
    public void EmailVO_DeveRejeitarEnderecoInvalido(string endereco)
    {
        Assert.Throws<DomainException>(() => new EmailVO(endereco));
    }

    [Theory]
    [InlineData("ABC-1234", "ABC-1234")]
    [InlineData(" abc1d23 ", "ABC1D23")]
    public void PlacaVO_DeveAceitarENormalizarOsFormatosSuportados(string entrada, string esperado)
    {
        var placa = new PlacaVO(entrada);

        Assert.Equal(esperado, placa.Valor);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    [InlineData("AB-12345")]
    [InlineData("ABC1234")]
    public void PlacaVO_DeveRejeitarFormatoInvalido(string placa)
    {
        Assert.Throws<DomainException>(() => new PlacaVO(placa));
    }

    [Fact]
    public void PlacaVO_DeveCompararPorValor()
    {
        var primeira = new PlacaVO("ABC-1234");
        var equivalente = new PlacaVO("abc-1234");
        var diferente = new PlacaVO("XYZ-9876");

        Assert.True(primeira.Equals(equivalente));
        Assert.Equal(primeira.GetHashCode(), equivalente.GetHashCode());
        Assert.False(primeira.Equals(diferente));
        Assert.False(primeira.Equals(null));
        Assert.False(primeira.Equals("ABC-1234"));
    }
}
