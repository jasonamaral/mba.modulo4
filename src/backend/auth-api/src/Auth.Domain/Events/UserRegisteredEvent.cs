namespace Auth.Domain.Events;

public class UserRegisteredEvent
{
    public UserRegisteredEvent(Guid userId, string email, string nome, DateTime dataNascimento, bool ehAdministrador, DateTime dataCadastro)
    {
        UserId = userId;
        Email = email;
        Nome = nome;
        DataNascimento = dataNascimento;
        EhAdministrador = ehAdministrador;
        DataCadastro = dataCadastro;
    }
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public bool EhAdministrador { get; set; }
    public DateTime DataCadastro { get; set; }


}