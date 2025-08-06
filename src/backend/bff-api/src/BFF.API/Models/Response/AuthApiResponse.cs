namespace BFF.API.Models.Response;

public class AuthLoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public Guid RefreshToken { get; set; }
    public double ExpiresIn { get; set; }
    public AuthUserToken UsuarioToken { get; set; } = new();
}

public class AuthUserToken
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public IEnumerable<AuthUserClaim> Claims { get; set; } = new List<AuthUserClaim>();
}

public class AuthUserClaim
{
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class AuthRegistroResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public Guid RefreshToken { get; set; }
    public double ExpiresIn { get; set; }
    public AuthUserToken UsuarioToken { get; set; } = new();
}

public class AuthRefreshTokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public Guid RefreshToken { get; set; }
    public double ExpiresIn { get; set; }
    public AuthUserToken UsuarioToken { get; set; } = new();
}