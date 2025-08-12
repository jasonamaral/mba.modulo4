using BFF.API.Extensions;
using BFF.API.Models.Request;
using BFF.API.Models.Response;
using BFF.API.Settings;
using BFF.Application.Interfaces.Services;
using BFF.Domain.DTOs;
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
    private readonly IAlunoStoreService _store;
    private readonly ApiSettings _apiSettings;
    private readonly ILogger<AlunosController> _logger;

    public AlunosController(
        IApiClientService apiClient,
        IOptions<ApiSettings> apiSettings,
        IAlunoStoreService store,
        ILogger<AlunosController> logger,
        IMediatorHandler mediator,
        INotificationHandler<DomainNotificacaoRaiz> notifications,
        INotificador notificador) : base(mediator, notifications, notificador)
    {
        _apiClient = apiClient;
        _store = store;
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

            _apiClient.SetBaseAddress(_apiSettings.AlunosApiUrl);
            _apiClient.ClearDefaultHeaders();

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

            _apiClient.SetBaseAddress(_apiSettings.AlunosApiUrl);
            _apiClient.ClearDefaultHeaders();

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

            var lista = await _store.ListarMatriculasAsync(userId);
            return RespostaPadraoApi(System.Net.HttpStatusCode.OK, lista, "Matrículas do aluno obtidas com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar busca de matrículas via BFF");
            return ProcessarErro(System.Net.HttpStatusCode.InternalServerError, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Cria uma matrícula temporária (store BFF) e prepara checkout
    /// </summary>
    [HttpPost("matriculas")]
    [Authorize(Roles = "Usuario")]
    public async Task<IActionResult> CriarMatricula([FromBody] dynamic body)
    {
        var userId = User.GetUserId();
        if (userId == Guid.Empty)
            return ProcessarErro(System.Net.HttpStatusCode.Unauthorized, "Token inválido");

        try
        {
            // Lê cursoId do body
            Guid cursoId = Guid.Parse((string)body.cursoId);

            // Busca nome do curso na Conteudo API para enriquecer
            _apiClient.SetBaseAddress(_apiSettings.ConteudoApiUrl);
            _apiClient.ClearDefaultHeaders();
            var curso = await _apiClient.GetAsync<ResponseResult<CursoDto>>($"api/cursos/{cursoId}");

            var cursoNome = curso?.Data?.Nome ?? string.Empty;
            var matricula = await _store.CriarMatriculaAsync(userId, cursoId, cursoNome);

            return RespostaPadraoApi(System.Net.HttpStatusCode.Created, new { matriculaId = matricula.Id }, "Matrícula criada e pendente de pagamento");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar matrícula");
            return ProcessarErro(System.Net.HttpStatusCode.InternalServerError, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Atualiza progresso do aluno em uma aula
    /// </summary>
    [HttpPost("aulas/{aulaId}/progresso")]
    [Authorize(Roles = "Usuario")]
    public async Task<IActionResult> PostProgresso(Guid aulaId, [FromBody] dynamic body)
    {
        var userId = User.GetUserId();
        if (userId == Guid.Empty)
            return ProcessarErro(System.Net.HttpStatusCode.Unauthorized, "Token inválido");

        try
        {
            Guid cursoId = Guid.Parse((string)body.cursoId);
            decimal percentual = (decimal)body.percentual;
            await _store.AtualizarProgressoAulaAsync(userId, cursoId, aulaId, percentual);
            return RespostaPadraoApi<object>(System.Net.HttpStatusCode.OK, null, "Progresso atualizado");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar progresso");
            return ProcessarErro(System.Net.HttpStatusCode.InternalServerError, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Emite certificado caso curso concluído
    /// </summary>
    [HttpPost("cursos/{cursoId}/finalizar")]
    [Authorize(Roles = "Usuario")]
    public async Task<IActionResult> FinalizarCurso(Guid cursoId)
    {
        var userId = User.GetUserId();
        if (userId == Guid.Empty)
            return ProcessarErro(System.Net.HttpStatusCode.Unauthorized, "Token inválido");

        try
        {
            var matricula = (await _store.ListarMatriculasAsync(userId)).FirstOrDefault(m => m.CursoId == cursoId);
            if (matricula == null)
                return ProcessarErro(System.Net.HttpStatusCode.NotFound, "Matrícula não encontrada");

            if (matricula.PercentualConclusao < 100)
                return ProcessarErro(System.Net.HttpStatusCode.BadRequest, "Curso ainda não concluído");

            var cert = await _store.EmitirCertificadoAsync(userId, cursoId, matricula.CursoNome);
            return RespostaPadraoApi(System.Net.HttpStatusCode.OK, cert, "Certificado emitido");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao finalizar curso");
            return ProcessarErro(System.Net.HttpStatusCode.InternalServerError, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Lista certificados do aluno
    /// </summary>
    [HttpGet("certificados")]
    [Authorize(Roles = "Usuario")]
    public async Task<IActionResult> ListarCertificados()
    {
        var userId = User.GetUserId();
        if (userId == Guid.Empty)
            return ProcessarErro(System.Net.HttpStatusCode.Unauthorized, "Token inválido");

        var certs = await _store.ListarCertificadosAsync(userId);
        return RespostaPadraoApi(System.Net.HttpStatusCode.OK, certs, "Certificados");
    }
}