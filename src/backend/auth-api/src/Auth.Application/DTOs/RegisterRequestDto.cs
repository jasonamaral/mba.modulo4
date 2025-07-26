using System.ComponentModel.DataAnnotations;

namespace Auth.Application.DTOs;

public class RegisterRequestDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
    [StringLength(25, MinimumLength = 8, ErrorMessage = "Senha deve ter entre 8 e 25 caracteres")]
    public string Password { get; set; } = string.Empty;

    [Compare("Password", ErrorMessage = "As senhas não conferem")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Data de nascimento é obrigatória")]
    public DateTime DataNascimento { get; set; }

    public bool EhAdministrador { get; set; } = false;
} 