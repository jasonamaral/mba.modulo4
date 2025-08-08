using BFF.Application.Interfaces.Services;
using BFF.Domain.DTOs;
using Microsoft.Extensions.Logging;

namespace BFF.Infrastructure.Services;

public class AlunoStoreService : IAlunoStoreService
{
    private readonly ICacheService _cache;
    private readonly ILogger<AlunoStoreService> _logger;

    public AlunoStoreService(ICacheService cache, ILogger<AlunoStoreService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    private static string KeyMatriculas(Guid alunoId) => $"aluno:{alunoId}:matriculas";
    private static string KeyCertificados(Guid alunoId) => $"aluno:{alunoId}:certificados";
    private static string KeyProgresso(Guid alunoId) => $"aluno:{alunoId}:progresso";

    public async Task<IReadOnlyList<MatriculaDto>> ListarMatriculasAsync(Guid alunoId)
    {
        return (await _cache.GetAsync<List<MatriculaDto>>(KeyMatriculas(alunoId))) ?? new List<MatriculaDto>();
    }

    public async Task<MatriculaDto?> ObterMatriculaAsync(Guid alunoId, Guid matriculaId)
    {
        var list = await ListarMatriculasAsync(alunoId);
        return list.FirstOrDefault(m => m.Id == matriculaId);
    }

    public async Task<MatriculaDto> CriarMatriculaAsync(Guid alunoId, Guid cursoId, string cursoNome)
    {
        var lista = (await ListarMatriculasAsync(alunoId)).ToList();
        var matricula = new MatriculaDto
        {
            Id = Guid.NewGuid(),
            AlunoId = alunoId,
            CursoId = cursoId,
            CursoNome = cursoNome,
            DataMatricula = DateTime.UtcNow,
            Status = "PendentePagamento",
            PercentualConclusao = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        lista.Add(matricula);
        await _cache.SetAsync(KeyMatriculas(alunoId), lista);
        return matricula;
    }

    public async Task<bool> AtualizarMatriculaAsync(Guid alunoId, MatriculaDto matricula)
    {
        var lista = (await ListarMatriculasAsync(alunoId)).ToList();
        var idx = lista.FindIndex(m => m.Id == matricula.Id);
        if (idx < 0) return false;
        matricula.UpdatedAt = DateTime.UtcNow;
        lista[idx] = matricula;
        await _cache.SetAsync(KeyMatriculas(alunoId), lista);
        return true;
    }

    public async Task<ProgressoGeralDto> ObterProgressoAsync(Guid alunoId)
    {
        return (await _cache.GetAsync<ProgressoGeralDto>(KeyProgresso(alunoId))) ?? new ProgressoGeralDto();
    }

    public async Task<bool> AtualizarProgressoAulaAsync(Guid alunoId, Guid cursoId, Guid aulaId, decimal percentualAcumulado)
    {
        // Atualiza progresso agregado simples por aluno
        var progresso = await ObterProgressoAsync(alunoId);
        progresso.CursosMatriculados = Math.Max(1, progresso.CursosMatriculados);
        progresso.PercentualConcluidoGeral = percentualAcumulado;
        await _cache.SetAsync(KeyProgresso(alunoId), progresso);

        // Atualiza matrícula específica
        var lista = (await ListarMatriculasAsync(alunoId)).ToList();
        var idx = lista.FindIndex(m => m.CursoId == cursoId);
        if (idx >= 0)
        {
            var m = lista[idx];
            m.PercentualConclusao = percentualAcumulado;
            if (percentualAcumulado >= 100 && m.Status != "Concluida")
            {
                m.Status = "Concluida";
                m.DataConclusao = DateTime.UtcNow;
            }
            m.UpdatedAt = DateTime.UtcNow;
            lista[idx] = m;
            await _cache.SetAsync(KeyMatriculas(alunoId), lista);
        }
        return true;
    }

    public async Task<IReadOnlyList<CertificadoDto>> ListarCertificadosAsync(Guid alunoId)
    {
        return (await _cache.GetAsync<List<CertificadoDto>>(KeyCertificados(alunoId))) ?? new List<CertificadoDto>();
    }

    public async Task<CertificadoDto> EmitirCertificadoAsync(Guid alunoId, Guid cursoId, string cursoNome)
    {
        var lista = (await ListarCertificadosAsync(alunoId)).ToList();
        var cert = new CertificadoDto
        {
            Id = Guid.NewGuid(),
            AlunoId = alunoId,
            CursoId = cursoId,
            CursoNome = cursoNome,
            DataEmissao = DateTime.UtcNow,
            CodigoVerificacao = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpperInvariant(),
            Url = $"https://certificados.local/{alunoId}/{cursoId}",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        lista.Add(cert);
        await _cache.SetAsync(KeyCertificados(alunoId), lista);
        return cert;
    }
}


