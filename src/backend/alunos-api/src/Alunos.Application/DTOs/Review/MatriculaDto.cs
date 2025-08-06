namespace Alunos.Application.DTOs.Review;

public class MatriculaDto
{
    public Guid Id { get; set; }

    public Guid AlunoId { get; set; }

    public string NomeAluno { get; set; } = string.Empty;

    public Guid CursoId { get; set; }

    public string NomeCurso { get; set; } = string.Empty;

    public DateTime DataMatricula { get; set; }

    public DateTime DataInicio { get; set; }

    public DateTime? DataTermino { get; set; }

    public string Status { get; set; } = string.Empty;

    public decimal ValorPago { get; set; }

    public string FormaPagamento { get; set; } = string.Empty;

    public decimal PercentualConclusao { get; set; }

    public decimal? NotaFinal { get; set; }

    public string Observacoes { get; set; } = string.Empty;

    public bool IsAtiva { get; set; }

    public int DuracaoCursoDias { get; set; }

    public bool EstaVencida { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public List<ProgressoDto> Progresso { get; set; } = new();

    public List<CertificadoDto> Certificados { get; set; } = new();
}
