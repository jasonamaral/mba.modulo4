using BFF.API.Models.Request;
using BFF.API.Models.Response;
using BFF.API.Settings;
using BFF.Application.Interfaces.Services;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BFF.API.Controllers;

/// <summary>
/// Controller de autenticação no BFF - Proxy para Auth API
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : BffController
{
    private readonly IApiClientService _apiClient;
    private readonly ApiSettings _apiSettings;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IApiClientService apiClient,
        IOptions<ApiSettings> apiSettings,
        ILogger<AuthController> logger,
        IMediatorHandler mediator,
        INotificationHandler<DomainNotificacaoRaiz> notifications,
        INotificador notificador) : base(mediator, notifications, notificador)
    {
        _apiClient = apiClient;
        _apiSettings = apiSettings.Value;
        _logger = logger;
    }

    /// <summary>
    /// Registrar novo usuário
    /// </summary>
    /// <param name="request">Dados de registro</param>
    /// <returns>Resposta da autenticação</returns>
    [HttpPost("registro")]
    public async Task<IActionResult> Registro([FromBody] RegistroRequest request)
    {
        try
        {
            _apiClient.SetBaseAddress(_apiSettings.AuthApiUrl);
            var result = await _apiClient.PostAsyncWithActionResult<RegistroRequest, ResponseResult<AuthRegistroResponse>>(
                "/api/auth/registro",
                request,
                "Usuário registrado com sucesso");

            if (result.Success && result.Data != null)
            {
                // Se o resultado é um ResponseResult genérico, extrair o Data interno
                var dataType = result.Data.GetType();
                if (dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof(ResponseResult<>))
                {
                    var dataProperty = dataType.GetProperty("Data");
                    var innerData = dataProperty?.GetValue(result.Data);
                    if (innerData != null)
                    {
                        return RespostaPadraoApi(System.Net.HttpStatusCode.OK, innerData, result.Message);
                    }
                }

                return RespostaPadraoApi(System.Net.HttpStatusCode.OK, result.Data, result.Message);
            }

            if (result.ErrorContent != null)
            {
                return StatusCode(result.StatusCode, result.ErrorContent);
            }

            return ProcessarErro(System.Net.HttpStatusCode.BadRequest, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar registro via BFF para: {Email}", request.Email);
            return ProcessarErro(System.Net.HttpStatusCode.InternalServerError, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Fazer login
    /// </summary>
    /// <param name="request">Credenciais de login</param>
    /// <returns>Resposta da autenticação</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            _apiClient.SetBaseAddress(_apiSettings.AuthApiUrl);
            var result = await _apiClient.PostAsyncWithActionResult<LoginRequest, ResponseResult<AuthLoginResponse>>(
                "/api/auth/login",
                request,
                "Login realizado com sucesso");

            if (result.Success && result.Data != null)
            {
                // Se o resultado é um ResponseResult genérico, extrair o Data interno
                var dataType = result.Data.GetType();
                if (dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof(ResponseResult<>))
                {
                    var dataProperty = dataType.GetProperty("Data");
                    var innerData = dataProperty?.GetValue(result.Data);
                    if (innerData != null)
                    {
                        return RespostaPadraoApi(System.Net.HttpStatusCode.OK, innerData, result.Message);
                    }
                }

                return RespostaPadraoApi(System.Net.HttpStatusCode.OK, result.Data, result.Message);
            }

            if (result.ErrorContent != null)
            {
                return StatusCode(result.StatusCode, result.ErrorContent);
            }

            return ProcessarErro(System.Net.HttpStatusCode.BadRequest, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar login via BFF para: {Email}", request.Email);
            return ProcessarErro(System.Net.HttpStatusCode.InternalServerError, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Renovar token
    /// </summary>
    /// <param name="request">Request de refresh token</param>
    /// <returns>Novo token</returns>
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        try
        {
            _apiClient.SetBaseAddress(_apiSettings.AuthApiUrl);
            var result = await _apiClient.PostAsyncWithActionResult<RefreshTokenRequest, ResponseResult<AuthRefreshTokenResponse>>("/api/auth/refresh-token", request, "Token renovado com sucesso");

            if (result.Success && result.Data != null)
            {
                // Se o resultado é um ResponseResult genérico, extrair o Data interno
                var dataType = result.Data.GetType();
                if (dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof(ResponseResult<>))
                {
                    var dataProperty = dataType.GetProperty("Data");
                    var innerData = dataProperty?.GetValue(result.Data);
                    if (innerData != null)
                    {
                        return RespostaPadraoApi(System.Net.HttpStatusCode.OK, innerData, result.Message);
                    }
                }

                return RespostaPadraoApi(System.Net.HttpStatusCode.OK, result.Data, result.Message);
            }

            if (result.ErrorContent != null)
            {
                return StatusCode(result.StatusCode, result.ErrorContent);
            }

            return ProcessarErro(System.Net.HttpStatusCode.BadRequest, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar refresh token via BFF");
            return ProcessarErro(System.Net.HttpStatusCode.InternalServerError, "Erro interno do servidor");
        }
    }
}