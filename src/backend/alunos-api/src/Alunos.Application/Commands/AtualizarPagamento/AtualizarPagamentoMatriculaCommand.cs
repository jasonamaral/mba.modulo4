using Core.Messages;

namespace Alunos.Application.Commands.AtualizarPagamento;
public class AtualizarPagamentoMatriculaCommand : CommandRaiz
{
    public Guid AlunoId { get; init; }
    public Guid CursoId { get; init; }
    public bool CursoDisponivel { get; init; }

    public AtualizarPagamentoMatriculaCommand(Guid alunoId, Guid cursoId, bool cursoDisponivel)
    {
        DefinirRaizAgregacao(alunoId);

        AlunoId = alunoId;
        CursoId = cursoId;
        CursoDisponivel = cursoDisponivel;
    }
}
