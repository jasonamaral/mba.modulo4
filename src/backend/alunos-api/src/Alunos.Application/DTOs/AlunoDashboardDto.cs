namespace Alunos.Application.DTOs;

public class AlunoDashboardDto
{
    public AlunoResumoDto Aluno { get; set; } = new();

    public AlunoEstatisticasDto Estatisticas { get; set; } = new();

    public List<MatriculaDto> MatriculasEmAndamento { get; set; } = new();

    public List<ProximaAulaDto> ProximasAulas { get; set; } = new();

    public List<CertificadoDto> CertificadosRecentes { get; set; } = new();

    public List<HistoricoAlunoDto> AtividadesRecentes { get; set; } = new();
}