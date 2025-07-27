using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Auth.Application.Settings;
using Auth.Domain.Entities;
using Auth.Domain.Events;
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
    private readonly IAuthDbContext _context;
    private readonly ILogger<AuthService> _logger;
    private readonly JwtSettings _jwtSettings;
    private readonly IEventPublisher _eventPublisher;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IAuthDbContext context,
        ILogger<AuthService> logger,
        IOptions<JwtSettings> jwtSettings,
        IEventPublisher eventPublisher)
    {
        _userManager = userManager;
        _context = context;
        _logger = logger;
        _jwtSettings = jwtSettings.Value;
        _eventPublisher = eventPublisher;
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

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                Nome = request.Nome,
                DataNascimento = request.DataNascimento,
                CPF = request.CPF,
                Telefone = request.Telefone,
                Genero = request.Genero,
                Cidade = request.Cidade,
                Estado = request.Estado,
                CEP = request.CEP,
                Foto = request.Foto,
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

            // Publicar evento para criar perfil do aluno (se não for administrador)
            if (!request.EhAdministrador)
            {
                var userRegisteredEvent = new UserRegisteredEvent
                {
                    UserId = user.Id,
                    Email = user.Email ?? "",
                    Nome = user.Nome,
                    DataNascimento = user.DataNascimento,
                    CPF = user.CPF,
                    Telefone = user.Telefone,
                    Genero = user.Genero,
                    Cidade = user.Cidade,
                    Estado = user.Estado,
                    CEP = user.CEP,
                    Foto = user.Foto,
                    DataCadastro = user.DataCadastro,
                    EhAdministrador = request.EhAdministrador
                };
                
                await _eventPublisher.PublishAsync(userRegisteredEvent);
            }

            // Gerar tokens
            var accessToken = await GenerateJwtTokenAsync(user);
            var refreshToken = GenerateRefreshToken();

            // Salvar refresh token
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
            await _userManager.UpdateAsync(user);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Usuário registrado com sucesso",
                Token = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email ?? "",
                    Nome = user.Nome,
                    DataNascimento = user.DataNascimento,
                    CPF = user.CPF,
                    Telefone = user.Telefone,
                    Genero = user.Genero,
                    Cidade = user.Cidade,
                    Estado = user.Estado,
                    CEP = user.CEP,
                    Foto = user.Foto,
                    DataCadastro = user.DataCadastro,
                    Ativo = user.Ativo,
                    Roles = [roleName]
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante o registro do usuário");
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
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Senha))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Credenciais inválidas",
                    Errors = ["Email ou senha incorretos"]
                };
            }

            if (!user.Ativo)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Usuário inativo",
                    Errors = ["Conta desativada"]
                };
            }

            // Gerar tokens
            var accessToken = await GenerateJwtTokenAsync(user);
            var refreshToken = GenerateRefreshToken();

            // Salvar refresh token
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
            await _userManager.UpdateAsync(user);

            // Obter roles do usuário
            var roles = await _userManager.GetRolesAsync(user);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Login realizado com sucesso",
                Token = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email ?? "",
                    Nome = user.Nome,
                    DataNascimento = user.DataNascimento,
                    CPF = user.CPF,
                    Telefone = user.Telefone,
                    Genero = user.Genero,
                    Cidade = user.Cidade,
                    Estado = user.Estado,
                    CEP = user.CEP,
                    Foto = user.Foto,
                    DataCadastro = user.DataCadastro,
                    Ativo = user.Ativo,
                    Roles = roles.ToList()
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante o login");
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
                    Message = "Refresh token inválido ou expirado",
                    Errors = ["Token inválido"]
                };
            }

            // Gerar novos tokens
            var accessToken = await GenerateJwtTokenAsync(user);
            var refreshToken = GenerateRefreshToken();

            // Atualizar refresh token
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
            await _userManager.UpdateAsync(user);

            // Obter roles do usuário
            var roles = await _userManager.GetRolesAsync(user);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Token renovado com sucesso",
                Token = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email ?? "",
                    Nome = user.Nome,
                    DataNascimento = user.DataNascimento,
                    CPF = user.CPF,
                    Telefone = user.Telefone,
                    Genero = user.Genero,
                    Cidade = user.Cidade,
                    Estado = user.Estado,
                    CEP = user.CEP,
                    Foto = user.Foto,
                    DataCadastro = user.DataCadastro,
                    Ativo = user.Ativo,
                    Roles = roles.ToList()
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante refresh token");
            return new AuthResponseDto
            {
                Success = false,
                Message = "Erro interno do servidor",
                Errors = ["Erro interno do servidor"]
            };
        }
    }

    private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email ?? ""),
            new(ClaimTypes.Name, user.Nome),
            new("userId", user.Id)
        };

        // Adicionar roles como claims
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
} 