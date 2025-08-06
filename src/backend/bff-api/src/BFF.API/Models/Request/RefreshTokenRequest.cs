namespace BFF.API.Models.Request;

/// <summary>
/// Request para renovação de token
/// </summary>
public class RefreshTokenRequest
{
    /// <summary>
    /// Token de refresh
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
} 