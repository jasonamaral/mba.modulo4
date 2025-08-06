using System.ComponentModel.DataAnnotations;

namespace Alunos.Application.DTOs.Review;

public class MatriculaConclusaoDto
{
    [Range(0, 10, ErrorMessage = "Nota deve estar entre 0 e 10")]
    public decimal? NotaFinal { get; set; }

    [StringLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres")]
    public string Observacoes { get; set; } = string.Empty;
}
