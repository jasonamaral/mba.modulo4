namespace Alunos.Application.DTOs;

public class ProgressoDto
{
    public Guid Id { get; set; }

    public Guid MatriculaId { get; set; }

    public Guid AulaId { get; set; }

    public string NomeAula { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public decimal PercentualAssistido { get; set; }

    public int TempoAssistido { get; set; }

    public int TempoAssistidoMinutos { get; set; }

    public decimal TempoAssistidoHoras { get; set; }

    public DateTime? DataInicio { get; set; }

    public DateTime? DataConclusao { get; set; }

    public DateTime? UltimoAcesso { get; set; }

    public decimal? Nota { get; set; }

    public string Observacoes { get; set; } = string.Empty;

    public bool EstaAbandonada { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
