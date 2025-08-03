using BFF.API.Models.Request;
using BFF.API.Settings;
using BFF.Application.Interfaces.Services;
using BFF.Domain.DTOs;
using BFF.Infrastructure.Services;
using Core.Communication;
using Microsoft.Extensions.Options;

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

    public class ConteudoService : BaseApiService, IConteudoService
    {
        public ConteudoService(IOptions<ApiSettings> apiSettings, IRestApiService restApiService, ILogger<ConteudoService> logger) : base(restApiService, logger)
        {
            _restApiService.SetBaseAddress(apiSettings.Value.ConteudoApiUrl);
        }

        public async Task<ResponseResult?> ObterCursoPorId(Guid cursoId, string token)
        {
            if (!ValidateId(cursoId, nameof(ObterCursoPorId)) || !ValidateToken(token, nameof(ObterCursoPorId)))
                return null;

            return await ExecuteWithErrorHandling(async () =>
            {
                var headers = CreateAuthHeaders(token);
                return await _restApiService.GetAsync<ResponseResult>($"api/cursos/{cursoId}", headers);
            }, nameof(ObterCursoPorId), cursoId);
        }

        public async Task<ResponseResult?> ObterTodosCursos(string token)
        {
            if (!ValidateToken(token, nameof(ObterTodosCursos)))
                return null;

            return await ExecuteWithErrorHandling(async () =>
            {
                var headers = CreateAuthHeaders(token);
                return await _restApiService.GetAsync<ResponseResult>("api/cursos", headers);
            }, nameof(ObterTodosCursos));
        }

        public async Task<ResponseResult?> AdicionarCurso(CursoCriarRequest curso, string token)
        {
            if (!ValidateToken(token, nameof(AdicionarCurso)))
                return null;

            return await ExecuteWithErrorHandling(async () =>
            {
                var headers = CreateAuthHeaders(token);
                return await _restApiService.PostAsync<CursoCriarRequest, ResponseResult>("api/cursos", curso, headers);
            }, nameof(AdicionarCurso), curso.Nome);
        }

        public async Task<ResponseResult?> AtualizarCurso(Guid id, AtualizarCursoRequest curso, string token)
        {
            if (!ValidateId(id, nameof(AtualizarCurso)) || !ValidateToken(token, nameof(AtualizarCurso)))
                return null;

            return await ExecuteWithErrorHandling(async () =>
            {
                var headers = CreateAuthHeaders(token);
                return await _restApiService.PutAsync<AtualizarCursoRequest, ResponseResult>($"api/cursos/{id}", curso, headers);
            }, nameof(AtualizarCurso), id);
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
    }
}