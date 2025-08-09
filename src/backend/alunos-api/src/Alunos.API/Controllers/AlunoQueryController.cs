using Alunos.Application.DTOs;
using Core.Communication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Alunos.API.Controllers;

public partial class AlunoController
{
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseResult<AlunoDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    public async Task<IActionResult> ObterAlunoPorId(Guid id)
    {
        try
        {
            var aluno = await _alunoQueryService.ObterAlunoPorIdAsync(id);
            if (aluno == null)
            {
                _notificador.AdicionarErro("Aluno não encontrado.");
                return RespostaPadraoApi<string>();
            }

            return RespostaPadraoApi(data: aluno);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    [HttpGet("{id}/evolucao")]
    [ProducesResponseType(typeof(ResponseResult<EvolucaoAlunoDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    public async Task<IActionResult> ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(Guid id)
    {
        try
        {
            var aluno = await _alunoQueryService.ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(id);
            if (aluno == null)
            {
                _notificador.AdicionarErro("Evolução da matricula do aluno não encontrado.");
                return RespostaPadraoApi<string>();
            }

            return RespostaPadraoApi(data: aluno);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    [HttpGet("{id}/todas-matriculas")]
    [ProducesResponseType(typeof(ResponseResult<ICollection<MatriculaCursoDto>>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    public async Task<IActionResult> ObterMatriculasPorAlunoId(Guid id)
    {
        try
        {
            var matriculas = await _alunoQueryService.ObterMatriculasPorAlunoIdAsync(id);
            if (matriculas == null || !matriculas.Any())
            {
                _notificador.AdicionarErro("Matrícula do aluno não encontrada.");
                return RespostaPadraoApi<string>();
            }

            return RespostaPadraoApi(data: matriculas);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    [HttpGet("matricula/{matriculaId}/certificado")]
    [ProducesResponseType(typeof(ResponseResult<CertificadoDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    public async Task<IActionResult> ObterCertificadoPorMatriculaId(Guid matriculaId)
    {
        try
        {
            var certificado = await _alunoQueryService.ObterCertificadoPorMatriculaIdAsync(matriculaId);
            if (certificado == null)
            {
                _notificador.AdicionarErro("Certificado não encontrado.");
                return RespostaPadraoApi<string>();
            }

            return RespostaPadraoApi(data: certificado);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    [HttpGet("aulas/{matriculaId}")]
    [ProducesResponseType(typeof(ResponseResult<ICollection<AulaCursoDto>>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    public async Task<IActionResult> ObterAulasPorMatriculaId(Guid matriculaId)
    {
        try
        {
            var aulas = await _alunoQueryService.ObterAulasPorMatriculaIdAsync(matriculaId);
            if (aulas == null)
            {
                _notificador.AdicionarErro("Aulas não encontradas não encontrado.");
                return RespostaPadraoApi<string>();
            }

            return RespostaPadraoApi(data: aulas);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }
}
