using Alunos.Application.Commands.ConcluirCurso;
using Alunos.Application.Commands.MatricularAluno;
using Alunos.Application.Commands.RegistrarHistoricoAprendizado;
using Alunos.Application.Commands.SolicitarCertificado;
using Alunos.Application.DTOs.Request;
using Alunos.Application.Interfaces;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using Core.Services.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Alunos.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public partial class AlunoController(IMediatorHandler mediator, 
    IAlunoQueryService alunoQueryService,
    INotificationHandler<DomainNotificacaoRaiz> notifications,
    INotificador notificador) : MainController(mediator, notifications, notificador)
{
    private readonly IAlunoQueryService _alunoQueryService = alunoQueryService;

    [Authorize(Roles = "Usuario")]
    [HttpPost("{alunoId}/matricular-aluno")]
    [ProducesResponseType(typeof(ResponseResult<Guid>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 404)]
    public async Task<IActionResult> MatricularAluno(Guid alunoId, MatriculaCursoRequest dto)
    {
        try
        {
            if (!ModelState.IsValid) { return RespostaPadraoApi<CommandResult>(ModelState); }
            if (alunoId != dto.AlunoId) { return RespostaPadraoApi(HttpStatusCode.BadRequest, "ID do aluno não confere"); }

            var comando = new MatricularAlunoCommand(dto.AlunoId, dto.CursoId, dto.CursoDisponivel, dto.Nome, dto.Valor, dto.Observacao);
            return RespostaPadraoApi<Guid>(await _mediatorHandler.ExecutarComando(comando));
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    [Authorize(Roles = "Usuario")]
    [HttpPost("{alunoId}/registrar-historico-aprendizado")]
    [ProducesResponseType(typeof(ResponseResult<bool>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 404)]
    public async Task<IActionResult> RegistrarHistoricoAprendizado(Guid alunoId, RegistroHistoricoAprendizadoRequest dto)
    {
        try
        {
            if (!ModelState.IsValid) { return RespostaPadraoApi<CommandResult>(ModelState); }
            if (alunoId != dto.AlunoId) { return RespostaPadraoApi(HttpStatusCode.BadRequest, "ID do aluno não confere"); }

            var matriculaCurso = await _alunoQueryService.ObterInformacaoMatriculaCursoAsync(dto.MatriculaCursoId);
            if (matriculaCurso == null) { return RespostaPadraoApi(HttpStatusCode.BadRequest, "Matrícula não encontrada"); }

            var comando = new RegistrarHistoricoAprendizadoCommand(dto.AlunoId,
                dto.MatriculaCursoId,
                dto.AulaId,
                dto.NomeAula,
                dto.DuracaoMinutos,
                dto.DataTermino
            );

            return RespostaPadraoApi<Guid>(await _mediatorHandler.ExecutarComando(comando));
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    [Authorize(Roles = "Usuario")]
    [HttpPut("{alunoId}/concluir-curso")]
    [ProducesResponseType(typeof(ResponseResult<bool>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 404)]
    public async Task<IActionResult> ConcluirCurso(Guid alunoId, ConcluirCursoRequest dto)
    {
        try
        {
            if (!ModelState.IsValid) { return RespostaPadraoApi<CommandResult>(ModelState); }
            if (alunoId != dto.AlunoId) { return RespostaPadraoApi(HttpStatusCode.BadRequest, "ID do aluno não confere"); }
            if (dto.CursoDto == null) { return RespostaPadraoApi(HttpStatusCode.BadRequest, "Curso desta matrícula não encontrada"); }

            var matriculaCurso = await _alunoQueryService.ObterInformacaoMatriculaCursoAsync(dto.MatriculaCursoId);
            if (matriculaCurso == null) { return RespostaPadraoApi(HttpStatusCode.BadRequest, "Matrícula não encontrada"); }

            var comando = new ConcluirCursoCommand(dto.AlunoId, dto.MatriculaCursoId, dto.CursoDto);
            return RespostaPadraoApi<bool>(await _mediatorHandler.ExecutarComando(comando));
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    [Authorize(Roles = "Usuario")]
    [HttpPost("{alunoId}/solicitar-certificado")]
    [ProducesResponseType(typeof(ResponseResult<Guid>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 404)]
    public async Task<IActionResult> SolicitarCertificado(Guid alunoId, SolicitaCertificadoRequest dto)
    {
        try
        {
            if (!ModelState.IsValid) { return RespostaPadraoApi<CommandResult>(ModelState); }
            if (alunoId != dto.AlunoId) { return RespostaPadraoApi(HttpStatusCode.BadRequest, "ID do aluno não confere"); }

            var comando = new SolicitarCertificadoCommand(dto.AlunoId, dto.MatriculaCursoId);
            return RespostaPadraoApi<bool>(await _mediatorHandler.ExecutarComando(comando));
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }
}
