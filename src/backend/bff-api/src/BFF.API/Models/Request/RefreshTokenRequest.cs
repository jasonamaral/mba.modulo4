using System.ComponentModel.DataAnnotations;

namespace BFF.API.Models.Request;

public class RefreshTokenRequest
{
    [Required(ErrorMessage = "Token de refresh � obrigat�rio")]
    public string RefreshToken { get; set; } = string.Empty;
}
