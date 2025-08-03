using System.ComponentModel.DataAnnotations;

namespace Alunos.Application.DTOs.Review;

public class MatriculaSuspensaoDto
{
    [Required(ErrorMessage = "Motivo da suspensão é obrigatório")]
    [StringLength(200, ErrorMessage = "Motivo deve ter no máximo 200 caracteres")]
    public string Motivo { get; set; } = string.Empty;
}
