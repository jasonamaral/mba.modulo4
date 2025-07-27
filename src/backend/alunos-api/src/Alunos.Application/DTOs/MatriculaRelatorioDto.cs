namespace Alunos.Application.DTOs;

public class MatriculaRelatorioDto
{
    public MatriculaDto Matricula { get; set; } = new();

    public CursoInfoDto CursoInfo { get; set; } = new();

    public MatriculaEstatisticasDto Estatisticas { get; set; } = new();

    public List<HistoricoAlunoDto> HistoricoAtividades { get; set; } = new();
}
