using System.ComponentModel.DataAnnotations;

namespace Alunos.Application.DTOs;

public class MatriculaCancelamentoDto
{
    [Required(ErrorMessage = "Motivo do cancelamento é obrigatório")]
    [StringLength(200, ErrorMessage = "Motivo deve ter no máximo 200 caracteres")]
    public string Motivo { get; set; } = string.Empty;
}
