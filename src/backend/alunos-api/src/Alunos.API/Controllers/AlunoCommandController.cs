using Alunos.Application.Commands.MatricularAluno;
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
    public async Task<IActionResult> MatricularAluno(Guid alunoId, MatricularCursoViewModel dto)
    {
        try
        {
            if (!ModelState.IsValid) { return RespostaPadraoApi<CommandResult>(ModelState); }
            if (alunoId != dto.Id) { return RespostaPadraoApi(HttpStatusCode.BadRequest, "ID do aluno não confere"); }

            CursoDto cursoDto = await _cursoAppService.ObterPorIdAsync(matriculaCursoViewModel.CursoId);
            var comando = new MatricularAlunoCommand(matriculaCursoViewModel.AlunoId, matriculaCursoViewModel.CursoId, cursoDto.CursoDisponivel, cursoDto.Nome, cursoDto.Valor);
            var sucesso = await _mediatorHandler.EnviarComando(comando);
            if (sucesso)
            {
                return GenerateResponse(new { matriculaCursoViewModel.AlunoId, matriculaCursoViewModel.CursoId },
                    responseType: ResponseTypeEnum.Success,
                    statusCode: HttpStatusCode.Created);
            }

            return GenerateResponse(responseType: ResponseTypeEnum.GenericError, statusCode: HttpStatusCode.BadRequest);







            var command = dto.Adapt<AtualizarCursoCommand>();

            return RespostaPadraoApi<CursoDto>(await _mediator.ExecutarComando(command));
        }
        catch (ArgumentException ex)
        {
            return RespostaPadraoApi(HttpStatusCode.NotFound, ex.Message);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    [Authorize(Roles = "Usuario")]
    [HttpPost("{alunoId}/registrar-historico-aprendizado")]
    [ProducesResponseType(typeof(ResponseResult<CategoriaDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 404)]
    public async Task<IActionResult> RegistrarHistoricoAprendizado(RegistrarHistoricoAprendizadoViewModel viewModel)
    {
        throw new NotImplementedException("This method is not implemented yet.");
        //if (!ModelState.IsValid) { return GenerateModelStateResponse(ResponseTypeEnum.ValidationError, HttpStatusCode.BadRequest, ModelState); }

        //try
        //{
        //    if (UserId != viewModel.AlunoId) { return GenerateResponse(null, ResponseTypeEnum.ValidationError, HttpStatusCode.Forbidden, ["Você não tem permissão para realizar essa operação"]); }

        //    var matriculaCurso = await _alunoQueryService.ObterInformacaoMatriculaCursoAsync(viewModel.MatriculaCursoId);
        //    if (matriculaCurso == null) { return GenerateResponse(null, ResponseTypeEnum.NotFound, HttpStatusCode.NotFound, ["Matrícula não encontrada"]); }

        //    CursoDto cursoDto = await _cursoAppService.ObterPorIdAsync(matriculaCurso.CursoId);
        //    if (cursoDto == null) { return GenerateResponse(null, ResponseTypeEnum.NotFound, HttpStatusCode.NotFound, ["Curso desta matrícula não encontrada"]); }

        //    var comando = new RegistrarHistoricoAprendizadoCommand(
        //        viewModel.AlunoId,
        //        viewModel.MatriculaCursoId,
        //        viewModel.AulaId,
        //        cursoDto,
        //        viewModel.DataTermino
        //    );

        //    var sucesso = await _mediatorHandler.EnviarComando(comando);

        //    if (sucesso)
        //    {
        //        return GenerateResponse(new { viewModel.AlunoId, viewModel.MatriculaCursoId, viewModel.AulaId },
        //            responseType: ResponseTypeEnum.Success,
        //            statusCode: HttpStatusCode.Created);
        //    }

        //    return GenerateResponse(responseType: ResponseTypeEnum.GenericError, statusCode: HttpStatusCode.BadRequest);
        //}
        //catch (DomainException exDomain)
        //{
        //    return GenerateDomainExceptionResponse(null, ResponseTypeEnum.DomainError, HttpStatusCode.BadRequest, exDomain);
        //}
        //catch (Exception ex)
        //{
        //    return GenerateResponse(null, ResponseTypeEnum.GenericError, HttpStatusCode.BadRequest, [ex.Message]);
        //}
    }

    [Authorize(Roles = "Usuario")]
    [HttpPut("{alunoId}/concluir-curso")]
    [ProducesResponseType(typeof(ResponseResult<CategoriaDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 404)]
    public async Task<IActionResult> ConcluirCurso(ConcluirCursoViewModel viewModel)
    {
        throw new NotImplementedException("This method is not implemented yet.");
        //if (!ModelState.IsValid) { return GenerateModelStateResponse(ResponseTypeEnum.ValidationError, HttpStatusCode.BadRequest, ModelState); }

        //try
        //{
        //    if (UserId != viewModel.AlunoId) { return GenerateResponse(null, ResponseTypeEnum.ValidationError, HttpStatusCode.Forbidden, ["Você não tem permissão para realizar essa operação"]); }

        //    var matriculaCurso = await _alunoQueryService.ObterInformacaoMatriculaCursoAsync(viewModel.MatriculaCursoId);
        //    if (matriculaCurso == null) { return GenerateResponse(null, ResponseTypeEnum.NotFound, HttpStatusCode.NotFound, ["Matrícula não encontrada"]); }

        //    CursoDto cursoDto = await _cursoAppService.ObterPorIdAsync(matriculaCurso.CursoId);
        //    if (cursoDto == null) { return GenerateResponse(null, ResponseTypeEnum.NotFound, HttpStatusCode.NotFound, ["Curso desta matrícula não encontrada"]); }

        //    var comando = new ConcluirCursoCommand(viewModel.AlunoId, viewModel.MatriculaCursoId, cursoDto);
        //    var sucesso = await _mediatorHandler.EnviarComando(comando);

        //    if (sucesso)
        //    {
        //        return GenerateResponse(new { viewModel.AlunoId, viewModel.MatriculaCursoId },
        //            responseType: ResponseTypeEnum.Success,
        //            statusCode: HttpStatusCode.NoContent);
        //    }

        //    return GenerateResponse(responseType: ResponseTypeEnum.GenericError, statusCode: HttpStatusCode.BadRequest);
        //}
        //catch (DomainException exDomain)
        //{
        //    return GenerateDomainExceptionResponse(null, ResponseTypeEnum.DomainError, HttpStatusCode.BadRequest, exDomain);
        //}
        //catch (Exception ex)
        //{
        //    return GenerateResponse(null, ResponseTypeEnum.GenericError, HttpStatusCode.BadRequest, [ex.Message]);
        //}
    }

    [Authorize(Roles = "Usuario")]
    [HttpPost("{alunoId}/solicitar-certificado")]
    [ProducesResponseType(typeof(ResponseResult<CategoriaDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 404)]
    public async Task<IActionResult> SolicitarCertificado(SolicitarCertificadoViewModel viewModel)
    {
        throw new NotImplementedException("This method is not implemented yet.");
        //if (!ModelState.IsValid) { return GenerateModelStateResponse(ResponseTypeEnum.ValidationError, HttpStatusCode.BadRequest, ModelState); }

        //try
        //{
        //    if (UserId != viewModel.AlunoId) { return GenerateResponse(null, ResponseTypeEnum.ValidationError, HttpStatusCode.Forbidden, ["Você não tem permissão para realizar essa operação"]); }

        //    var comando = new SolicitarCertificadoCommand(viewModel.AlunoId, viewModel.MatriculaCursoId, viewModel.PathCertificado);
        //    var sucesso = await _mediatorHandler.EnviarComando(comando);

        //    if (sucesso)
        //    {
        //        return GenerateResponse(new { viewModel.AlunoId, viewModel.MatriculaCursoId, viewModel.PathCertificado },
        //            responseType: ResponseTypeEnum.Success,
        //            statusCode: HttpStatusCode.Created);
        //    }

        //    return GenerateResponse(responseType: ResponseTypeEnum.GenericError, statusCode: HttpStatusCode.BadRequest);
        //}
        //catch (DomainException exDomain)
        //{
        //    return GenerateDomainExceptionResponse(null, ResponseTypeEnum.DomainError, HttpStatusCode.BadRequest, exDomain);
        //}
        //catch (Exception ex)
        //{
        //    return GenerateResponse(null, ResponseTypeEnum.GenericError, HttpStatusCode.BadRequest, [ex.Message]);
        //}
    }
}
