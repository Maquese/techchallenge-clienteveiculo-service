using System;
using Xunit;
using Domain.Aggregates;
using Domain.VOs;

namespace Domain.Tests
{
    public class ClienteTests
    {
        [Fact]
        public void Construtor_DeveInicializarCorretamente()
        {
            // Arrange
            var documento = new DocumentoVO("11144477735"); // CPF válido
            var email = new EmailVO("teste@dominio.com");
            var celular = new CelularVO("11999999999");
            string nome = "João da Silva";

            // Act
            var cliente = new Cliente(documento, nome, email, celular);

            // Assert
            Assert.Equal(documento, cliente.Documento);
            Assert.Equal(nome, cliente.Nome);
            Assert.Equal(email, cliente.Email);
            Assert.Equal(celular, cliente.Celular);
            Assert.True(cliente.Ativo);
        }

        [Fact]
        public void Atualizar_DeveAlterarValoresCorretamente()
        {
            // Arrange
            var cliente = new Cliente(
                new DocumentoVO("11144477735"),
                "João da Silva",
                new EmailVO("teste@dominio.com"),
                new CelularVO("11999999999")
            );

            var novoEmail = new EmailVO("novo@dominio.com");
            var novoCelular = new CelularVO("11988888888");
            string novoNome = "Maria Oliveira";

            // Act
            cliente.Atualizar(novoNome, novoEmail, novoCelular);

            // Assert
            Assert.Equal(novoNome, cliente.Nome);
            Assert.Equal(novoEmail, cliente.Email);
            Assert.Equal(novoCelular, cliente.Celular);
        }

        [Fact]
        public void AtualizarComDocumento_DeveAlterarTodosOsValores()
        {
            // Arrange
            var cliente = new Cliente(
                new DocumentoVO("11144477735"),
                "João da Silva",
                new EmailVO("teste@dominio.com"),
                new CelularVO("11999999999")
            );

            var novoDocumento = new DocumentoVO("52998224725"); // CPF válido
            var novoEmail = new EmailVO("novo@dominio.com");
            var novoCelular = new CelularVO("11988888888");
            string novoNome = "Carlos Pereira";

            // Act
            cliente.AtualizarComDocumento(novoDocumento, novoNome, novoEmail, novoCelular);

            // Assert
            Assert.Equal(novoDocumento, cliente.Documento);
            Assert.Equal(novoNome, cliente.Nome);
            Assert.Equal(novoEmail, cliente.Email);
            Assert.Equal(novoCelular, cliente.Celular);
        }
    }
}
