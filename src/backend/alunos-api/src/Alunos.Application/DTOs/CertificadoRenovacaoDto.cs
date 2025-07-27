using System.ComponentModel.DataAnnotations;

namespace Alunos.Application.DTOs;

public class CertificadoRenovacaoDto
{
    [Required(ErrorMessage = "Dias de validade são obrigatórios")]
    [Range(1, 36500, ErrorMessage = "Dias de validade deve estar entre 1 e 36500")]
    public int NovosDias { get; set; }
}