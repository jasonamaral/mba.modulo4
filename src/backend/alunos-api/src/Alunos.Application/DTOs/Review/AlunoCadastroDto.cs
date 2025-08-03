using System.ComponentModel.DataAnnotations;

namespace Alunos.Application.DTOs.Review;

public class AlunoCadastroDto
{
    [Required(ErrorMessage = "Código do usuário de autenticação é obrigatório")]
    public Guid CodigoUsuarioAutenticacao { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
    [StringLength(200, ErrorMessage = "Email deve ter no máximo 200 caracteres")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "CPF é obrigatório")]
    [StringLength(14, MinimumLength = 11, ErrorMessage = "CPF deve ter entre 11 e 14 caracteres")]
    public string CPF { get; set; } = string.Empty;

    [Required(ErrorMessage = "Data de nascimento é obrigatória")]
    public DateTime DataNascimento { get; set; }

    [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
    public string Telefone { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "Gênero deve ter no máximo 20 caracteres")]
    public string Genero { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Cidade deve ter no máximo 100 caracteres")]
    public string Cidade { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Estado deve ter no máximo 50 caracteres")]
    public string Estado { get; set; } = string.Empty;

    [StringLength(10, ErrorMessage = "CEP deve ter no máximo 10 caracteres")]
    public string CEP { get; set; } = string.Empty;
}