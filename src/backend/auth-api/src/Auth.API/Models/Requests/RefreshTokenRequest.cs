using System.ComponentModel.DataAnnotations;

namespace Auth.API.Models.Requests;

public class RefreshTokenRequest
{
    [Required(ErrorMessage = "Token de refresh é obrigatório")]
    public Guid RefreshToken { get; set; }
}