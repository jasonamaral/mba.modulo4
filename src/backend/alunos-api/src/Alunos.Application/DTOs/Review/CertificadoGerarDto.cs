using System.ComponentModel.DataAnnotations;

namespace Alunos.Application.DTOs.Review;

public class CertificadoGerarDto
{
    [Required(ErrorMessage = "ID da matrícula é obrigatório")]
    public Guid MatriculaId { get; set; }

    [Required(ErrorMessage = "Nome do curso é obrigatório")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Nome do curso deve ter entre 2 e 200 caracteres")]
    public string NomeCurso { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nome do aluno é obrigatório")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome do aluno deve ter entre 2 e 100 caracteres")]
    public string NomeAluno { get; set; } = string.Empty;

    [Required(ErrorMessage = "Carga horária é obrigatória")]
    [Range(1, 10000, ErrorMessage = "Carga horária deve estar entre 1 e 10000 horas")]
    public int CargaHoraria { get; set; }

    [Range(0, 10, ErrorMessage = "Nota deve estar entre 0 e 10")]
    public decimal? NotaFinal { get; set; }

    [StringLength(100, ErrorMessage = "Nome do instrutor deve ter no máximo 100 caracteres")]
    public string NomeInstrutor { get; set; } = string.Empty;

    [Range(1, 36500, ErrorMessage = "Dias de validade deve estar entre 1 e 36500")]
    public int ValidadeDias { get; set; } = 3650; // 10 anos por padrão
}