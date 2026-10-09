using Domain.Entidades;
using Domain.VOs;

namespace Domain.Aggregates;

public class Cliente : IEntity
{
    protected Cliente() { }
    public Cliente(DocumentoVO documento, string nome, EmailVO email, CelularVO celular)
    {
        this.Documento = documento;
        Nome = nome;
        Email = email;
        Celular = celular;
        Ativo = true;
    }
    public int Id { get; private set; }
    public DocumentoVO Documento { get; private set; }
    public string Nome { get; private set; }
    public EmailVO Email { get; private set; }
    public CelularVO Celular { get; private set; }
    public IList<Veiculo> Veiculos { get; private set; }

    public void Atualizar(string nome, EmailVO email, CelularVO celular)
    {
        Nome = nome;
        Email = email;
        Celular = celular;
    }

    public void AtualizarComDocumento(DocumentoVO documento, string nome, EmailVO email, CelularVO celular)
    {
        Documento = documento;
        Nome = nome;
        Email = email;
        Celular = celular;
    }
}
