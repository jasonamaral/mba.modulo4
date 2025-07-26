namespace Auth.Domain.Events;

public class UserRegisteredEvent
{
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public DateTime DataCadastro { get; set; }
    public bool EhAdministrador { get; set; }
} 