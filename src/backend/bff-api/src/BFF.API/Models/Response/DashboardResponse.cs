namespace BFF.API.Models.Response;

public class DashboardAlunoResponse
{
    public AlunoPerfilResponse Aluno { get; set; } = new();
    public List<MatriculaResponse> Matriculas { get; set; } = new();
    public List<CertificadoResponse> Certificados { get; set; } = new();
    public List<CursoRecomendadoResponse> CursosRecomendados { get; set; } = new();
    public List<PagamentoResponse> PagamentosRecentes { get; set; } = new();
    public ProgressoGeralResponse ProgressoGeral { get; set; } = new();
}

public class DashboardAdminResponse
{
    public EstatisticasGeraisResponse EstatisticasGerais { get; set; } = new();
    public List<CursoPopularResponse> CursosPopulares { get; set; } = new();
    public List<AlunoRecenteResponse> AlunosRecentes { get; set; } = new();
    public List<MatriculaRecenteResponse> MatriculasRecentes { get; set; } = new();
    public List<PagamentoRecenteResponse> PagamentosRecentes { get; set; } = new();
}

public class CertificadoResponse
{
    public Guid Id { get; set; }
    public string NomeCurso { get; set; } = string.Empty;
    public DateTime DataConclusao { get; set; }
    public string CodigoValidacao { get; set; } = string.Empty;
    public string UrlDownload { get; set; } = string.Empty;
}

public class CursoRecomendadoResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public int DuracaoHoras { get; set; }
    public string Nivel { get; set; } = string.Empty;
    public string ImagemUrl { get; set; } = string.Empty;
}

public class PagamentoResponse
{
    public Guid Id { get; set; }
    public string CursoNome { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime DataPagamento { get; set; }
    public string FormaPagamento { get; set; } = string.Empty;
}

public class ProgressoGeralResponse
{
    public int TotalCursos { get; set; }
    public int CursosConcluidos { get; set; }
    public int CursosEmAndamento { get; set; }
    public decimal PercentualConclusaoGeral { get; set; }
    public int HorasEstudadas { get; set; }
    public int CertificadosObtidos { get; set; }
}

public class EstatisticasGeraisResponse
{
    public int TotalAlunos { get; set; }
    public int TotalCursos { get; set; }
    public int TotalMatriculas { get; set; }
    public decimal ReceitaTotal { get; set; }
    public int AlunosAtivos { get; set; }
    public int CursosAtivos { get; set; }
}

public class CursoPopularResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int TotalMatriculas { get; set; }
    public decimal ReceitaGerada { get; set; }
    public decimal AvaliacaoMedia { get; set; }
}

public class AlunoRecenteResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; }
    public int TotalMatriculas { get; set; }
}

public class MatriculaRecenteResponse
{
    public Guid Id { get; set; }
    public string AlunoNome { get; set; } = string.Empty;
    public string CursoNome { get; set; } = string.Empty;
    public DateTime DataMatricula { get; set; }
    public decimal Valor { get; set; }
}

public class PagamentoRecenteResponse
{
    public Guid Id { get; set; }
    public string AlunoNome { get; set; } = string.Empty;
    public string CursoNome { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime DataPagamento { get; set; }
}