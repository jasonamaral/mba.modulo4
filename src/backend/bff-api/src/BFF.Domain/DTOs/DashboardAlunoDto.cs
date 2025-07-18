namespace BFF.Domain.DTOs;

public class DashboardAlunoDto
{
    public AlunoDto Aluno { get; set; } = new();
    public List<MatriculaDto> Matriculas { get; set; } = new();
    public List<CertificadoDto> Certificados { get; set; } = new();
    public List<CursoDto> CursosRecomendados { get; set; } = new();
    public List<PagamentoDto> PagamentosRecentes { get; set; } = new();
    public ProgressoGeralDto ProgressoGeral { get; set; } = new();
}

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

public class MatriculaDto
{
    public Guid Id { get; set; }
    public Guid AlunoId { get; set; }
    public Guid CursoId { get; set; }
    public string CursoNome { get; set; } = string.Empty;
    public DateTime DataMatricula { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal PercentualConclusao { get; set; }
    public DateTime? DataConclusao { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CertificadoDto
{
    public Guid Id { get; set; }
    public Guid AlunoId { get; set; }
    public Guid CursoId { get; set; }
    public string CursoNome { get; set; } = string.Empty;
    public DateTime DataEmissao { get; set; }
    public string CodigoVerificacao { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CursoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int CargaHoraria { get; set; }
    public int TotalAulas { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ImagemCapa { get; set; } = string.Empty;
    public List<AulaDto> Aulas { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class AulaDto
{
    public Guid Id { get; set; }
    public Guid CursoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public int DuracaoMinutos { get; set; }
    public string VideoUrl { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class PagamentoDto
{
    public Guid Id { get; set; }
    public Guid AlunoId { get; set; }
    public Guid CursoId { get; set; }
    public string CursoNome { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public string Status { get; set; } = string.Empty;
    public string FormaPagamento { get; set; } = string.Empty;
    public DateTime DataPagamento { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class ProgressoGeralDto
{
    public int CursosMatriculados { get; set; }
    public int CursosConcluidos { get; set; }
    public int CertificadosEmitidos { get; set; }
    public decimal PercentualConcluidoGeral { get; set; }
    public int HorasEstudadas { get; set; }
} 