using System.ComponentModel.DataAnnotations;

namespace Alunos.Application.DTOs.Review;

public class CertificadoAtualizarDto
{
    [StringLength(500, ErrorMessage = "URL do arquivo deve ter no máximo 500 caracteres")]
    public string UrlArquivo { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres")]
    public string Observacoes { get; set; } = string.Empty;
}