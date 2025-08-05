namespace Alunos.Application.DTOs.Review;

public class MatriculaResumoDto
{
    public Guid Id { get; set; }

    public string NomeAluno { get; set; } = string.Empty;

    public string NomeCurso { get; set; } = string.Empty;

    public DateTime DataMatricula { get; set; }

    public string Status { get; set; } = string.Empty;

    public decimal PercentualConclusao { get; set; }

    public decimal ValorPago { get; set; }

    public bool IsAtiva { get; set; }

    public bool EstaVencida { get; set; }
}
