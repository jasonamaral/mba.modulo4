using Core.Messages;

namespace Alunos.Application.Commands.AtualizarPagamento;

public class AtualizarPagamentoMatriculaCommand : CommandRaiz
{
    public Guid AlunoId { get; init; }
    public Guid CursoId { get; init; }

    public AtualizarPagamentoMatriculaCommand(Guid alunoId, Guid cursoId)
    {
        DefinirRaizAgregacao(alunoId);

        AlunoId = alunoId;
        CursoId = cursoId;
    }
}
