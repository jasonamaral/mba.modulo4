using BFF.API.Extensions;
using BFF.API.Models.Request;
using BFF.API.Models.Response;
using BFF.API.Services.Aulas;
using BFF.API.Settings;
using BFF.Application.Interfaces.Services;
using BFF.Domain.DTOs;
using BFF.Domain.DTOs.Alunos;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
}