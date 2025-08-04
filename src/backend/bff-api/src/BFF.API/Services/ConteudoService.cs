using BFF.API.Models.Request;
using BFF.API.Settings;
using BFF.Application.Interfaces.Services;
using BFF.Domain.DTOs;
using Core.Communication;
using Core.Communication.Filters;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace BFF.API.Services
{   
    public interface IConteudoService
    {
        Task<ResponseResult<CursoDto>> ObterCursoPorId(Guid cursoId, string token, bool includeAulas = false);
        Task<ResponseResult<PagedResult<CursoDto>>> ObterTodosCursos(string token, CursoFilter filter);
        Task<ResponseResult<IEnumerable<CursoDto>>> ObterPorCategoriaIdAsync(string token, Guid categoriaId, bool includeAulas = false);    
        Task<ResponseResult<Guid>> AdicionarCurso(CursoCriarRequest curso, string token);
        Task<ResponseResult<CursoDto>> AtualizarCurso(Guid id, AtualizarCursoRequest curso, string token);
        Task<ResponseResult<bool>> ExcluirCurso(Guid cursoId);
        Task<ResponseResult<Guid>> AdicionarAula(Guid cursoId, AulaDto aula);
        Task<ResponseResult<AulaDto>> AtualizarAula(Guid cursoId, AulaDto aula);
        Task<ResponseResult<bool>> ExcluirAula(Guid cursoId, Guid aulaId);
    }
    public class ConteudoService : IConteudoService
    {
        private readonly IHttpClientService _httpClientService;
        public ConteudoService(IOptions<ApiSettings> apiSettings, IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _httpClientService.SetBaseAddress(apiSettings.Value.ConteudoApiUrl);
        }

        public async Task<ResponseResult<CursoDto>> ObterCursoPorId(Guid cursoId, string token, bool includeAulas = false)
        {
            AdicionarToken(token);
            var response = await _httpClientService.GetAsync<ResponseResult<CursoDto>>($"api/cursos/{cursoId}?includeAulas={includeAulas}");
            return response;
        }

        public async Task<ResponseResult<PagedResult<CursoDto>>> ObterTodosCursos(string token, CursoFilter filter)
        {
            AdicionarToken(token);
            var response = await _httpClientService.GetAsync<ResponseResult<PagedResult<CursoDto>>>($"api/cursos?pageIndex={filter.PageIndex}&pageSize={filter.PageSize}&query={filter.Query}&includeAulas={filter.IncludeAulas}");
            return response;
        }
        public async Task<ResponseResult<IEnumerable<CursoDto>>> ObterPorCategoriaIdAsync(string token, Guid categoriaId, bool includeAulas = false)
        {
            AdicionarToken(token);
            var response = await _httpClientService.GetAsync<ResponseResult<IEnumerable<CursoDto>>>($"api/cursos/categoria/{categoriaId}?includeAulas={includeAulas}");
            return response;
        }
        public async Task<ResponseResult<Guid>> AdicionarCurso(CursoCriarRequest curso, string token)
        {
            AdicionarToken(token);
            var response = await _httpClientService.PostAsync<CursoCriarRequest, ResponseResult<Guid>>($"api/cursos", curso);
            return response;
        }

        public async Task<ResponseResult<CursoDto>> AtualizarCurso(Guid id, AtualizarCursoRequest curso, string token)
        {   
            AdicionarToken(token);
            var response = await _httpClientService.PutAsync<AtualizarCursoRequest, ResponseResult<CursoDto>>($"api/cursos/{id}", curso);
            return response;
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

        private void AdicionarToken(string token)
        {
            var _httpClient = _httpClientService.GetHttpClient();
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
