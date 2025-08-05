using Auth.API.Models.Requests;
using Auth.Application.Services;
using Auth.Domain.Entities;
using Core.Communication;
using Core.Controller;
using Core.Messages;
using Core.Messages.Integration;
using Core.Notification;
using MessageBus;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Auth.API.Controllers;

/// <summary>
/// Controller de autenticação
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : MainController
{
    private readonly AuthService _authService;
    private readonly ILogger<AuthController> _logger;
    private readonly IMessageBus _bus;

    public AuthController(AuthService authService, ILogger<AuthController> logger, INotificador notificador, IMessageBus bus) : base(notificador)
    {
        _authService = authService;
        _logger = logger;
        _bus = bus;
    }

    /// <summary>
    /// Registra um novo usuário no sistema
    /// </summary>
    /// <param name="registroRequest">Dados do usuário para registro</param>
    /// <returns>Confirmação de registro</returns>
    [HttpPost("registro")]
    [ProducesResponseType(typeof(ApiSuccess), 200)]
    [ProducesResponseType(typeof(ResponseResult), 400)]
    public async Task<IActionResult> Registro([FromBody] RegistroRequest registroRequest)
    {
        if (!ModelState.IsValid) return RespostaPadraoApi(ModelState);

        var user = new ApplicationUser
        {
            Nome = registroRequest.Nome,
            UserName = registroRequest.Email,
            Email = registroRequest.Email,
            EmailConfirmed = true
        };

        var result = await _authService.UserManager.CreateAsync(user, registroRequest.Senha);

        if (result.Succeeded)
        {
            var clienteResult = await RegistrarCliente(registroRequest);

            if (!clienteResult.ValidationResult.IsValid)
            {
                await _authService.UserManager.DeleteAsync(user);
                return RespostaPadraoApi(clienteResult.ValidationResult);
            }
            return RespostaPadraoApi(HttpStatusCode.OK, await _authService.GerarJwt(registroRequest.Email));
        }

        foreach (var error in result.Errors)
        {
            Notificador.AdicionarErro(error.Description);
        }

        return RespostaPadraoApi();
    }

    /// <summary>
    /// Autentica um usuário no sistema
    /// </summary>
    /// <param name="loginRequest">Credenciais do usuário</param>
    /// <returns>Token de autenticação</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiSuccess), 200)]
    [ProducesResponseType(typeof(ResponseResult), 401)]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (!ModelState.IsValid) return RespostaPadraoApi(ModelState);

        var result = await _authService.SignInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Senha, false, true);

        if (result.Succeeded)
        {
            return RespostaPadraoApi(HttpStatusCode.OK, await _authService.GerarJwt(loginRequest.Email));
        }

        if (result.IsLockedOut)
        {
            Notificador.AdicionarErro("Usuário temporariamente bloqueado por tentativas inválidas");
            return RespostaPadraoApi(HttpStatusCode.Forbidden, "Usuário temporariamente bloqueado por tentativas inválidas");
        }

        Notificador.AdicionarErro("Usuário ou Senha incorretos");
        return RespostaPadraoApi(HttpStatusCode.BadRequest, "Usuário ou Senha incorretos");
    }

    /// <summary>
    /// Renova o token de autenticação
    /// </summary>
    /// <param name="refreshToken">Token de refresh</param>
    /// <returns>Novo token de autenticação</returns>
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ApiSuccess), 200)]
    [ProducesResponseType(typeof(ResponseResult), 401)]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            Notificador.AdicionarErro("Refresh Token inválido.");
            return RespostaPadraoApi(HttpStatusCode.BadRequest, "Refresh Token inválido.");
        }

        var token = await _authService.ObterRefreshToken(Guid.Parse(refreshToken));

        if (token is null)
        {
            Notificador.AdicionarErro("Refresh Token expirado");
            return RespostaPadraoApi(HttpStatusCode.BadRequest, "Refresh Token expirado");
        }

        return RespostaPadraoApi(HttpStatusCode.OK, await _authService.GerarJwt(token.Username));
    }

    private async Task<ResponseMessage> RegistrarCliente(RegistroRequest registroRequest)
    {
        var usuario = await _authService.UserManager.FindByEmailAsync(registroRequest.Email);

        var usuarioRegistrado = new UsuarioRegistradoIntegrationEvent(
             Guid.Parse(usuario!.Id),
             registroRequest.Nome,
             registroRequest.Email,
             registroRequest.CPF,
             registroRequest.DataNascimento,
             registroRequest.Telefone,
             registroRequest.Genero,
             registroRequest.Cidade,
             registroRequest.Estado,
             registroRequest.CEP,
             registroRequest.Foto,
             registroRequest.EhAdministrador,
             DateTime.Now
         );

        try
        {
            return await _bus.RequestAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(usuarioRegistrado);
        }
        catch
        {
            await _authService.UserManager.DeleteAsync(usuario);
            throw;
        }
    }
}