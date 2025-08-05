namespace Alunos.Application.DTOs.Review;

public class AlunoDto
{
    public Guid Id { get; set; }

    public Guid CodigoUsuarioAutenticacao { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string CPF { get; set; } = string.Empty;

    public DateTime DataNascimento { get; set; }

    public int Idade { get; set; }

    public string Telefone { get; set; } = string.Empty;

    public string Genero { get; set; } = string.Empty;

    public string Cidade { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public string CEP { get; set; } = string.Empty;

    public bool IsAtivo { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public List<MatriculaDto> Matriculas { get; set; } = new();
}