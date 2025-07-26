using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Auth.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Auth.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly AuthDbContext _context;
    private readonly ILogger<AuthService> _logger;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        AuthDbContext context,
        ILogger<AuthService> logger,
        IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _logger = logger;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        try
        {
            // Verificar se o usuário já existe
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Usuário já existe com este email",
                    Errors = ["Email já está em uso"]
                };
            }

            // Criar novo usuário
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                Nome = request.Nome,
                DataNascimento = request.DataNascimento,
                DataCadastro = DateTime.UtcNow,
                Ativo = true,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Erro ao criar usuário",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            // Adicionar role
            var roleName = request.EhAdministrador ? "Administrador" : "Usuario";
            await _userManager.AddToRoleAsync(user, roleName);

            // Gerar tokens
            var accessToken = await GenerateJwtTokenAsync(user);
            var refreshToken = GenerateRefreshToken();

            // Salvar refresh token
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
            await _userManager.UpdateAsync(user);

            _logger.LogInformation("Usuário registrado com sucesso: {Email}", user.Email ?? "N/A");

            return new AuthResponseDto
            {
                Success = true,
                Message = "Usuário registrado com sucesso",
                UserId = Guid.Parse(user.Id),
                Nome = user.Nome,
                Email = user.Email ?? string.Empty,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar usuário: {Email}", request.Email);
            return new AuthResponseDto
            {
                Success = false,
                Message = "Erro interno do servidor",
                Errors = ["Erro interno do servidor"]
            };
        }
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !user.Ativo)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Credenciais inválidas",
                    Errors = ["Email ou senha incorretos"]
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Credenciais inválidas",
                    Errors = ["Email ou senha incorretos"]
                };
            }

            // Gerar tokens
            var accessToken = await GenerateJwtTokenAsync(user);
            var refreshToken = GenerateRefreshToken();

            // Salvar refresh token
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
            await _userManager.UpdateAsync(user);

            _logger.LogInformation("Usuário logado com sucesso: {Email}", user.Email ?? "N/A");

            return new AuthResponseDto
            {
                Success = true,
                Message = "Login realizado com sucesso",
                UserId = Guid.Parse(user.Id),
                Nome = user.Nome,
                Email = user.Email ?? string.Empty,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer login: {Email}", request.Email);
            return new AuthResponseDto
            {
                Success = false,
                Message = "Erro interno do servidor",
                Errors = ["Erro interno do servidor"]
            };
        }
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);

            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Token de refresh inválido ou expirado",
                    Errors = ["Token de refresh inválido"]
                };
            }

            // Gerar novos tokens
            var accessToken = await GenerateJwtTokenAsync(user);
            var refreshToken = GenerateRefreshToken();

            // Salvar novo refresh token
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
            await _userManager.UpdateAsync(user);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Token renovado com sucesso",
                UserId = Guid.Parse(user.Id),
                Nome = user.Nome,
                Email = user.Email ?? string.Empty,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao renovar token");
            return new AuthResponseDto
            {
                Success = false,
                Message = "Erro interno do servidor",
                Errors = ["Erro interno do servidor"]
            };
        }
    }

    public async Task<bool> LogoutAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                await _userManager.UpdateAsync(user);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer logout: {UserId}", userId);
            return false;
        }
    }

    public Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new("nome", user.Nome),
            new("dataNascimento", user.DataNascimento.ToString("yyyy-MM-dd"))
        };

        // Adicionar claims de role
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
            
            // Claim específico para administrador
            if (role == "Administrador")
            {
                claims.Add(new Claim("level", "Admin"));
            }
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

        return tokenHandler.WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
} 