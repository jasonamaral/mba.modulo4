using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;

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
    /// <returns>Confirmação de registro com tokens</returns>
    [HttpPost("registro")]
    [ProducesResponseType(typeof(AuthResponseDto), 200)]
    [ProducesResponseType(typeof(AuthResponseDto), 400)]
    public async Task<IActionResult> Registro([FromBody] RegisterRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new AuthResponseDto 
            { 
                Success = false,
                Message = "Dados inválidos",
                Errors = [.. ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)]
            });
        }

        var result = await _authService.RegisterAsync(request);
        
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Autentica um usuário no sistema
    /// </summary>
    /// <param name="request">Credenciais do usuário</param>
    /// <returns>Token de autenticação</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), 200)]
    [ProducesResponseType(typeof(AuthResponseDto), 401)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new AuthResponseDto 
            { 
                Success = false,
                Message = "Dados inválidos",
                Errors = [.. ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)]
            });
        }

        var result = await _authService.LoginAsync(request);
        
        if (result.Success)
        {
            return Ok(result);
        }

        return Unauthorized(result);
    }

    /// <summary>
    /// Renova o token de autenticação
    /// </summary>
    /// <param name="request">Token de refresh</param>
    /// <returns>Novo token de autenticação</returns>
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(AuthResponseDto), 200)]
    [ProducesResponseType(typeof(AuthResponseDto), 401)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new AuthResponseDto 
            { 
                Success = false,
                Message = "Dados inválidos",
                Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
            });
        }

        var result = await _authService.RefreshTokenAsync(request);
        
        if (result.Success)
        {
            return Ok(result);
        }

        return Unauthorized(result);
    }

    /// <summary>
    /// Faz logout do usuário
    /// </summary>
    /// <returns>Confirmação de logout</returns>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest(new { Message = "Usuário não identificado" });
        }

        var result = await _authService.LogoutAsync(userId);
        
        if (result)
        {
            return Ok(new { Message = "Logout realizado com sucesso" });
        }

        return BadRequest(new { Message = "Erro ao realizar logout" });
    }

    /// <summary>
    /// Valida um token JWT
    /// </summary>
    /// <param name="token">Token a ser validado</param>
    /// <returns>Resultado da validação</returns>
    [HttpPost("validate-token")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> ValidateToken([FromBody] string token)
    {
        var isValid = await _authService.ValidateTokenAsync(token);
        
        if (isValid)
        {
            return Ok(new { Valid = true, Message = "Token válido" });
        }

        return Unauthorized(new { Valid = false, Message = "Token inválido" });
    }
}
