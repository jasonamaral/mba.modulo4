namespace Auth.Application.DTOs;

public class AuthResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid? UserId { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public List<string>? Errors { get; set; }
} 