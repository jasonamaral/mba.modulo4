namespace Auth.Application.DTOs;

public class UsuarioTokenDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public IEnumerable<UsuarioClaimDto> Claims { get; set; }
}