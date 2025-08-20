using Core.Messages;

namespace Alunos.Application.Commands.AtualizarPagamento;
public class AtualizarPagamentoMatriculaCommand : CommandRaiz
{
    public Guid AlunoId { get; init; }
    public Guid MatriculaCursoId { get; init; }

    public AtualizarPagamentoMatriculaCommand(Guid alunoId, Guid matriculaCursoId) 
    {
        DefinirRaizAgregacao(alunoId);

        AlunoId = alunoId;
        MatriculaCursoId = matriculaCursoId;
    }
}
