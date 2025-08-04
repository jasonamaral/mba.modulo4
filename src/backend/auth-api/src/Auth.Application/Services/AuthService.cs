using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Auth.Application.Settings;
using Auth.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auth.Application.Services;

public class AuthService
{
    public readonly UserManager<ApplicationUser> UserManager;
    public readonly SignInManager<ApplicationUser> SignInManager;
    private readonly IAuthDbContext _context;
    private readonly ILogger<AuthService> _logger;
    private readonly JwtSettings _jwtSettings;
    private readonly IEventPublisher _eventPublisher;
    private readonly IJwtService _jwtService;
    private readonly IHttpContextAccessor _accessor;
    private readonly AppTokenSettings _appTokenSettingsSettings;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IAuthDbContext context,
        ILogger<AuthService> logger,
        IOptions<JwtSettings> jwtSettings,
        IEventPublisher eventPublisher,
        IJwtService jwksService,
        IHttpContextAccessor accessor,
        IOptions<AppTokenSettings> appTokenSettingsSettings)
    {
        UserManager = userManager;
        SignInManager = signInManager;
        _context = context;
        _logger = logger;
        _jwtSettings = jwtSettings.Value;
        _eventPublisher = eventPublisher;
        _jwtService = jwksService;
        _accessor = accessor;
        _appTokenSettingsSettings = appTokenSettingsSettings.Value;
    }

    public async Task<UsuarioRespostaLoginDto> GerarJwt(string email)
    {
        var user = await UserManager.FindByEmailAsync(email);
        var claims = await UserManager.GetClaimsAsync(user!);

        var identityClaims = await ObterClaimsUsuario(claims, user!);
        var encodedToken = await CodificarTokenAsync(identityClaims);

        var refreshToken = await GerarRefreshToken(email);

        return ObterRespostaToken(encodedToken, user!, claims, refreshToken);
    }

    public async Task<RefreshToken?> ObterRefreshToken(Guid refreshToken)
    {
        var token = await _context.RefreshTokens.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Token == refreshToken);

        return token != null && token.ExpirationDate.ToLocalTime() > DateTime.Now ? token : null;
    }

    private async Task<ClaimsIdentity> ObterClaimsUsuario(ICollection<Claim> claims, ApplicationUser user)
    {
        var userRoles = await UserManager.GetRolesAsync(user);

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email!));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

        foreach (var userRole in userRoles)
        {
            claims.Add(new Claim("role", userRole));
        }

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        return identityClaims;
    }

    private async Task<string> CodificarTokenAsync(ClaimsIdentity identityClaims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var currentIssuer =
            $"{_accessor.HttpContext!.Request.Scheme}://{_accessor.HttpContext!.Request.Host}";
        var key = await _jwtService.GetCurrentSigningCredentials();
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = currentIssuer,
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = key
        });

        return tokenHandler.WriteToken(token);
    }

    private UsuarioRespostaLoginDto ObterRespostaToken(string encodedToken, IdentityUser user, IEnumerable<Claim> claims, RefreshToken refreshToken)
    {
        return new UsuarioRespostaLoginDto
        {
            AccessToken = encodedToken,
            RefreshToken = refreshToken.Token,
            ExpiresIn = TimeSpan.FromHours(1).TotalSeconds,
            UsuarioToken = new UsuarioTokenDto
            {
                Id = user.Id,
                Email = user.Email!,
                Claims = claims.Select(c => new UsuarioClaimDto { Type = c.Type, Value = c.Value })
            }
        };
    }

    private static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
            .TotalSeconds);

    private async Task<RefreshToken> GerarRefreshToken(string email)
    {
        var refreshToken = new RefreshToken
        {
            Username = email,
            ExpirationDate = DateTime.UtcNow.AddHours(_appTokenSettingsSettings.RefreshTokenExpiration)
        };

        _context.RefreshTokens.RemoveRange(_context.RefreshTokens.Where(u => u.Username == email));
        await _context.RefreshTokens.AddAsync(refreshToken);

        await _context.SaveChangesAsync();

        return refreshToken;
    }
}