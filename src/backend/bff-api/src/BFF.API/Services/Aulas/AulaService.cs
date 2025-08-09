using BFF.API.Services.Conteudos;
using BFF.API.Settings;
using BFF.Application.Interfaces.Services;
using BFF.Domain.DTOs;
using BFF.Domain.DTOs.Alunos;
using BFF.Infrastructure.Services;
using Core.Communication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;

namespace BFF.API.Services.Aulas;
public class AulaService(IOptions<ApiSettings> apiSettings,
        IApiClientService apiClient,
        ILogger<AulaService> logger) : BaseApiService(apiClient, logger), IAulaService
{
    private readonly ApiSettings _apiSettings = apiSettings.Value;

    public async Task<ResponseResult<AlunoDto>> ObterAlunoPorIdAsync(Guid alunoId, string token)
    {
        if (!ValidateToken(token, nameof(ObterAlunoPorIdAsync)))
            return new ResponseResult<AlunoDto> { Status = 400, Errors = new ResponseErrorMessages { Mensagens = ["Token inválido"] } };

        var result = await ExecuteWithErrorHandling(async () =>
        {
            _apiClient.SetBaseAddress(_apiSettings.AlunosApiUrl);
            ConfigureAuthToken(token);

            var url = $"api/aluno/{alunoId}";
            return await _apiClient.GetAsync<ResponseResult<AlunoDto>>(url);
        }, nameof(ObterAlunoPorIdAsync), alunoId);

        return result ?? new ResponseResult<AlunoDto> { Status = 500, Errors = new ResponseErrorMessages { Mensagens = ["Erro interno do servidor"] } };
    }

    public async Task<ResponseResult<EvolucaoAlunoDto>> ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(Guid alunoId, string token)
    {
        if (!ValidateToken(token, nameof(ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync)))
            return new ResponseResult<EvolucaoAlunoDto> { Status = 400, Errors = new ResponseErrorMessages { Mensagens = ["Token inválido"] } };

        var result = await ExecuteWithErrorHandling(async () =>
        {
            _apiClient.SetBaseAddress(_apiSettings.AlunosApiUrl);
            ConfigureAuthToken(token);

            var url = $"api/aluno/{alunoId}/evolucao";
            return await _apiClient.GetAsync<ResponseResult<EvolucaoAlunoDto>>(url);
        }, nameof(ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync), alunoId);

        return result ?? new ResponseResult<EvolucaoAlunoDto> { Status = 500, Errors = new ResponseErrorMessages { Mensagens = ["Erro interno do servidor"] } };
    }

    public async Task<ResponseResult<ICollection<MatriculaCursoDto>>> ObterMatriculasPorAlunoIdAsync(Guid alunoId, string token)
    {
        if (!ValidateToken(token, nameof(ObterMatriculasPorAlunoIdAsync)))
            return new ResponseResult<ICollection<MatriculaCursoDto>> { Status = 400, Errors = new ResponseErrorMessages { Mensagens = ["Token inválido"] } };

        var result = await ExecuteWithErrorHandling(async () =>
        {
            _apiClient.SetBaseAddress(_apiSettings.AlunosApiUrl);
            ConfigureAuthToken(token);

            var url = $"api/aluno/{alunoId}/todas-matriculas";
            return await _apiClient.GetAsync<ResponseResult<ICollection<MatriculaCursoDto>>>(url);
        }, nameof(ObterMatriculasPorAlunoIdAsync), alunoId);

        return result ?? new ResponseResult<ICollection<MatriculaCursoDto>> { Status = 500, Errors = new ResponseErrorMessages { Mensagens = ["Erro interno do servidor"] } };
    }

    public async Task<ResponseResult<CertificadoDto>> ObterCertificadoPorMatriculaIdAsync(Guid matriculaId, string token)
    {
        if (!ValidateToken(token, nameof(ObterCertificadoPorMatriculaIdAsync)))
            return new ResponseResult<CertificadoDto> { Status = 400, Errors = new ResponseErrorMessages { Mensagens = ["Token inválido"] } };

        var result = await ExecuteWithErrorHandling(async () =>
        {
            _apiClient.SetBaseAddress(_apiSettings.AlunosApiUrl);
            ConfigureAuthToken(token);

            var url = $"api/aluno/matricula/{matriculaId}/certificado";
            return await _apiClient.GetAsync<ResponseResult<CertificadoDto>>(url);
        }, nameof(ObterCertificadoPorMatriculaIdAsync), matriculaId);

        return result ?? new ResponseResult<CertificadoDto> { Status = 500, Errors = new ResponseErrorMessages { Mensagens = ["Erro interno do servidor"] } };
    }

    public async Task<ResponseResult<ICollection<AulaCursoDto>>> ObterAulasPorMatriculaIdAsync(Guid matriculaId, string token)
    {
        if (!ValidateToken(token, nameof(ObterAulasPorMatriculaIdAsync)))
            return new ResponseResult<ICollection<AulaCursoDto>> { Status = 400, Errors = new ResponseErrorMessages { Mensagens = ["Token inválido"] } };

        // Obtenho a matrícula e aulas do aluno
        var aulaCursoDto = await ExecuteWithErrorHandling(async () =>
        {
            _apiClient.SetBaseAddress(_apiSettings.AlunosApiUrl);
            ConfigureAuthToken(token);

            var url = $"api/aulas/{matriculaId}";
            return await _apiClient.GetAsync<ResponseResult<ICollection<AulaCursoDto>>>(url);
        }, nameof(ObterAulasPorMatriculaIdAsync), matriculaId);

        // Obtenho o curso e suas aulas para "adicionar" as aulas que não estão na matrícula (por não ter histórico ou novas aulas)
        var cursoDto = await ExecuteWithErrorHandling(async () =>
        {
            _apiClient.SetBaseAddress(_apiSettings.ConteudoApiUrl);
            ConfigureAuthToken(token);

            var url = $"api/aulas/{matriculaId}";
            return await _apiClient.GetAsync<ResponseResult<CursoDto>>(url);
        }, nameof(ObterAulasPorMatriculaIdAsync), matriculaId);

        foreach (var aula in cursoDto.Data.Aulas)
        {
            var historico = aulaCursoDto.Data.FirstOrDefault(h => h.AulaId == aula.Id);

            if (historico == null)
            {
                aulaCursoDto.Data.Add(new AulaCursoDto
                {
                    AulaId = aula.Id,
                    CursoId = aula.CursoId,
                    NomeAula = aula.Nome,
                    OrdemAula = aula.Ordem,
                    //Ativo = aula.Ativo,
                    DataInicio = null,
                    DataTermino = null,
                    AulaJaIniciadaRealizada = false,
                    Url = aula.VideoUrl
                });
            }
        }

        return aulaCursoDto ?? new ResponseResult<ICollection<AulaCursoDto>> { Status = 500, Errors = new ResponseErrorMessages { Mensagens = ["Erro interno do servidor"] } };
    }
}
