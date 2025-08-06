using System.ComponentModel.DataAnnotations;

namespace Alunos.Application.DTOs.Review;

public class CertificadoValidacaoDto
{
    [Required(ErrorMessage = "Código do certificado é obrigatório")]
    [StringLength(100, ErrorMessage = "Código deve ter no máximo 100 caracteres")]
    public string Codigo { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Hash deve ter no máximo 500 caracteres")]
    public string Hash { get; set; } = string.Empty;
}