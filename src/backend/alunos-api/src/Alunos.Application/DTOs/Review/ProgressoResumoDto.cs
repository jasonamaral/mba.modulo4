namespace Alunos.Application.DTOs.Review;

public class ProgressoResumoDto
{
    public Guid Id { get; set; }

    public Guid AulaId { get; set; }

    public string NomeAula { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public decimal PercentualAssistido { get; set; }

    public int TempoAssistidoMinutos { get; set; }

    public DateTime? UltimoAcesso { get; set; }

    public decimal? Nota { get; set; }

    public bool EstaAbandonada { get; set; }
}
