namespace Alunos.Application.DTOs;

public class AlunoPerfilDto
{
    public AlunoDto Aluno { get; set; } = new();

    public AlunoEstatisticasDto Estatisticas { get; set; } = new();

    public List<HistoricoAlunoDto> HistoricoRecente { get; set; } = new();
}