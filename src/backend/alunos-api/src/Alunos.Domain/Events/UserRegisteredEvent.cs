namespace Alunos.Domain.Events;

public class UserRegisteredEvent
{
    public UserRegisteredEvent()
    {
    }

    public UserRegisteredEvent(string userId, string email, string nome, string cpf, DateTime dataNascimento, string telefone, string genero, string cidade, string estado, string cep, string? foto, bool ehAdministrador, DateTime dataCadastro)
    {
        UserId = userId;
        Email = email;
        Nome = nome;
        CPF = cpf;
        DataNascimento = dataNascimento;
        Telefone = telefone;
        Genero = genero;
        Cidade = cidade;
        Estado = estado;
        CEP = cep;
        Foto = foto;
        EhAdministrador = ehAdministrador;
        DataCadastro = dataCadastro;
    }

    public string UserId { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Nome { get; set; } = string.Empty;

    public string CPF { get; set; } = string.Empty;

    public DateTime DataNascimento { get; set; }

    public string Telefone { get; set; } = string.Empty;

    public string Genero { get; set; } = string.Empty;

    public string Cidade { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public string CEP { get; set; } = string.Empty;

    public string? Foto { get; set; }

    public bool EhAdministrador { get; set; }

    public DateTime DataCadastro { get; set; }
}