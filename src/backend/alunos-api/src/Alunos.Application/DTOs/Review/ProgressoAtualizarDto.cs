using System.ComponentModel.DataAnnotations;

namespace Alunos.Application.DTOs.Review;

public class ProgressoAtualizarDto
{
    [Required(ErrorMessage = "Percentual assistido é obrigatório")]
    [Range(0, 100, ErrorMessage = "Percentual deve estar entre 0 e 100")]
    public decimal PercentualAssistido { get; set; }

    [Required(ErrorMessage = "Tempo assistido é obrigatório")]
    [Range(0, 86400, ErrorMessage = "Tempo assistido deve estar entre 0 e 86400 segundos")]
    public int TempoAssistido { get; set; }

    [StringLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres")]
    public string Observacoes { get; set; } = string.Empty;
}
