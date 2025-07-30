namespace Alunos.Application.DTOs;

public class AlunoResumoDto
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public int Idade { get; set; }

    public string Cidade { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public bool IsAtivo { get; set; }

    public int QuantidadeMatriculasAtivas { get; set; }

    public int QuantidadeCursosConcluidos { get; set; }

    public DateTime CreatedAt { get; set; }
}