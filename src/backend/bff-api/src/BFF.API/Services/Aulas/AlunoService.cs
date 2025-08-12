using BFF.API.Models.Request;
using BFF.API.Services.Conteudos;
using BFF.API.Settings;
using BFF.Application.Interfaces.Services;
using BFF.Domain.DTOs;
using BFF.Domain.DTOs.Alunos.Request;
using BFF.Domain.DTOs.Alunos.Response;
using BFF.Infrastructure.Services;
using Core.Communication;
using Microsoft.Extensions.Options;
using System.Net;

namespace BFF.API.Services.Aulas;
public class AlunoService : BaseApiService, IAlunoService
{
    private readonly ApiSettings _apiSettings;
    private readonly IConteudoService _conteudoService;

    public AlunoService(IOptions<ApiSettings> apiSettings,
        IConteudoService conteudoService,
        IApiClientService apiClient,
        ILogger<AlunoService> logger) : base(apiClient, logger)
    {
        _apiSettings = apiSettings.Value;
        _conteudoService = conteudoService;

        _apiClient.SetBaseAddress(_apiSettings.AlunosApiUrl);
    }

    #region Gets
    public async Task<ResponseResult<AlunoDto>> ObterAlunoPorIdAsync(Guid alunoId)
    {
        //if (!ValidateToken(token, nameof(ObterAlunoPorIdAsync)))
        //{
        //    return new ResponseResult<AlunoDto> { Status = 400, Errors = new ResponseErrorMessages { Mensagens = ["Token inválido"] } };
        //}

        var result = await ExecuteWithErrorHandling(async () =>
        {
            //_apiClient.SetBaseAddress(_apiSettings.AlunosApiUrl);
            //ConfigureAuthToken(token);

            var url = $"api/aluno/{alunoId}";
            return await _apiClient.GetAsync<ResponseResult<AlunoDto>>(url);
        }, nameof(ObterAlunoPorIdAsync), alunoId);

        return result ?? new ResponseResult<AlunoDto> { Status = 500, Errors = new ResponseErrorMessages { Mensagens = ["Erro interno do servidor"] } };
    }

    public async Task<ResponseResult<EvolucaoAlunoDto>> ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(Guid alunoId)
    {
        //if (!ValidateToken(token, nameof(ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync)))
        //{
        //    return new ResponseResult<EvolucaoAlunoDto> { Status = 400, Errors = new ResponseErrorMessages { Mensagens = ["Token inválido"] } };
        //}

        var result = await ExecuteWithErrorHandling(async () =>
        {
            //_apiClient.SetBaseAddress(_apiSettings.AlunosApiUrl);
            //ConfigureAuthToken(token);

            var url = $"api/aluno/{alunoId}/evolucao";
            return await _apiClient.GetAsync<ResponseResult<EvolucaoAlunoDto>>(url);
        }, nameof(ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync), alunoId);

        return result ?? new ResponseResult<EvolucaoAlunoDto> { Status = 500, Errors = new ResponseErrorMessages { Mensagens = ["Erro interno do servidor"] } };
    }

    public async Task<ResponseResult<ICollection<MatriculaCursoDto>>> ObterMatriculasPorAlunoIdAsync(Guid alunoId)
    {
        //if (!ValidateToken(token, nameof(ObterMatriculasPorAlunoIdAsync)))
        //{
        //    return new ResponseResult<ICollection<MatriculaCursoDto>> { Status = 400, Errors = new ResponseErrorMessages { Mensagens = ["Token inválido"] } };
        //}

        var result = await ExecuteWithErrorHandling(async () =>
        {
            //_apiClient.SetBaseAddress(_apiSettings.AlunosApiUrl);
            //ConfigureAuthToken(token);

            var url = $"api/aluno/{alunoId}/todas-matriculas";
            return await _apiClient.GetAsync<ResponseResult<ICollection<MatriculaCursoDto>>>(url);
        }, nameof(ObterMatriculasPorAlunoIdAsync), alunoId);

        return result ?? new ResponseResult<ICollection<MatriculaCursoDto>> { Status = 500, Errors = new ResponseErrorMessages { Mensagens = ["Erro interno do servidor"] } };
    }

    public async Task<ResponseResult<CertificadoDto>> ObterCertificadoPorMatriculaIdAsync(Guid matriculaId)
    {
        //if (!ValidateToken(token, nameof(ObterCertificadoPorMatriculaIdAsync)))
        //{
        //    return new ResponseResult<CertificadoDto> { Status = 400, Errors = new ResponseErrorMessages { Mensagens = ["Token inválido"] } };
        //}

        var result = await ExecuteWithErrorHandling(async () =>
        {
            //_apiClient.SetBaseAddress(_apiSettings.AlunosApiUrl);
            //ConfigureAuthToken(token);

            var url = $"api/aluno/matricula/{matriculaId}/certificado";
            return await _apiClient.GetAsync<ResponseResult<CertificadoDto>>(url);
        }, nameof(ObterCertificadoPorMatriculaIdAsync), matriculaId);

        return result ?? new ResponseResult<CertificadoDto> { Status = 500, Errors = new ResponseErrorMessages { Mensagens = ["Erro interno do servidor"] } };
    }

    public async Task<ResponseResult<ICollection<AulaCursoDto>>> ObterAulasPorMatriculaIdAsync(Guid matriculaId)
    {
        //if (!ValidateToken(token, nameof(ObterAulasPorMatriculaIdAsync)))
        //{
        //    return new ResponseResult<ICollection<AulaCursoDto>> { Status = 400, Errors = new ResponseErrorMessages { Mensagens = ["Token inválido"] } };
        //}

        // Obtenho a matrícula e aulas do aluno
        var aulaCursoDto = await ExecuteWithErrorHandling(async () =>
        {
            //_apiClient.SetBaseAddress(_apiSettings.AlunosApiUrl);
            //ConfigureAuthToken(token);

            var url = $"api/aluno/aulas/{matriculaId}";
            return await _apiClient.GetAsync<ResponseResult<ICollection<AulaCursoDto>>>(url);
        }, nameof(ObterAulasPorMatriculaIdAsync), matriculaId);

        // Obtenho o curso e suas aulas para "adicionar" as aulas que não estão na matrícula (por não ter histórico ou novas aulas)
        var cursoDto = await _conteudoService.ObterCursoPorId(matriculaId, includeAulas: true);

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
    #endregion

    #region Posts and Puts
    public async Task<ResponseResult<Guid>> MatricularAlunoAsync(MatriculaCursoRequest dto)
    {
        //if (!ValidateToken(token, nameof(MatricularAlunoAsync)))
        //{
        //    return new ResponseResult<Guid> { Status = 400, Errors = new ResponseErrorMessages { Mensagens = ["Token inválido"] } };
        //}

        var cursoDto = await _conteudoService.ObterCursoPorId(dto.CursoId, includeAulas: false);
        if (cursoDto == null || cursoDto.Data == null)
        {
            return new ResponseResult<Guid> { Status = 404, Errors = new ResponseErrorMessages { Mensagens = ["Curso não encontrado"] } };
        }

        var matriculaCursoApi = new MatriculaCursoApiRequest()
        {
            AlunoId = dto.AlunoId,
            CursoId = cursoDto.Data.Id,
            CursoDisponivel = cursoDto.Data.PodeSerMatriculado,
            Nome = cursoDto.Data.Nome,
            Valor = cursoDto.Data.Valor,
            Observacao = dto.Observacao
        };

        var result = await ExecuteWithErrorHandling(async () =>
        {
            //_apiClient.SetBaseAddress(_apiSettings.AlunosApiUrl);
            //ConfigureAuthToken(token);

            var apiResponse = await _apiClient.PostAsyncWithDetails<MatriculaCursoApiRequest, ResponseResult<Guid>>($"api/aluno/{dto.AlunoId}/matricular-aluno", matriculaCursoApi);
            if (apiResponse.IsSuccess) { return apiResponse.Data; }

            // Se não foi sucesso, criar um ResponseResult com o erro da API chamada
            if (!string.IsNullOrEmpty(apiResponse.ErrorContent))
            {
                try
                {
                    var errorResponse = System.Text.Json.JsonSerializer.Deserialize<ResponseResult<Guid>>(apiResponse.ErrorContent);
                    return errorResponse;
                }
                catch
                {
                    return new ResponseResult<Guid>
                    {
                        Status = apiResponse.StatusCode,
                        Errors = new ResponseErrorMessages { Mensagens = [apiResponse.ErrorContent] }
                    };
                }
            }

            return new ResponseResult<Guid>
            {
                Status = apiResponse.StatusCode,
                Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Erro desconhecido na API" } }
            };
        }, nameof(MatricularAlunoAsync), dto.AlunoId);

        return result ?? new ResponseResult<Guid> { Status = 500, Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Erro interno do servidor" } } };
    }

    public async Task<ResponseResult<bool>> RegistrarHistoricoAprendizadoAsync(RegistroHistoricoAprendizadoRequest dto)
    {
        //if (!ValidateToken(token, nameof(RegistrarHistoricoAprendizadoAsync)))
        //{
        //    return new ResponseResult<bool> { Status = 400, Errors = new ResponseErrorMessages { Mensagens = ["Token inválido"] } };
        //}

        var matriculaCurso = await ObterMatriculasPorAlunoIdAsync(dto.AlunoId);
        if (matriculaCurso == null || matriculaCurso.Data == null || !matriculaCurso.Data.Any(x => x.Id == dto.MatriculaCursoId))
        {
            return new ResponseResult<bool> { Status = 400, Errors = new ResponseErrorMessages { Mensagens = ["Matrícula não encontrada"] } };
        }

        // TODO :: Isso está feio demais. Refatorar urgente! Deve ter um endpoint na AlunoApi para obter a matrícula por ID
        var cursoDto = await _conteudoService.ObterCursoPorId(matriculaCurso.Data.FirstOrDefault(x => x.Id == dto.MatriculaCursoId).CursoId, includeAulas: false);
        if (cursoDto == null || cursoDto.Data == null)
        {
            return new ResponseResult<bool> { Status = 404, Errors = new ResponseErrorMessages { Mensagens = ["Curso não encontrado"] } };
        }

        var historicoAprendizado = new RegistroHistoricoAprendizadoApiRequest
        {
            AlunoId = dto.AlunoId,
            MatriculaCursoId = dto.MatriculaCursoId,
            AulaId = dto.AulaId,
            NomeAula = cursoDto.Data.Nome,
            // TODO :: Verificar se a duração está correta, pois o DTO é em minutos e o API espera em horas
            DuracaoMinutos = (byte)cursoDto.Data.DuracaoHoras,
            DataTermino = dto.DataTermino
        };

        var result = await ExecuteWithErrorHandling(async () =>
        {
            //_apiClient.SetBaseAddress(_apiSettings.AlunosApiUrl);
            //ConfigureAuthToken(token);

            var apiResponse = await _apiClient.PostAsyncWithDetails<RegistroHistoricoAprendizadoApiRequest, ResponseResult<bool>>($"api/aluno/{dto.AlunoId}/registrar-historico-aprendizado", historicoAprendizado);
            if (apiResponse.IsSuccess) { return apiResponse.Data; }

            // Se não foi sucesso, criar um ResponseResult com o erro da API chamada
            if (!string.IsNullOrEmpty(apiResponse.ErrorContent))
            {
                try
                {
                    var errorResponse = System.Text.Json.JsonSerializer.Deserialize<ResponseResult<bool>>(apiResponse.ErrorContent);
                    return errorResponse;
                }
                catch
                {
                    return new ResponseResult<bool>
                    {
                        Status = apiResponse.StatusCode,
                        Errors = new ResponseErrorMessages { Mensagens = [apiResponse.ErrorContent] }
                    };
                }
            }

            return new ResponseResult<bool>
            {
                Status = apiResponse.StatusCode,
                Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Erro desconhecido na API" } }
            };
        }, nameof(RegistrarHistoricoAprendizadoAsync), dto.AlunoId);

        return result ?? new ResponseResult<bool> { Status = 500, Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Erro interno do servidor" } } };
    }

    public async Task<ResponseResult<bool>> ConcluirCursoAsync(ConcluirCursoRequest dto)
    {
        //if (!ValidateToken(token, nameof(ConcluirCursoAsync)))
        //{
        //    return new ResponseResult<bool> { Status = 400, Errors = new ResponseErrorMessages { Mensagens = ["Token inválido"] } };
        //}

        var matriculaCurso = await ObterMatriculasPorAlunoIdAsync(dto.AlunoId);
        if (matriculaCurso == null || matriculaCurso.Data == null || !matriculaCurso.Data.Any(x => x.Id == dto.MatriculaCursoId))
        {
            return new ResponseResult<bool> { Status = 400, Errors = new ResponseErrorMessages { Mensagens = ["Matrícula não encontrada"] } };
        }
        
        // TODO :: Isso está feio demais. Refatorar urgente! Deve ter um endpoint na AlunoApi para obter a matrícula por ID
        var cursoDto = await _conteudoService.ObterCursoPorId(matriculaCurso.Data.FirstOrDefault(x => x.Id == dto.MatriculaCursoId).CursoId, includeAulas: false);
        if (cursoDto == null || cursoDto.Data == null)
        {
            return new ResponseResult<bool> { Status = 404, Errors = new ResponseErrorMessages { Mensagens = ["Curso não encontrado"] } };
        }

        var concluirCurso = new ConcluirCursoApiRequest
        {
            AlunoId = dto.AlunoId,
            MatriculaCursoId = dto.MatriculaCursoId,
            CursoDto = cursoDto.Data
        };

        var result = await ExecuteWithErrorHandling(async () =>
        {
            //_apiClient.SetBaseAddress(_apiSettings.AlunosApiUrl);
            //ConfigureAuthToken(token);

            var apiResponse = await _apiClient.PutAsyncWithDetails<ConcluirCursoApiRequest, ResponseResult<bool>>($"api/aluno/{dto.AlunoId}/concluir-curso", concluirCurso);
            if (apiResponse.IsSuccess) { return apiResponse.Data; }

            // Se não foi sucesso, criar um ResponseResult com o erro da API chamada
            if (!string.IsNullOrEmpty(apiResponse.ErrorContent))
            {
                try
                {
                    var errorResponse = System.Text.Json.JsonSerializer.Deserialize<ResponseResult<bool>>(apiResponse.ErrorContent);
                    return errorResponse;
                }
                catch
                {
                    return new ResponseResult<bool>
                    {
                        Status = apiResponse.StatusCode,
                        Errors = new ResponseErrorMessages { Mensagens = [apiResponse.ErrorContent] }
                    };
                }
            }

            return new ResponseResult<bool>
            {
                Status = apiResponse.StatusCode,
                Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Erro desconhecido na API" } }
            };
        }, nameof(ConcluirCursoAsync), dto.AlunoId);

        return result ?? new ResponseResult<bool> { Status = 500, Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Erro interno do servidor" } } };
    }

    public async Task<ResponseResult<Guid>> SolicitarCertificadoAsync(SolicitaCertificadoRequest dto)
    {
        //if (!ValidateToken(token, nameof(SolicitarCertificadoAsync)))
        //{
        //    return new ResponseResult<Guid> { Status = 400, Errors = new ResponseErrorMessages { Mensagens = ["Token inválido"] } };
        //}

        var result = await ExecuteWithErrorHandling(async () =>
        {
            //_apiClient.SetBaseAddress(_apiSettings.AlunosApiUrl);
            //ConfigureAuthToken(token);

            var apiResponse = await _apiClient.PostAsyncWithDetails<SolicitaCertificadoRequest, ResponseResult<Guid>>($"api/aluno/{dto.AlunoId}/solicitar-certificado", dto);
            if (apiResponse.IsSuccess) { return apiResponse.Data; }

            // Se não foi sucesso, criar um ResponseResult com o erro da API chamada
            if (!string.IsNullOrEmpty(apiResponse.ErrorContent))
            {
                try
                {
                    var errorResponse = System.Text.Json.JsonSerializer.Deserialize<ResponseResult<Guid>>(apiResponse.ErrorContent);
                    return errorResponse;
                }
                catch
                {
                    return new ResponseResult<Guid>
                    {
                        Status = apiResponse.StatusCode,
                        Errors = new ResponseErrorMessages { Mensagens = [apiResponse.ErrorContent] }
                    };
                }
            }

            return new ResponseResult<Guid>
            {
                Status = apiResponse.StatusCode,
                Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Erro desconhecido na API" } }
            };
        }, nameof(SolicitarCertificadoAsync), dto.AlunoId);

        return result ?? new ResponseResult<Guid> { Status = 500, Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Erro interno do servidor" } } };
    }
    #endregion
}
