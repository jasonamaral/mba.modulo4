using BFF.API.Services.Aluno;
using BFF.Domain.DTOs.Alunos.Request;
using BFF.Domain.DTOs.Alunos.Response;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BFF.API.Controllers;

/// <summary>
/// Controller de alunos no BFF - Orquestra chamadas para Alunos API
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AlunosController(IAlunoService aulaService,
    ILogger<AlunosController> logger,
    IMediatorHandler mediator,
    INotificationHandler<DomainNotificacaoRaiz> notifications,
    INotificador notificador) : BffController(mediator, notifications, notificador)
{
    private readonly IAlunoService _aulaService = aulaService;
    private readonly ILogger<AlunosController> _logger = logger;

    /// <summary>
    /// Obtem informações do Aluno e Matrículas vinculadas
    /// </summary>
    /// <param name="alunoId">ID do Aluno</param>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpGet("{alunoId}")]
    [ProducesResponseType(typeof(ResponseResult<AlunoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterAlunoPorIdAsync(Guid alunoId)
    {
        if (alunoId == Guid.Empty)
        {
            return ProcessarErro(System.Net.HttpStatusCode.BadRequest, "Id do aluno é inválido.");
        }

        var resultado = await _aulaService.ObterAlunoPorIdAsync(alunoId);

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    /// <summary>
    /// Obtem a evolução do Aluno em um curso matriculado
    /// </summary>
    /// <param name="alunoId">ID do Aluno</param>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpGet("{alunoId}/evolucao")]
    [ProducesResponseType(typeof(ResponseResult<EvolucaoAlunoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(Guid alunoId)
    {
        if (alunoId == Guid.Empty)
        {
            return ProcessarErro(System.Net.HttpStatusCode.BadRequest, "Id do aluno é inválido.");
        }

        var resultado = await _aulaService.ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(alunoId);

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    /// <summary>
    /// Obtem uma lista das matrículas de um determinado aluno
    /// </summary>
    /// <param name="alunoId">ID do Aluno</param>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpGet("{alunoId}/todas-matriculas")]
    [ProducesResponseType(typeof(ResponseResult<ICollection<MatriculaCursoDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterMatriculasPorAlunoIdAsync(Guid alunoId)
    {
        if (alunoId == Guid.Empty)
        {
            return ProcessarErro(System.Net.HttpStatusCode.BadRequest, "Id do aluno é inválido.");
        }

        var resultado = await _aulaService.ObterMatriculasPorAlunoIdAsync(alunoId);

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    /// <summary>
    /// Obtem o certificado de conclusão de um curso
    /// </summary>
    /// <param name="matriculaId">ID da matrícula no curso</param>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpGet("matricula/{matriculaId}/certificado")]
    [ProducesResponseType(typeof(ResponseResult<CertificadoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterCertificadoPorMatriculaIdAsync(Guid matriculaId)
    {
        if (matriculaId == Guid.Empty)
        {
            return ProcessarErro(System.Net.HttpStatusCode.BadRequest, "Id da matrícula é inválida.");
        }

        var resultado = await _aulaService.ObterCertificadoPorMatriculaIdAsync(matriculaId);

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    /// <summary>
    /// Obtem as aulas de um determinado curso onde o aluno está matriculado
    /// </summary>
    /// <param name="matriculaId">ID da matrícula</param>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpGet("aulas/{matriculaId}")]
    [ProducesResponseType(typeof(ResponseResult<ICollection<AulaCursoDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterAulasPorMatriculaIdAsync(Guid matriculaId)
    {
        if (matriculaId == Guid.Empty)
        {
            return ProcessarErro(System.Net.HttpStatusCode.BadRequest, "Id da matrícula é inválida.");
        }

        var resultado = await _aulaService.ObterAulasPorMatriculaIdAsync(matriculaId);

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    /// <summary>
    /// Realiza a matrícula do aluno em um curso
    /// </summary>
    /// <param name="alunoId">ID do aluno</param>
    /// <param name="dto">Objeto com informação do curso</param>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpPost("{alunoId}/matricular-aluno")]
    [ProducesResponseType(typeof(ResponseResult<Guid?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> MatricularAlunoAsync(Guid alunoId, MatriculaCursoRequest dto)
    {
        if (alunoId == Guid.Empty) { return ProcessarErro(System.Net.HttpStatusCode.BadRequest, "Id do aluno é inválida."); }

        var resultado = await _aulaService.MatricularAlunoAsync(dto);

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    /// <summary>
    /// Registra o histórico de uma aula em andamento
    /// </summary>
    /// <param name="alunoId">ID do aluno</param>
    /// <param name="dto">Objeto com informação da aula</param>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpPost("{alunoId}/registrar-historico-aprendizado")]
    [ProducesResponseType(typeof(ResponseResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegistrarHistoricoAprendizadoAsync(Guid alunoId, RegistroHistoricoAprendizadoRequest dto)
    {
        if (alunoId == Guid.Empty) { return ProcessarErro(System.Net.HttpStatusCode.BadRequest, "Id do aluno é inválida."); }

        var resultado = await _aulaService.RegistrarHistoricoAprendizadoAsync(dto);

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    /// <summary>
    /// Registra a conclusão do curso
    /// </summary>
    /// <param name="alunoId">ID do aluno</param>
    /// <param name="dto">Objeto com informação do curso</param>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpPut("{alunoId}/concluir-curso")]
    [ProducesResponseType(typeof(ResponseResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ConcluirCursoAsync(Guid alunoId, ConcluirCursoRequest dto)
    {
        if (alunoId == Guid.Empty) { return ProcessarErro(System.Net.HttpStatusCode.BadRequest, "Id do aluno é inválida."); }

        var resultado = await _aulaService.ConcluirCursoAsync(dto);

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    /// <summary>
    /// Registra a solicitação de conclusão do curso
    /// </summary>
    /// <param name="alunoId">ID do aluno</param>
    /// <param name="dto">Objeto com informação do curso</param>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpPost("{alunoId}/solicitar-certificado")]
    [ProducesResponseType(typeof(ResponseResult<Guid?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SolicitarCertificadoAsync(Guid alunoId, SolicitaCertificadoRequest dto)
    {
        if (alunoId == Guid.Empty) { return ProcessarErro(System.Net.HttpStatusCode.BadRequest, "Id do aluno é inválida."); }

        var resultado = await _aulaService.SolicitarCertificadoAsync(dto);

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }
}