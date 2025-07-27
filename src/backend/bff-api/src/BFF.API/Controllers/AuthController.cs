using BFF.API.Extensions;
using BFF.API.Models.Request;
using BFF.API.Models.Response;
using BFF.API.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace BFF.API.Controllers;

/// <summary>
/// Controller de autenticação no BFF - Proxy para Auth API
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IHttpClientFactory httpClientFactory,
        IOptions<ApiSettings> apiSettings,
        ILogger<AuthController> logger)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");
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
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiSettings.AuthApiUrl}/api/auth/registro", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent, JsonExtensions.GlobalJsonOptions);
                    return Ok(authResponse);
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Erro ao deserializar resposta da Auth API no registro. Response: {Response}", responseContent);
                    return StatusCode(500, new { error = "Erro interno do servidor - falha na deserialização" });
                }
            }
            else
            {
                return StatusCode((int)response.StatusCode, responseContent);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar registro via BFF para: {Email}", request.Email);
            return StatusCode(500, new { error = "Erro interno do servidor" });
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
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiSettings.AuthApiUrl}/api/auth/login", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent, JsonExtensions.GlobalJsonOptions);
                    return Ok(authResponse);
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Erro ao deserializar resposta da Auth API no login. Response: {Response}", responseContent);
                    return StatusCode(500, new { error = "Erro interno do servidor - falha na deserialização" });
                }
            }
            else
            {
                return StatusCode((int)response.StatusCode, responseContent);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar login via BFF para: {Email}", request.Email);
            return StatusCode(500, new { error = "Erro interno do servidor" });
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
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiSettings.AuthApiUrl}/api/auth/refresh-token", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent, JsonExtensions.GlobalJsonOptions);
                    return Ok(authResponse);
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Erro ao deserializar resposta da Auth API no refresh token. Response: {Response}", responseContent);
                    return StatusCode(500, new { error = "Erro interno do servidor - falha na deserialização" });
                }
            }
            else
            {
                return StatusCode((int)response.StatusCode, responseContent);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar refresh token via BFF");
            return StatusCode(500, new { error = "Erro interno do servidor" });
        }
    }
}