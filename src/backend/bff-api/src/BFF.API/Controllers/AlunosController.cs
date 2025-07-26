using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Text;
using BFF.API.Settings;
using Microsoft.Extensions.Options;
using BFF.API.Extensions;
using BFF.API.Models.Response;
using BFF.API.Models.Request;

namespace BFF.API.Controllers;

/// <summary>
/// Controller de alunos no BFF - Orquestra chamadas para Alunos API
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AlunosController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;
    private readonly ILogger<AlunosController> _logger;

    public AlunosController(
        HttpClient httpClient,
        IOptions<ApiSettings> apiSettings,
        ILogger<AlunosController> logger)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings.Value;
        _logger = logger;
    }

    /// <summary>
    /// Obter perfil do aluno pelo código de usuário (do JWT)
    /// </summary>
    /// <returns>Dados do perfil do aluno</returns>
    [HttpGet("meu-perfil")]
    [Authorize(Roles = "Usuario")]
    public async Task<IActionResult> GetMeuPerfil()
    {
        try
        {
            var userId = User.GetUserId();
            if (userId == Guid.Empty)
            {
                return Unauthorized("Token inválido");
            }

            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            _httpClient.DefaultRequestHeaders.Authorization =                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Fazer chamada para Alunos API
            var response = await _httpClient.GetAsync($"{_apiSettings.AlunosApiUrl}/api/alunos/usuario/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
               
                return Ok(JsonSerializer.Deserialize<AlunoPerfil>(responseContent));
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound(new { message = "Perfil do aluno não encontrado. O perfil pode estar sendo criado." });
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Erro ao buscar perfil do aluno");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar busca de perfil via BFF");
            return StatusCode(500, new { error = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Atualizar dados complementares do perfil do aluno
    /// </summary>
    /// <param name="request">Dados para atualização</param>
    /// <returns>Perfil atualizado</returns>
    [HttpPut("meu-perfil")]
    [Authorize(Roles = "Usuario")]
    public async Task<IActionResult> AtualizarMeuPerfil([FromBody] AtualizarPerfilAluno request)
    {
        try
        {
            var userId = User.GetUserId();
            if (userId == Guid.Empty)
            {
                return Unauthorized("Token inválido");
            }

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsync($"{_apiSettings.AlunosApiUrl}/api/alunos/usuario/{userId}", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return Ok(JsonSerializer.Deserialize<AlunoPerfil>(responseContent));
            }
            else
            {
                _logger.LogWarning("Erro ao atualizar perfil do aluno via BFF: {UserId}. Status: {StatusCode}", 
                    userId, response.StatusCode);
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorContent);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar atualização de perfil via BFF");
            return StatusCode(500, new { error = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Obter matrículas do aluno
    /// </summary>
    /// <returns>Lista de matrículas</returns>
    [HttpGet("minhas-matriculas")]
    [Authorize(Roles = "Usuario")]
    public async Task<IActionResult> GetMinhasMatriculas()
    {
        try
        {
            var userId = User.GetUserId();
            if (userId == Guid.Empty)
            {
                return Unauthorized("Token inválido");
            }

            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            _httpClient.DefaultRequestHeaders.Authorization =                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var alunoResponse = await _httpClient.GetAsync($"{_apiSettings.AlunosApiUrl}/api/alunos/usuario/{userId}");
            
            if (!alunoResponse.IsSuccessStatusCode)
            {
                return NotFound(new { message = "Perfil do aluno não encontrado" });
            }

            var alunoContent = await alunoResponse.Content.ReadAsStringAsync();
            var aluno = JsonSerializer.Deserialize<AlunoPerfil>(alunoContent);

            var matriculasResponse = await _httpClient.GetAsync($"{_apiSettings.AlunosApiUrl}/api/alunos/{aluno?.Id}/matriculas");

            if (matriculasResponse.IsSuccessStatusCode)
            {
                var responseContent = await matriculasResponse.Content.ReadAsStringAsync();
                return Ok(responseContent);
            }
            else
            {
                return StatusCode((int)matriculasResponse.StatusCode, "Erro ao buscar matrículas");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar busca de matrículas via BFF");
            return StatusCode(500, new { error = "Erro interno do servidor" });
        }
    }
}
