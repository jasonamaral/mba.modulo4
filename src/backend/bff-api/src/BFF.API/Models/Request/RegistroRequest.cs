using System.ComponentModel.DataAnnotations;

namespace BFF.API.Models.Request;

public class RegistroRequest
{
    /// <summary>
    /// Nome do usu�rio
    /// </summary>
    [Required(ErrorMessage = "Nome � obrigat�rio")]
    [StringLength(100, ErrorMessage = "Nome deve ter no m�ximo 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Email do usu�rio
    /// </summary>
    [Required(ErrorMessage = "Email � obrigat�rio")]
    [EmailAddress(ErrorMessage = "Email inv�lido")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Senha do usu�rio
    /// </summary>
    [Required(ErrorMessage = "Senha � obrigat�ria")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Senha deve ter entre 6 e 100 caracteres")]
    public string Senha { get; set; } = string.Empty;

    /// <summary>
    /// Data de nascimento do usu�rio
    /// </summary>
    [Required(ErrorMessage = "Data de nascimento � obrigat�ria")]
    public DateTime DataNascimento { get; set; }

    /// <summary>
    /// Indica se o usu�rio � administrador
    /// </summary>
    public bool EhAdministrador { get; set; } = false;
}
