using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace Auth.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    /// <summary>
    /// Registra um novo usuário no sistema
    /// </summary>
    /// <param name="model">Dados do usuário para registro</param>
    /// <returns>Confirmação de registro</returns>
    [HttpPost("registro")]
    [ProducesResponseType(typeof(AuthResponse), 200)]
    [ProducesResponseType(typeof(ApiError), 400)]
    public IActionResult Registro([FromBody] RegistroRequest model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiError 
            { 
                Message = "Dados inválidos",
                Details = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        return Ok(new AuthResponse 
        { 
            Success = true,
            Message = "Usuário registrado com sucesso!",
            Endpoint = "registro"
        });
    }

    /// <summary>
    /// Autentica um usuário no sistema
    /// </summary>
    /// <param name="model">Credenciais do usuário</param>
    /// <returns>Token de autenticação</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), 200)]
    [ProducesResponseType(typeof(ApiError), 401)]
    public IActionResult Login([FromBody] LoginRequest model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiError 
            { 
                Message = "Dados inválidos",
                Details = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        return Ok(new AuthResponse 
        { 
            Success = true,
            Message = "Login realizado com sucesso!",
            Endpoint = "login"
        });
    }

    /// <summary>
    /// Renova o token de autenticação
    /// </summary>
    /// <param name="model">Token de refresh</param>
    /// <returns>Novo token de autenticação</returns>
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(AuthResponse), 200)]
    [ProducesResponseType(typeof(ApiError), 401)]
    public IActionResult RefreshToken([FromBody] RefreshTokenRequest model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiError 
            { 
                Message = "Dados inválidos",
                Details = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        return Ok(new AuthResponse 
        { 
            Success = true,
            Message = "Token renovado com sucesso!",
            Endpoint = "refresh-token"
        });
    }
}

#region DTOs

public class RegistroRequest
{
    /// <summary>
    /// Nome do usuário
    /// </summary>
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Email do usuário
    /// </summary>
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Senha do usuário
    /// </summary>
    [Required(ErrorMessage = "Senha é obrigatória")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Senha deve ter entre 6 e 100 caracteres")]
    public string Senha { get; set; } = string.Empty;
}

public class LoginRequest
{
    /// <summary>
    /// Email do usuário
    /// </summary>
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Senha do usuário
    /// </summary>
    [Required(ErrorMessage = "Senha é obrigatória")]
    public string Senha { get; set; } = string.Empty;
}

public class RefreshTokenRequest
{
    /// <summary>
    /// Token de refresh
    /// </summary>
    [Required(ErrorMessage = "Token de refresh é obrigatório")]
    public string RefreshToken { get; set; } = string.Empty;
}

public class AuthResponse
{
    /// <summary>
    /// Indica se a operação foi bem-sucedida
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Mensagem da resposta
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Endpoint chamado (para teste)
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// Token de acesso JWT
    /// </summary>
    public string? AccessToken { get; set; }

    /// <summary>
    /// Token de refresh
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Data de expiração do token
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
}

public class ApiError
{
    /// <summary>
    /// Mensagem de erro
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Detalhes do erro
    /// </summary>
    public IEnumerable<string>? Details { get; set; }
}

#endregion
