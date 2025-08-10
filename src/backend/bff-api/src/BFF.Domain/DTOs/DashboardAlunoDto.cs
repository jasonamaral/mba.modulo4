using BFF.Domain.DTOs.Alunos.Response;

namespace BFF.Domain.DTOs;

public class DashboardAlunoDto
{
    public AlunoDto Aluno { get; set; } = new();
    public List<MatriculaCursoDto> Matriculas { get; set; } = new();
    public List<CertificadoDto> Certificados { get; set; } = new();
    public List<CursoDto> CursosRecomendados { get; set; } = new();
    public List<PagamentoDto> PagamentosRecentes { get; set; } = new();
    public ProgressoGeralDto ProgressoGeral { get; set; } = new();
}
