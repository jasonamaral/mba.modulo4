using System.ComponentModel.DataAnnotations;

namespace BFF.Domain.DTOs.Alunos.Request;
public class SolicitaCertificadoRequest
{
    [Required(ErrorMessage = "ID do aluno é obrigatório")]
    public Guid AlunoId { get; private set; }

    [Required(ErrorMessage = "ID da matrícula do curso é obrigatório")]
    public Guid MatriculaCursoId { get; private set; }
}
