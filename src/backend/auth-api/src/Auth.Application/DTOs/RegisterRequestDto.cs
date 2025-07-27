namespace Auth.Application.DTOs;

public class RegisterRequestDto
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Nome { get; set; } = string.Empty;

    public DateTime DataNascimento { get; set; }

    public string CPF { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    public string Genero { get; set; } = string.Empty;

    public string Cidade { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public string CEP { get; set; } = string.Empty;

    public string? Foto { get; set; }

    public bool EhAdministrador { get; set; } = false;
}