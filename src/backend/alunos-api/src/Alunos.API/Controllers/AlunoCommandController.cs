using Alunos.Application.Interfaces;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using Core.Services.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    //[Authorize(Policy = "ApenasAluno")]
    //[HttpPost("{alunoId}/matricular-aluno")]
    //public async Task<IActionResult> MatricularAluno(Guid alunoId, MatricularCursoViewModel matriculaCursoViewModel)
    //{
    //    if (!ModelState.IsValid) { return GenerateModelStateResponse(ResponseTypeEnum.ValidationError, HttpStatusCode.BadRequest, ModelState); }

    //    try
    //    {
    //        if (UserId != matriculaCursoViewModel.AlunoId) { return GenerateResponse(null, ResponseTypeEnum.ValidationError, HttpStatusCode.Forbidden, ["Você não tem permissão para realizar essa operação"]); }

    //        CursoDto cursoDto = await _cursoAppService.ObterPorIdAsync(matriculaCursoViewModel.CursoId);
    //        var comando = new MatricularAlunoCommand(matriculaCursoViewModel.AlunoId, matriculaCursoViewModel.CursoId, cursoDto.CursoDisponivel, cursoDto.Nome, cursoDto.Valor);
    //        var sucesso = await _mediatorHandler.EnviarComando(comando);
    //        if (sucesso)
    //        {
    //            return GenerateResponse(new { matriculaCursoViewModel.AlunoId, matriculaCursoViewModel.CursoId },
    //                responseType: ResponseTypeEnum.Success,
    //                statusCode: HttpStatusCode.Created);
    //        }

    //        return GenerateResponse(responseType: ResponseTypeEnum.GenericError, statusCode: HttpStatusCode.BadRequest);
    //    }
    //    catch (DomainException exDomain)
    //    {
    //        return GenerateDomainExceptionResponse(null, ResponseTypeEnum.DomainError, HttpStatusCode.BadRequest, exDomain);
    //    }
    //    catch (Exception ex)
    //    {
    //        return GenerateResponse(null, ResponseTypeEnum.GenericError, HttpStatusCode.BadRequest, [ex.Message]);
    //    }
    //}

    //#region Devo ou não manter isto???
    ////[Authorize(Policy = "ApenasAluno")]
    ////[HttpPut("{alunoId}/atualizar-pagamento-matricula")]
    ////public async Task<IActionResult> AtualizarPagamentoMatricula(Guid alunoId, AtualizarPagamentoMatriculaViewModel viewModel)
    ////{
    ////    if (!ModelState.IsValid) { return GenerateModelStateResponse(ResponseTypeEnum.ValidationError, HttpStatusCode.BadRequest, ModelState); }

    ////    try
    ////    {
    ////        if (UserId != viewModel.AlunoId) { return GenerateResponse(null, ResponseTypeEnum.ValidationError, HttpStatusCode.Forbidden, ["Você não tem permissão para realizar essa operação"]); }

    ////        var comando = new AtualizarPagamentoMatriculaCommand(viewModel.AlunoId, viewModel.CursoId);
    ////        var sucesso = await _mediatorHandler.EnviarComando(comando);

    ////        if (sucesso)
    ////        {
    ////            return GenerateResponse(new { viewModel.AlunoId, viewModel.CursoId },
    ////                responseType: ResponseTypeEnum.Success,
    ////                statusCode: HttpStatusCode.NoContent);
    ////        }

    ////        return GenerateResponse(responseType: ResponseTypeEnum.GenericError, statusCode: HttpStatusCode.BadRequest);
    ////    }
    ////    catch (DomainException exDomain)
    ////    {
    ////        return GenerateDomainExceptionResponse(null, ResponseTypeEnum.DomainError, HttpStatusCode.BadRequest, exDomain);
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        return GenerateResponse(null, ResponseTypeEnum.GenericError, HttpStatusCode.BadRequest, [ex.Message]);
    ////    }
    ////}
    //#endregion

    //[Authorize(Policy = "ApenasAluno")]
    //[HttpPost("{alunoId}/registrar-historico-aprendizado")]
    //public async Task<IActionResult> RegistrarHistoricoAprendizado(RegistrarHistoricoAprendizadoViewModel viewModel)
    //{
    //    if (!ModelState.IsValid) { return GenerateModelStateResponse(ResponseTypeEnum.ValidationError, HttpStatusCode.BadRequest, ModelState); }

    //    try
    //    {
    //        if (UserId != viewModel.AlunoId) { return GenerateResponse(null, ResponseTypeEnum.ValidationError, HttpStatusCode.Forbidden, ["Você não tem permissão para realizar essa operação"]); }

    //        var matriculaCurso = await _alunoQueryService.ObterInformacaoMatriculaCursoAsync(viewModel.MatriculaCursoId);
    //        if (matriculaCurso == null) { return GenerateResponse(null, ResponseTypeEnum.NotFound, HttpStatusCode.NotFound, ["Matrícula não encontrada"]); }

    //        CursoDto cursoDto = await _cursoAppService.ObterPorIdAsync(matriculaCurso.CursoId);
    //        if (cursoDto == null) { return GenerateResponse(null, ResponseTypeEnum.NotFound, HttpStatusCode.NotFound, ["Curso desta matrícula não encontrada"]); }

    //        var comando = new RegistrarHistoricoAprendizadoCommand(
    //            viewModel.AlunoId,
    //            viewModel.MatriculaCursoId,
    //            viewModel.AulaId,
    //            cursoDto,
    //            viewModel.DataTermino
    //        );

    //        var sucesso = await _mediatorHandler.EnviarComando(comando);

    //        if (sucesso)
    //        {
    //            return GenerateResponse(new { viewModel.AlunoId, viewModel.MatriculaCursoId, viewModel.AulaId },
    //                responseType: ResponseTypeEnum.Success,
    //                statusCode: HttpStatusCode.Created);
    //        }

    //        return GenerateResponse(responseType: ResponseTypeEnum.GenericError, statusCode: HttpStatusCode.BadRequest);
    //    }
    //    catch (DomainException exDomain)
    //    {
    //        return GenerateDomainExceptionResponse(null, ResponseTypeEnum.DomainError, HttpStatusCode.BadRequest, exDomain);
    //    }
    //    catch (Exception ex)
    //    {
    //        return GenerateResponse(null, ResponseTypeEnum.GenericError, HttpStatusCode.BadRequest, [ex.Message]);
    //    }
    //}

    //[Authorize(Policy = "ApenasAluno")]
    //[HttpPut("{alunoId}/concluir-curso")]
    //public async Task<IActionResult> ConcluirCurso(ConcluirCursoViewModel viewModel)
    //{
    //    if (!ModelState.IsValid) { return GenerateModelStateResponse(ResponseTypeEnum.ValidationError, HttpStatusCode.BadRequest, ModelState); }

    //    try
    //    {
    //        if (UserId != viewModel.AlunoId) { return GenerateResponse(null, ResponseTypeEnum.ValidationError, HttpStatusCode.Forbidden, ["Você não tem permissão para realizar essa operação"]); }

    //        var matriculaCurso = await _alunoQueryService.ObterInformacaoMatriculaCursoAsync(viewModel.MatriculaCursoId);
    //        if (matriculaCurso == null) { return GenerateResponse(null, ResponseTypeEnum.NotFound, HttpStatusCode.NotFound, ["Matrícula não encontrada"]); }

    //        CursoDto cursoDto = await _cursoAppService.ObterPorIdAsync(matriculaCurso.CursoId);
    //        if (cursoDto == null) { return GenerateResponse(null, ResponseTypeEnum.NotFound, HttpStatusCode.NotFound, ["Curso desta matrícula não encontrada"]); }

    //        var comando = new ConcluirCursoCommand(viewModel.AlunoId, viewModel.MatriculaCursoId, cursoDto);
    //        var sucesso = await _mediatorHandler.EnviarComando(comando);

    //        if (sucesso)
    //        {
    //            return GenerateResponse(new { viewModel.AlunoId, viewModel.MatriculaCursoId },
    //                responseType: ResponseTypeEnum.Success,
    //                statusCode: HttpStatusCode.NoContent);
    //        }

    //        return GenerateResponse(responseType: ResponseTypeEnum.GenericError, statusCode: HttpStatusCode.BadRequest);
    //    }
    //    catch (DomainException exDomain)
    //    {
    //        return GenerateDomainExceptionResponse(null, ResponseTypeEnum.DomainError, HttpStatusCode.BadRequest, exDomain);
    //    }
    //    catch (Exception ex)
    //    {
    //        return GenerateResponse(null, ResponseTypeEnum.GenericError, HttpStatusCode.BadRequest, [ex.Message]);
    //    }
    //}

    //[Authorize(Policy = "ApenasAluno")]
    //[HttpPost("{alunoId}/solicitar-certificado")]
    //public async Task<IActionResult> SolicitarCertificado(SolicitarCertificadoViewModel viewModel)
    //{
    //    if (!ModelState.IsValid) { return GenerateModelStateResponse(ResponseTypeEnum.ValidationError, HttpStatusCode.BadRequest, ModelState); }

    //    try
    //    {
    //        if (UserId != viewModel.AlunoId) { return GenerateResponse(null, ResponseTypeEnum.ValidationError, HttpStatusCode.Forbidden, ["Você não tem permissão para realizar essa operação"]); }

    //        var comando = new SolicitarCertificadoCommand(viewModel.AlunoId, viewModel.MatriculaCursoId, viewModel.PathCertificado);
    //        var sucesso = await _mediatorHandler.EnviarComando(comando);

    //        if (sucesso)
    //        {
    //            return GenerateResponse(new { viewModel.AlunoId, viewModel.MatriculaCursoId, viewModel.PathCertificado },
    //                responseType: ResponseTypeEnum.Success,
    //                statusCode: HttpStatusCode.Created);
    //        }

    //        return GenerateResponse(responseType: ResponseTypeEnum.GenericError, statusCode: HttpStatusCode.BadRequest);
    //    }
    //    catch (DomainException exDomain)
    //    {
    //        return GenerateDomainExceptionResponse(null, ResponseTypeEnum.DomainError, HttpStatusCode.BadRequest, exDomain);
    //    }
    //    catch (Exception ex)
    //    {
    //        return GenerateResponse(null, ResponseTypeEnum.GenericError, HttpStatusCode.BadRequest, [ex.Message]);
    //    }
    //}
}
