namespace BFF.Domain.DTOs;

public class AlunoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Foto { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
