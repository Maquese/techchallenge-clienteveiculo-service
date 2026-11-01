using System;
using Xunit;
using Domain.Entidades;
using Domain.VOs;

namespace Domain.Tests
{
    public class VeiculoTests
    {
        [Fact]
        public void Construtor_DeveInicializarCorretamente()
        {
            // Arrange
            var placa = new PlacaVO("ABC-1234"); // placa válida
            string modelo = "Civic";
            string marca = "Honda";
            int ano = 2020;
            int clienteId = 1;

            // Act
            var veiculo = new Veiculo(placa, modelo, marca, ano, clienteId);

            // Assert
            Assert.Equal(placa, veiculo.Placa);
            Assert.Equal(modelo, veiculo.Modelo);
            Assert.Equal(marca, veiculo.Marca);
            Assert.Equal(ano, veiculo.Ano);
            Assert.Equal(clienteId, veiculo.ClienteId);
            Assert.True(veiculo.Ativo);
        }

        [Fact]
        public void Atualizar_DeveAlterarValoresCorretamente()
        {
            // Arrange
            var veiculo = new Veiculo(new PlacaVO("ABC-1234"), "Civic", "Honda", 2020, 1);

            var novaPlaca = new PlacaVO("XYZ1D23"); // placa válida Mercosul
            string novoModelo = "Corolla";
            string novaMarca = "Toyota";
            int novoAno = 2022;

            // Act
            veiculo.Atualizar(novaPlaca, novoModelo, novaMarca, novoAno);

            // Assert
            Assert.Equal(novaPlaca, veiculo.Placa);
            Assert.Equal(novoModelo, veiculo.Modelo);
            Assert.Equal(novaMarca, veiculo.Marca);
            Assert.Equal(novoAno, veiculo.Ano);
        }

        [Fact]
        public void Construtor_DeveDefinirAtivoComoTrue()
        {
            // Act
            var veiculo = new Veiculo(new PlacaVO("DEF-5678"), "Fiesta", "Ford", 2018, 2);

            // Assert
            Assert.True(veiculo.Ativo);
        }
    }
}
