using Microsoft.AspNetCore.Mvc;
using Auth.API.Models.Requests;
using Auth.API.Models.Responses;
using Auth.Application.Interfaces;
using Auth.Application.DTOs;

namespace Auth.API.Controllers;

/// <summary>
/// Controller de autenticação
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Registra um novo usuário no sistema
    /// </summary>
    /// <param name="request">Dados do usuário para registro</param>
    /// <returns>Confirmação de registro</returns>
    [HttpPost("registro")]
    [ProducesResponseType(typeof(AuthResponse), 200)]
    [ProducesResponseType(typeof(ApiError), 400)]
    public async Task<IActionResult> Registro([FromBody] RegistroRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiError
                {
                    Message = "Dados inválidos",
                    Details = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),
                    ErrorCode = "VALIDATION_ERROR"
                });
            }

            var registerDto = new RegisterRequestDto
            {
                Email = request.Email,
                Password = request.Senha,
                Nome = request.Nome,
                DataNascimento = request.DataNascimento,
                EhAdministrador = request.EhAdministrador,
                CPF = request.CPF,
                Telefone = request.Telefone,
                Genero = request.Genero,
                Cidade = request.Cidade,
                Estado = request.Estado,
                CEP = request.CEP,
                Foto = request.Foto
            };

            var result = await _authService.RegistrarAsync(registerDto);

            if (!result.Success)
            {
                return BadRequest(new ApiError
                {
                    Message = result.Message,
                    Details = result.Errors,
                    ErrorCode = "REGISTRATION_ERROR"
                });
            }

            var response = new AuthResponse
            {
                Success = true,
                Message = result.Message,
                AccessToken = result.Token,
                RefreshToken = result.RefreshToken,
                ExpiresAt = result.ExpiresAt,
                Endpoint = "registro"
            };

            if (result.User != null)
            {
                response.User = new UserInfo
                {
                    Id = result.User.Id,
                    Email = result.User.Email,
                    Nome = result.User.Nome,
                    Roles = result.User.Roles
                };
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar usuário: {Email}", request.Email);
            return StatusCode(500, new ApiError
            {
                Message = "Erro interno do servidor",
                ErrorCode = "INTERNAL_ERROR"
            });
        }
    }

    /// <summary>
    /// Autentica um usuário no sistema
    /// </summary>
    /// <param name="request">Credenciais do usuário</param>
    /// <returns>Token de autenticação</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), 200)]
    [ProducesResponseType(typeof(ApiError), 401)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiError
                {
                    Message = "Dados inválidos",
                    Details = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),
                    ErrorCode = "VALIDATION_ERROR"
                });
            }

            var loginDto = new LoginRequestDto
            {
                Email = request.Email,
                Senha = request.Senha
            };

            var result = await _authService.LoginAsync(loginDto);

            if (!result.Success)
            {
                return Unauthorized(new ApiError
                {
                    Message = result.Message,
                    Details = result.Errors,
                    ErrorCode = "AUTHENTICATION_ERROR"
                });
            }

            var response = new AuthResponse
            {
                Success = true,
                Message = result.Message,
                AccessToken = result.Token,
                RefreshToken = result.RefreshToken,
                ExpiresAt = result.ExpiresAt,
                Endpoint = "login"
            };

            if (result.User != null)
            {
                response.User = new UserInfo
                {
                    Id = result.User.Id,
                    Email = result.User.Email,
                    Nome = result.User.Nome,
                    Roles = result.User.Roles
                };
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer login: {Email}", request.Email);
            return StatusCode(500, new ApiError
            {
                Message = "Erro interno do servidor",
                ErrorCode = "INTERNAL_ERROR"
            });
        }
    }

    /// <summary>
    /// Renova o token de autenticação
    /// </summary>
    /// <param name="request">Token de refresh</param>
    /// <returns>Novo token de autenticação</returns>
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(AuthResponse), 200)]
    [ProducesResponseType(typeof(ApiError), 401)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiError
                {
                    Message = "Dados inválidos",
                    Details = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),
                    ErrorCode = "VALIDATION_ERROR"
                });
            }

            var refreshDto = new RefreshTokenRequestDto
            {
                RefreshToken = request.RefreshToken
            };

            var result = await _authService.RefreshTokenAsync(refreshDto);

            if (!result.Success)
            {
                return Unauthorized(new ApiError
                {
                    Message = result.Message,
                    Details = result.Errors,
                    ErrorCode = "TOKEN_REFRESH_ERROR"
                });
            }

            var response = new AuthResponse
            {
                Success = true,
                Message = result.Message,
                AccessToken = result.Token,
                RefreshToken = result.RefreshToken,
                ExpiresAt = result.ExpiresAt,
                Endpoint = "refresh-token"
            };

            if (result.User != null)
            {
                response.User = new UserInfo
                {
                    Id = result.User.Id,
                    Email = result.User.Email,
                    Nome = result.User.Nome,
                    Roles = result.User.Roles
                };
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao renovar token");
            return StatusCode(500, new ApiError
            {
                Message = "Erro interno do servidor",
                ErrorCode = "INTERNAL_ERROR"
            });
        }
    }
}
