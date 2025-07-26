using System.ComponentModel.DataAnnotations;

namespace Auth.API.Models.Requests;

public class RefreshTokenRequest
{
    [Required(ErrorMessage = "Token de refresh é obrigatório")]
    public string RefreshToken { get; set; } = string.Empty;
} 