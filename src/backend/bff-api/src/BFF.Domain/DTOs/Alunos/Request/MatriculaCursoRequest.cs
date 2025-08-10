using System.ComponentModel.DataAnnotations;

namespace BFF.Domain.DTOs.Alunos.Request;
public class MatriculaCursoRequest
{
    [Required(ErrorMessage = "ID do aluno é obrigatório")]
    public Guid AlunoId { get; set; }

    [Required(ErrorMessage = "ID do curso é obrigatório")]
    public Guid CursoId { get; set; }
    public string Observacao { get; init; }
}
