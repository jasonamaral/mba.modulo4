using System.ComponentModel.DataAnnotations;

namespace BFF.API.Models.Request;

public class RegistroRequest
{
    /// <summary>
    /// Nome do usuário
    /// </summary>
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Email do usuário
    /// </summary>
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Senha do usuário
    /// </summary>
    [Required(ErrorMessage = "Senha é obrigatória")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Senha deve ter entre 6 e 100 caracteres")]
    public string Senha { get; set; } = string.Empty;

    /// <summary>
    /// Data de nascimento do usuário
    /// </summary>
    [Required(ErrorMessage = "Data de nascimento é obrigatória")]
    public DateTime DataNascimento { get; set; }

    /// <summary>
    /// Indica se o usuário é administrador
    /// </summary>
    public bool EhAdministrador { get; set; } = false;
}
