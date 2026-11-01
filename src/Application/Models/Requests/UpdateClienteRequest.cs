namespace Application.Models.Requests;

public class UpdateClienteRequest
{
    public int Id { get; set; }
    public string Documento { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Celular { get; set; }
}
