using System.ComponentModel.DataAnnotations;

namespace BFF.API.Models.Request;

public class LoginRequest
{

    [Required(ErrorMessage = "Email � obrigat�rio")]
    [EmailAddress(ErrorMessage = "Email inv�lido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha � obrigat�ria")]
    public string Senha { get; set; } = string.Empty;
}
