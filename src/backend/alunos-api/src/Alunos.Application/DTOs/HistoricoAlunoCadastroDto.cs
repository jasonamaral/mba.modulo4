using System.ComponentModel.DataAnnotations;

namespace Alunos.Application.DTOs;

public class HistoricoAlunoCadastroDto
{
    [Required(ErrorMessage = "ID do aluno é obrigatório")]
    public Guid AlunoId { get; set; }

    [Required(ErrorMessage = "Ação é obrigatória")]
    [StringLength(100, ErrorMessage = "Ação deve ter no máximo 100 caracteres")]
    public string Acao { get; set; } = string.Empty;

    [Required(ErrorMessage = "Descrição é obrigatória")]
    [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tipo da ação é obrigatório")]
    public string TipoAcao { get; set; } = string.Empty;

    public string DetalhesJson { get; set; } = string.Empty;

    public Guid? UsuarioId { get; set; }

    [StringLength(50, ErrorMessage = "Endereço IP deve ter no máximo 50 caracteres")]
    public string EnderecoIP { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "User Agent deve ter no máximo 500 caracteres")]
    public string UserAgent { get; set; } = string.Empty;
}