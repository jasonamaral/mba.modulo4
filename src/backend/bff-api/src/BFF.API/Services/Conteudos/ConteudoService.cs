using BFF.API.Models.Request;
using BFF.API.Settings;
using BFF.Application.Interfaces.Services;
using BFF.Domain.DTOs;
using BFF.Infrastructure.Services;
using Core.Communication;
using Core.Communication.Filters;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.WebUtilities;

namespace BFF.API.Services.Conteudos;

public partial class ConteudoService : BaseApiService, IConteudoService
{
    private readonly ApiSettings _apiSettings;

    public ConteudoService(
        IOptions<ApiSettings> apiSettings, 
        IApiClientService apiClient, 
        ILogger<ConteudoService> logger) : base(apiClient, logger)
    {
        _apiSettings = apiSettings.Value;
        _apiClient.SetBaseAddress(_apiSettings.ConteudoApiUrl);
    }

    public async Task<ResponseResult<CursoDto>> ObterCursoPorId(Guid cursoId, bool includeAulas = false)
    {
        var result = await ExecuteWithErrorHandling(async () =>
        {
            
            var url = includeAulas ? $"api/cursos/{cursoId}?includeAulas=true" : $"api/cursos/{cursoId}";
            return await _apiClient.GetAsync<ResponseResult<CursoDto>>(url);
        }, nameof(ObterCursoPorId), cursoId);

        return result ?? new ResponseResult<CursoDto> { Status = 500, Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Erro interno do servidor" } } };
    }

    public async Task<ResponseResult<PagedResult<CursoDto>>> ObterTodosCursos(CursoFilter filter)
    {
        var result = await ExecuteWithErrorHandling(async () =>
        {
            _apiClient.SetBaseAddress(_apiSettings.ConteudoApiUrl);

            var queryParams = new Dictionary<string, string>
            {
                [nameof(CursoFilter.PageSize)] = filter.PageSize > 0 ? filter.PageSize.ToString() : null,
                [nameof(CursoFilter.PageIndex)] = filter.PageIndex > 0 ? filter.PageIndex.ToString() : null,
                [nameof(CursoFilter.Query)] = string.IsNullOrWhiteSpace(filter.Query) ? null : filter.Query,
                [nameof(CursoFilter.IncludeAulas)] = filter.IncludeAulas.ToString().ToLowerInvariant(),
                [nameof(CursoFilter.Ativos)] = filter.Ativos.ToString().ToLowerInvariant()
            }!;

            var filteredParams = queryParams
                .Where(p => p.Value is not null)
                .ToDictionary(p => p.Key, p => p.Value);

            var url = QueryHelpers.AddQueryString("api/cursos", filteredParams);
            return await _apiClient.GetAsync<ResponseResult<PagedResult<CursoDto>>>(url);
        }, nameof(ObterTodosCursos));

        return result ?? new ResponseResult<PagedResult<CursoDto>>
        {
            Status = 500,
            Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Erro interno do servidor" } }
        };
    }
    public async Task<ResponseResult<ConteudoProgramaticoDto>> ObterConteudoProgramaticoPorCursoId(Guid cursoId, bool includeAulas = false)
    {
        var result = await ExecuteWithErrorHandling(async () =>
        {
            _apiClient.SetBaseAddress(_apiSettings.ConteudoApiUrl);
            
            return await _apiClient.GetAsync<ResponseResult<ConteudoProgramaticoDto>>($"api/cursos/{cursoId}/conteudo-programatico");
        }, nameof(ObterConteudoProgramaticoPorCursoId), cursoId);
        return result ?? new ResponseResult<ConteudoProgramaticoDto> { Status = 500, Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Erro interno do servidor" } } };
    }

    public async Task<ResponseResult<Guid>> AdicionarCurso(CursoCriarRequest curso)
    {
        var result = await ExecuteWithErrorHandling(async () =>
        {
            _apiClient.SetBaseAddress(_apiSettings.ConteudoApiUrl);
            
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
					var genericError = System.Text.Json.JsonSerializer.Deserialize<ResponseResult<object>>(apiResponse.ErrorContent,
						new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
					var mapped = new ResponseResult<Guid>
					{
						Title = genericError?.Title,
						Status = genericError?.Status ?? apiResponse.StatusCode,
						Errors = genericError?.Errors ?? new ResponseErrorMessages { Mensagens = new List<string> { "Erro ao interpretar resposta da API" } }
					};
					return mapped;
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

    public async Task<ResponseResult<CursoDto>> AtualizarCurso(Guid id, AtualizarCursoRequest curso)
    {
        var result = await ExecuteWithErrorHandling(async () =>
        {
            _apiClient.SetBaseAddress(_apiSettings.ConteudoApiUrl);
            
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
					var genericError = System.Text.Json.JsonSerializer.Deserialize<ResponseResult<object>>(apiResponse.ErrorContent,
						new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
					var mapped = new ResponseResult<CursoDto>
					{
						Title = genericError?.Title,
						Status = genericError?.Status ?? apiResponse.StatusCode,
						Errors = genericError?.Errors ?? new ResponseErrorMessages { Mensagens = new List<string> { "Erro ao interpretar resposta da API" } }
					};
					return mapped;
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

    public async Task<ResponseResult<bool>> ExcluirCurso(Guid cursoId)
    {
        var result = await ExecuteWithErrorHandling(async () =>
        {
            _apiClient.SetBaseAddress(_apiSettings.ConteudoApiUrl);
            var apiResponse = await _apiClient.DeleteAsync($"api/cursos/{cursoId}");
            if (apiResponse)
            {
                return new ResponseResult<bool> { Status = 200, Data = true };
            }
            return new ResponseResult<bool> 
            { 
                Status = 400, 
                Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Erro ao excluir o curso" } } 
            };
        }, nameof(ExcluirCurso), cursoId);
        return result ?? new ResponseResult<bool> { Status = 500, Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Erro interno do servidor" } } };
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

    public async Task<ResponseResult<IEnumerable<CursoDto>>> ObterPorCategoriaId(Guid categoriaId, bool includeAulas = false)
    {
        var result = await ExecuteWithErrorHandling(async () =>
        {
            _apiClient.SetBaseAddress(_apiSettings.ConteudoApiUrl);
            var url = includeAulas ? $"api/cursos/categoria/{categoriaId}?includeAulas=true" : $"api/cursos/categoria/{categoriaId}";
            return await _apiClient.GetAsync<ResponseResult<IEnumerable<CursoDto>>>(url);
        }, nameof(ObterPorCategoriaId), categoriaId);
        return result ?? new ResponseResult<IEnumerable<CursoDto>> { Status = 500, Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Erro interno do servidor" } } };
    }

    public async Task<ResponseResult<IEnumerable<CategoriaDto>>> ObterTodasCategorias()
    {
        var result = await ExecuteWithErrorHandling(async () =>
        {
            _apiClient.SetBaseAddress(_apiSettings.ConteudoApiUrl);
            var url = $"api/categoria";
            return await _apiClient.GetAsync<ResponseResult<IEnumerable<CategoriaDto>>>(url);
        }, nameof(ObterPorCategoriaId));
        return result ?? new ResponseResult<IEnumerable<CategoriaDto>> { Status = 500, Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Erro interno do servidor" } } };
    }
}