using System.ComponentModel.DataAnnotations;

namespace Auth.Application.DTOs;

public class LoginRequestDto
{
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
    [StringLength(25, MinimumLength = 8, ErrorMessage = "Senha deve ter entre 8 e 25 caracteres")]
    public string Password { get; set; } = string.Empty;
} 