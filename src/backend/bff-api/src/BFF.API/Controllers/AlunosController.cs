using BFF.API.Extensions;
using BFF.API.Models.Request;
using BFF.API.Models.Response;
using BFF.API.Settings;
using BFF.Application.Interfaces.Services;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BFF.API.Controllers;

/// <summary>
/// Controller de alunos no BFF - Orquestra chamadas para Alunos API
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AlunosController : BffController
{
    private readonly IApiClientService _apiClient;
    private readonly ApiSettings _apiSettings;
    private readonly ILogger<AlunosController> _logger;

    public AlunosController(
        IApiClientService apiClient,
        IOptions<ApiSettings> apiSettings,
        ILogger<AlunosController> logger,
        IMediatorHandler mediator,
        INotificationHandler<DomainNotificacaoRaiz> notifications,
        INotificador notificador) : base(mediator, notifications, notificador)
    {
        _apiClient = apiClient;
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
                return ProcessarErro(System.Net.HttpStatusCode.Unauthorized, "Token inválido");
            }

            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            _apiClient.SetBaseAddress(_apiSettings.AlunosApiUrl);
            _apiClient.ClearDefaultHeaders();
            _apiClient.AddDefaultHeader("Authorization", $"Bearer {token}");
            _apiClient.AddDefaultHeader("Accept", "application/json");
            _apiClient.AddDefaultHeader("Content-Type", "application/json");

            var response = await _apiClient.GetAsync<ResponseResult<AlunoPerfilResponse>>($"/api/alunos/usuario/{userId}");

            if (response?.Data != null)
            {
                return RespostaPadraoApi(System.Net.HttpStatusCode.OK, response.Data, "Perfil do aluno obtido com sucesso");
            }

            return ProcessarErro(System.Net.HttpStatusCode.NotFound, "Perfil do aluno não encontrado. O perfil pode estar sendo criado.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar busca de perfil via BFF");
            return ProcessarErro(System.Net.HttpStatusCode.InternalServerError, "Erro interno do servidor");
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
                return ProcessarErro(System.Net.HttpStatusCode.Unauthorized, "Token inválido");
            }

            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            _apiClient.SetBaseAddress(_apiSettings.AlunosApiUrl);
            _apiClient.ClearDefaultHeaders();
            _apiClient.AddDefaultHeader("Authorization", $"Bearer {token}");
            _apiClient.AddDefaultHeader("Accept", "application/json");
            _apiClient.AddDefaultHeader("Content-Type", "application/json");

            var result = await _apiClient.PutAsyncWithActionResult<AtualizarPerfilAluno, ResponseResult<AlunoPerfilResponse>>(
                $"/api/alunos/usuario/{userId}", 
                request, 
                "Perfil do aluno atualizado com sucesso");

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
            _logger.LogError(ex, "Erro ao processar atualização de perfil via BFF");
            return ProcessarErro(System.Net.HttpStatusCode.InternalServerError, "Erro interno do servidor");
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
                return ProcessarErro(System.Net.HttpStatusCode.Unauthorized, "Token inválido");
            }

            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            _apiClient.SetBaseAddress(_apiSettings.AlunosApiUrl);
            _apiClient.ClearDefaultHeaders();
            _apiClient.AddDefaultHeader("Authorization", $"Bearer {token}");
            _apiClient.AddDefaultHeader("Accept", "application/json");
            _apiClient.AddDefaultHeader("Content-Type", "application/json");

            // Primeiro, buscar o perfil do aluno
            var aluno = await _apiClient.GetAsync<ResponseResult<AlunoPerfilResponse>>($"/api/alunos/usuario/{userId}");

            if (aluno?.Data == null)
            {
                return ProcessarErro(System.Net.HttpStatusCode.NotFound, "Perfil do aluno não encontrado");
            }

            // Depois, buscar as matrículas
            var matriculas = await _apiClient.GetAsync<ResponseResult<AlunoMatriculasResponse>>($"/api/alunos/{userId}/matriculas");

            if (matriculas?.Data != null)
            {
                return RespostaPadraoApi(System.Net.HttpStatusCode.OK, matriculas.Data, "Matrículas do aluno obtidas com sucesso");
            }

            return ProcessarErro(System.Net.HttpStatusCode.NotFound, "Matrículas não encontradas");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar busca de matrículas via BFF");
            return ProcessarErro(System.Net.HttpStatusCode.InternalServerError, "Erro interno do servidor");
        }
    }
}