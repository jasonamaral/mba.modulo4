using System.ComponentModel.DataAnnotations;

namespace BFF.Domain.DTOs.Alunos.Request;
public class ConcluirCursoApiRequest
{
    [Required(ErrorMessage = "ID do aluno é obrigatório")]
    public Guid AlunoId { get; set; }
    
    [Required(ErrorMessage = "ID da matrícula no curso é obrigatório")]
    public Guid MatriculaCursoId { get; set; }
    
    [Required(ErrorMessage = "Informações do curso é obrigatório")]
    public CursoDto CursoDto { get; set; }
}
