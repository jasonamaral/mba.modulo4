using Core.Messages;
using Core.SharedDtos.Conteudo;

namespace Core.Mediator.AlunoCommands;
public class RegistrarHistoricoAprendizadoCommand : CommandRaiz
{
    public Guid AlunoId { get; private set; }
    public Guid MatriculaCursoId { get; private set; }
    public Guid AulaId { get; private set; }
    public DateTime? DataTermino { get; private set; }
    public CursoDto CursoDto { get; private set; }

    public RegistrarHistoricoAprendizadoCommand(Guid alunoId, Guid matriculaCursoId, Guid aulaId, CursoDto cursoDto, DateTime? dataTermino = null)
    {
        DefinirRaizAgregacao(alunoId);

        AlunoId = alunoId;
        MatriculaCursoId = matriculaCursoId;
        AulaId = aulaId;
        DataTermino = dataTermino;
        CursoDto = cursoDto;
    }
}