namespace Auth.Application.DTOs;

public class UsuarioRespostaLoginDto
{
    public string AccessToken { get; set; }
    public Guid RefreshToken { get; set; }
    public double ExpiresIn { get; set; }
    public UsuarioTokenDto UsuarioToken { get; set; }
}