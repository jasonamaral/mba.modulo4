using System.ComponentModel.DataAnnotations;

namespace Auth.Application.DTOs;

public class RefreshTokenRequestDto
{
    [Required(ErrorMessage = "Refresh token é obrigatório")]
    public string RefreshToken { get; set; } = string.Empty;
} 