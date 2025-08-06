using BFF.API.Models.Request;
using BFF.API.Settings;
using BFF.Application.Interfaces.Services;
using BFF.Domain.DTOs;
using BFF.Infrastructure.Services;
using Core.Communication;
using Core.Communication.Filters;
using Microsoft.Extensions.Options;

namespace BFF.API.Services;

public class ConteudoService : BaseApiService, IConteudoService
{
    private readonly ApiSettings _apiSettings;

    public ConteudoService(
        IOptions<ApiSettings> apiSettings, 
        IApiClientService apiClient, 
        ILogger<ConteudoService> logger) : base(apiClient, logger)
    {
        _apiSettings = apiSettings.Value;
    }

    public async Task<ResponseResult<CursoDto>> ObterCursoPorId(Guid cursoId, string token, bool includeAulas = false)
    {
        if (!ValidateToken(token, nameof(ObterCursoPorId)))
            return new ResponseResult<CursoDto> { Status = 400, Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Token inválido" } } };

        var result = await ExecuteWithErrorHandling(async () =>
        {
            _apiClient.SetBaseAddress(_apiSettings.ConteudoApiUrl);
            ConfigureAuthToken(token);
            
            var url = includeAulas ? $"api/cursos/{cursoId}?includeAulas=true" : $"api/cursos/{cursoId}";
            return await _apiClient.GetAsync<ResponseResult<CursoDto>>(url);
        }, nameof(ObterCursoPorId), cursoId);

        return result ?? new ResponseResult<CursoDto> { Status = 500, Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Erro interno do servidor" } } };
    }

    public async Task<ResponseResult<PagedResult<CursoDto>>> ObterTodosCursos(string token, CursoFilter filter)
    {
        if (!ValidateToken(token, nameof(ObterTodosCursos)))
            return new ResponseResult<PagedResult<CursoDto>> { Status = 400, Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Token inválido" } } };

        var result = await ExecuteWithErrorHandling(async () =>
        {
            _apiClient.SetBaseAddress(_apiSettings.ConteudoApiUrl);
            ConfigureAuthToken(token);
            
            return await _apiClient.GetAsync<ResponseResult<PagedResult<CursoDto>>>("api/cursos");
        }, nameof(ObterTodosCursos));

        return result ?? new ResponseResult<PagedResult<CursoDto>> { Status = 500, Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Erro interno do servidor" } } };
    }

    public async Task<ResponseResult<Guid>> AdicionarCurso(CursoCriarRequest curso, string token)
    {
        if (!ValidateToken(token, nameof(AdicionarCurso)))
            return new ResponseResult<Guid> { Status = 400, Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Token inválido" } } };

        var result = await ExecuteWithErrorHandling(async () =>
        {
            _apiClient.SetBaseAddress(_apiSettings.ConteudoApiUrl);
            ConfigureAuthToken(token);
            
            var apiResponse = await _apiClient.PostAsyncWithDetails<CursoCriarRequest, ResponseResult<Guid>>("api/cursos", curso);
            
            if (apiResponse.IsSuccess)
            {
                return apiResponse.Data;
            }
            
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
                        Errors = new ResponseErrorMessages { Mensagens = new List<string> { apiResponse.ErrorContent } } 
                    };
                }
            }
            
            return new ResponseResult<Guid> 
            { 
                Status = apiResponse.StatusCode, 
                Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Erro desconhecido na API" } } 
            };
        }, nameof(AdicionarCurso), curso.Nome);

        return result ?? new ResponseResult<Guid> { Status = 500, Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Erro interno do servidor" } } };
    }

    public async Task<ResponseResult<CursoDto>> AtualizarCurso(Guid id, AtualizarCursoRequest curso, string token)
    {
        if (!ValidateToken(token, nameof(AtualizarCurso)))
            return new ResponseResult<CursoDto> { Status = 400, Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Token inválido" } } };

        var result = await ExecuteWithErrorHandling(async () =>
        {
            _apiClient.SetBaseAddress(_apiSettings.ConteudoApiUrl);
            ConfigureAuthToken(token);
            
            var apiResponse = await _apiClient.PutAsyncWithDetails<AtualizarCursoRequest, ResponseResult<CursoDto>>($"api/cursos/{id}", curso);
            
            if (apiResponse.IsSuccess)
            {
                return apiResponse.Data;
            }
            
            // Se não foi sucesso, criar um ResponseResult com o erro da API chamada
            if (!string.IsNullOrEmpty(apiResponse.ErrorContent))
            {
                try
                {
                    var errorResponse = System.Text.Json.JsonSerializer.Deserialize<ResponseResult<CursoDto>>(apiResponse.ErrorContent);
                    return errorResponse;
                }
                catch
                {
                    return new ResponseResult<CursoDto> 
                    { 
                        Status = apiResponse.StatusCode, 
                        Errors = new ResponseErrorMessages { Mensagens = new List<string> { apiResponse.ErrorContent } } 
                    };
                }
            }
            
            return new ResponseResult<CursoDto> 
            { 
                Status = apiResponse.StatusCode, 
                Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Erro desconhecido na API" } } 
            };
        }, nameof(AtualizarCurso), id);

        return result ?? new ResponseResult<CursoDto> { Status = 500, Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Erro interno do servidor" } } };
    }

    public Task<ResponseResult<bool>> ExcluirCurso(Guid cursoId)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseResult<Guid>> AdicionarAula(Guid cursoId, AulaDto aula)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseResult<AulaDto>> AtualizarAula(Guid cursoId, AulaDto aula)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseResult<bool>> ExcluirAula(Guid cursoId, Guid aulaId)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseResult<IEnumerable<CursoDto>>> ObterPorCategoriaIdAsync(string token, Guid categoriaId, bool includeAulas = false)
    {
        throw new NotImplementedException();
    }
}