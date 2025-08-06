using Core.Messages;

namespace Alunos.Application.Commands.SolicitarCertificado;
public class SolicitarCertificadoCommand : CommandRaiz
{
    public Guid AlunoId { get; private set; }
    public Guid MatriculaCursoId { get; private set; }
    public string PathCertificado { get; private set; }
    public byte NotaFinal { get; private set; }
    public string NomeInstrutor { get; private set; }

    public SolicitarCertificadoCommand(Guid alunoId, Guid matriculaCursoId, string pathCertificado, byte notaFinal, string nomeInstrutor)
    {
        DefinirRaizAgregacao(alunoId);

        AlunoId = alunoId;
        MatriculaCursoId = matriculaCursoId;
        PathCertificado = pathCertificado;
        NotaFinal = notaFinal;
        NomeInstrutor = nomeInstrutor;
    }
}
