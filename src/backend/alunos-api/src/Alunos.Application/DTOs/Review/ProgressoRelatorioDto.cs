namespace Alunos.Application.DTOs.Review;

public class ProgressoRelatorioDto
{
    public Guid MatriculaId { get; set; }

    public string NomeCurso { get; set; } = string.Empty;

    public string NomeAluno { get; set; } = string.Empty;

    public int TotalAulas { get; set; }

    public int AulasIniciadas { get; set; }

    public int AulasConcluidas { get; set; }

    public decimal PercentualConclusaoGeral { get; set; }

    public decimal TempoTotalEstudoHoras { get; set; }

    public decimal MediaTempoPorAula { get; set; }

    public decimal? NotaMedia { get; set; }

    public DateTime? PrimeiraAulaAssistida { get; set; }

    public DateTime? UltimaAtividade { get; set; }

    public List<ProgressoDto> ProgressoPorAula { get; set; } = new();
}