namespace Alunos.Application.DTOs.Review;

public class MatriculaEstatisticasDto
{
    public int TotalAulas { get; set; }

    public int AulasAssistidas { get; set; }

    public int AulasConcluidas { get; set; }

    public decimal TempoEstudoHoras { get; set; }

    public decimal MediaTempoPorAula { get; set; }

    public decimal PercentualConclusao { get; set; }

    public DateTime? PrimeiraAulaAssistida { get; set; }

    public DateTime? UltimaAtividade { get; set; }
}
