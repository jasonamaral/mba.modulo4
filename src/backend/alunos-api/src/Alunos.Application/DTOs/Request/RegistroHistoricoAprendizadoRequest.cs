namespace Alunos.Application.DTOs.Request;
public class RegistroHistoricoAprendizadoRequest
{
    public Guid AlunoId { get; set; }
    public Guid MatriculaCursoId { get; set; }
    public Guid AulaId { get; set; }
    public string NomeAula { get; private set; }
    public byte DuracaoMinutos { get; private set; }
    public DateTime? DataTermino { get; set; }
}
