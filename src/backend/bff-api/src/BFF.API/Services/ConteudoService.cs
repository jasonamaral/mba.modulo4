using BFF.API.Models.Request;
using BFF.API.Settings;
using BFF.Application.Interfaces.Services;
using BFF.Domain.DTOs;
using Core.Communication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace BFF.API.Services
{   
    public interface IConteudoService
    {
        Task<ResponseResult?> ObterCursoPorId(Guid cursoId, string token);
        Task<ResponseResult?> ObterTodosCursos(string token);
        Task<ResponseResult?> AdicionarCurso(CursoCriarRequest curso, string token);
        Task<ResponseResult?> AtualizarCurso(Guid id, AtualizarCursoRequest curso, string token);
        Task<ResponseResult> ExcluirCurso(Guid cursoId);
        Task<ResponseResult> AdicionarAula(Guid cursoId, AulaDto aula);
        Task<ResponseResult> AtualizarAula(Guid cursoId, AulaDto aula);
        Task<ResponseResult> ExcluirAula(Guid cursoId, Guid aulaId);
    }
    public class ConteudoService : IConteudoService
    {
        private readonly IHttpClientService _httpClientService;
        public ConteudoService(IOptions<ApiSettings> apiSettings, IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _httpClientService.SetBaseAddress(apiSettings.Value.ConteudoApiUrl);
        }

        public async Task<ResponseResult?> ObterCursoPorId(Guid cursoId, string token)
        {
            AdicionarToken(token);
            var response = await _httpClientService.GetAsync<ResponseResult>($"api/cursos/{cursoId}");
            return response;
        }

        public async Task<ResponseResult?> ObterTodosCursos(string token)
        {
            AdicionarToken(token);
            var response = await _httpClientService.GetAsync<ResponseResult>($"api/cursos");
            return response;
        }
        public async Task<ResponseResult?> AdicionarCurso(CursoCriarRequest curso, string token)
        {
            AdicionarToken(token);
            var response = await _httpClientService.PostAsync<CursoCriarRequest, ResponseResult>($"api/cursos", curso);
            return response;
        }

        public async Task<ResponseResult?> AtualizarCurso(Guid id, AtualizarCursoRequest curso, string token)
        {   
            AdicionarToken(token);
            var response = await _httpClientService.PutAsync<AtualizarCursoRequest, ResponseResult>($"api/cursos/{id}", curso);
            return response;
        }

        public Task<ResponseResult> ExcluirCurso(Guid cursoId)
        {   
            throw new NotImplementedException();
        }

        public Task<ResponseResult> AdicionarAula(Guid cursoId, AulaDto aula)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseResult> AtualizarAula(Guid cursoId, AulaDto aula)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseResult> ExcluirAula(Guid cursoId, Guid aulaId)
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
