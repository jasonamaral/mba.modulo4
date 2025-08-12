using BFF.API.Services.Aulas;
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
public class AlunosController(IAulaService aulaService,
    ILogger<AlunosController> logger,
    IMediatorHandler mediator,
    INotificationHandler<DomainNotificacaoRaiz> notifications,
    INotificador notificador) : BffController(mediator, notifications, notificador)
{
    private readonly IAulaService _aulaService = aulaService;
    private readonly ILogger<AlunosController> _logger = logger;

    #region Gets
    [Authorize(Roles = "Usuario")]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseResult<AlunoDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    public async Task<IActionResult> ObterAlunoPorIdAsync(Guid alunoId)
    {
        if (alunoId == Guid.Empty)
        {
            return ProcessarErro(System.Net.HttpStatusCode.BadRequest, "Id do aluno é inválido.");
        }

        var resultado = await _aulaService.ObterAlunoPorIdAsync(alunoId, "");

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    [Authorize(Roles = "Usuario")]
    [HttpGet("{id}/evolucao")]
    [ProducesResponseType(typeof(ResponseResult<EvolucaoAlunoDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    public async Task<IActionResult> ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(Guid alunoId)
    {
        if (alunoId == Guid.Empty)
        {
            return ProcessarErro(System.Net.HttpStatusCode.BadRequest, "Id do aluno é inválido.");
        }

        var resultado = await _aulaService.ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(alunoId, "");

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    [Authorize(Roles = "Usuario")]
    [HttpGet("{id}/todas-matriculas")]
    [ProducesResponseType(typeof(ResponseResult<ICollection<MatriculaCursoDto>>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    public async Task<IActionResult> ObterMatriculasPorAlunoIdAsync(Guid alunoId)
    {
        if (alunoId == Guid.Empty)
        {
            return ProcessarErro(System.Net.HttpStatusCode.BadRequest, "Id do aluno é inválido.");
        }

        var resultado = await _aulaService.ObterMatriculasPorAlunoIdAsync(alunoId, "");

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    [Authorize(Roles = "Usuario")]
    [HttpGet("matricula/{matriculaId}/certificado")]
    [ProducesResponseType(typeof(ResponseResult<CertificadoDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    public async Task<IActionResult> ObterCertificadoPorMatriculaIdAsync(Guid matriculaId)
    {
        if (matriculaId == Guid.Empty)
        {
            return ProcessarErro(System.Net.HttpStatusCode.BadRequest, "Id da matrícula é inválida.");
        }

        var resultado = await _aulaService.ObterCertificadoPorMatriculaIdAsync(matriculaId, "");

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    [Authorize(Roles = "Usuario")]
    [HttpGet("aulas/{matriculaId}")]
    [ProducesResponseType(typeof(ResponseResult<ICollection<AulaCursoDto>>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    public async Task<IActionResult> ObterAulasPorMatriculaIdAsync(Guid matriculaId)
    {
        if (matriculaId == Guid.Empty)
        {
            return ProcessarErro(System.Net.HttpStatusCode.BadRequest, "Id da matrícula é inválida.");
        }

        var resultado = await _aulaService.ObterAulasPorMatriculaIdAsync(matriculaId, "");

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }
    #endregion

    #region Posts and Puts
    [Authorize(Roles = "Usuario")]
    [HttpPost("{alunoId}/matricular-aluno")]
    [ProducesResponseType(typeof(ResponseResult<Guid>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 404)]
    public async Task<IActionResult> MatricularAlunoAsync(Guid alunoId, MatriculaCursoRequest dto)
    {
        if (alunoId == Guid.Empty) { return ProcessarErro(System.Net.HttpStatusCode.BadRequest, "Id do aluno é inválida."); }

        var resultado = await _aulaService.MatricularAlunoAsync(dto, "");

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    [Authorize(Roles = "Usuario")]
    [HttpPost("{alunoId}/registrar-historico-aprendizado")]
    [ProducesResponseType(typeof(ResponseResult<bool>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 404)]
    public async Task<IActionResult> RegistrarHistoricoAprendizadoAsync(Guid alunoId, RegistroHistoricoAprendizadoRequest dto)
    {
        if (alunoId == Guid.Empty) { return ProcessarErro(System.Net.HttpStatusCode.BadRequest, "Id do aluno é inválida."); }

        var resultado = await _aulaService.RegistrarHistoricoAprendizadoAsync(dto, "");

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    [Authorize(Roles = "Usuario")]
    [HttpPut("{alunoId}/concluir-curso")]
    [ProducesResponseType(typeof(ResponseResult<bool>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 404)]
    public async Task<IActionResult> ConcluirCursoAsync(Guid alunoId, ConcluirCursoRequest dto)
    {
        if (alunoId == Guid.Empty) { return ProcessarErro(System.Net.HttpStatusCode.BadRequest, "Id do aluno é inválida."); }

        var resultado = await _aulaService.ConcluirCursoAsync(dto, "");

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    [Authorize(Roles = "Usuario")]
    [HttpPost("{alunoId}/solicitar-certificado")]
    [ProducesResponseType(typeof(ResponseResult<Guid>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 404)]
    public async Task<IActionResult> SolicitarCertificadoAsync(Guid alunoId, SolicitaCertificadoRequest dto)
    {
        if (alunoId == Guid.Empty) { return ProcessarErro(System.Net.HttpStatusCode.BadRequest, "Id do aluno é inválida."); }

        var resultado = await _aulaService.SolicitarCertificadoAsync(dto, "");

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }
    #endregion
}