using System.ComponentModel.DataAnnotations;

namespace Alunos.Application.DTOs;

public class MatriculaAtualizarDto
{
    [Required(ErrorMessage = "Data de início é obrigatória")]
    public DateTime DataInicio { get; set; }

    [Required(ErrorMessage = "Valor pago é obrigatório")]
    [Range(0, 999999.99, ErrorMessage = "Valor deve estar entre 0 e 999.999,99")]
    public decimal ValorPago { get; set; }

    [StringLength(50, ErrorMessage = "Forma de pagamento deve ter no máximo 50 caracteres")]
    public string FormaPagamento { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres")]
    public string Observacoes { get; set; } = string.Empty;
}
