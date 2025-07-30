namespace Alunos.Application.DTOs;

public class HistoricoAlunoRelatorioDto
{
    public Guid AlunoId { get; set; }

    public string NomeAluno { get; set; } = string.Empty;

    public string Periodo { get; set; } = string.Empty;

    public DateTime DataInicial { get; set; }

    public DateTime DataFinal { get; set; }

    public int TotalAcoes { get; set; }

    public List<HistoricoAlunoResumoTipoDto> ResumoPorTipo { get; set; } = new();

    public List<HistoricoAlunoDto> AcoesRecentes { get; set; } = new();

    public HistoricoAlunoEstatisticasDto Estatisticas { get; set; } = new();
}