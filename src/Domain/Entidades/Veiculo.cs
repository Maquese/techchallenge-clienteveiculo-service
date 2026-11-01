using Domain.Aggregates;
using Domain.VOs;

namespace Domain.Entidades;

public class Veiculo : IEntity
{
    protected Veiculo() { }

    public Veiculo(PlacaVO placa, string modelo, string marca, int ano, int clienteId): base()
    {
        Placa = placa;
        Modelo = modelo;
        Marca = marca;
        Ano = ano;
        ClienteId = clienteId;
        Ativo = true;
    }

    public int Id { get; private set; }
    public PlacaVO Placa { get; private set; }
    public string Modelo { get; private set; }
    public string Marca { get; private set; }
    public int Ano { get; private set; }
    public int ClienteId { get; private set; }
    public Cliente Cliente { get; private set; }

    public void Atualizar(PlacaVO placaVO, string modelo, string marca, int ano)
    {
        Placa = placaVO;
        Modelo = modelo;
        Marca = marca;
        Ano = ano;
    }
}
