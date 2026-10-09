namespace Application.Models.Requests;

public class AddVeiculoRequest
{
    public int? Id { get;  set; }
    public string Placa { get; set; }
    public string Modelo { get; set; }
    public string Marca { get; set; }
    public int Ano { get; set; }
    public int ClienteId { get; set; }
}
