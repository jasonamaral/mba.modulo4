using Alunos.Application.DTOs;
using Alunos.Application.Interfaces.Repositories;
using Alunos.Application.Interfaces.Services;
using Alunos.Domain.Entities;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Alunos.Application.Services;

public class AlunoAppService : IAlunoAppService
{
    private readonly IAlunoRepository _alunoRepository;
    private readonly ILogger<AlunoAppService> _logger;

    public AlunoAppService(IAlunoRepository alunoRepository, ILogger<AlunoAppService> logger)
    {
        _alunoRepository = alunoRepository;
        _logger = logger;
    }

    public async Task<AlunoDto?> GetByIdAsync(Guid id)
    {
        var aluno = await _alunoRepository.GetByIdAsync(id);
        return aluno?.Adapt<AlunoDto>();
    }

    public async Task<AlunoDto?> ObterAlunoPorIdAsync(Guid id)
    {
        return await GetByIdAsync(id);
    }

    public async Task<AlunoDto?> GetByCodigoUsuarioAsync(Guid codigoUsuario)
    {
        var aluno = await _alunoRepository.GetByCodigoUsuarioAsync(codigoUsuario);
        return aluno?.Adapt<AlunoDto>();
    }

    public async Task<AlunoDto?> ObterAlunoPorCodigoUsuarioAsync(Guid codigoUsuario)
    {
        return await GetByCodigoUsuarioAsync(codigoUsuario);
    }

    public async Task<AlunoDto?> GetByEmailAsync(string email)
    {
        var aluno = await _alunoRepository.GetByEmailAsync(email);
        return aluno?.Adapt<AlunoDto>();
    }

    public async Task<IEnumerable<AlunoResumoDto>> GetAllAsync(bool includeMatriculas = false)
    {
        var alunos = await _alunoRepository.GetAllAsync(includeMatriculas);
        return alunos.Adapt<IEnumerable<AlunoResumoDto>>();
    }

    public async Task<IEnumerable<AlunoResumoDto>> ListarAlunosAsync(
        int pagina, int tamanhoPagina, string? filtro = null, string? ordenacao = null, string? direcao = null)
    {
        // Implementação simples - pode ser melhorada com paginação real
        var alunos = await _alunoRepository.GetAllAsync();

        if (!string.IsNullOrEmpty(filtro))
        {
            alunos = alunos.Where(a => a.Nome.Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                                     a.Email.Contains(filtro, StringComparison.OrdinalIgnoreCase));
        }

        return alunos.Adapt<IEnumerable<AlunoResumoDto>>();
    }

    public async Task<IEnumerable<AlunoResumoDto>> GetAlunosAtivosAsync(bool includeMatriculas = false)
    {
        var alunos = await _alunoRepository.GetAlunosAtivosAsync(includeMatriculas);
        return alunos.Adapt<IEnumerable<AlunoResumoDto>>();
    }

    public async Task<IEnumerable<AlunoResumoDto>> BuscarPorNomeAsync(string nome, bool includeMatriculas = false)
    {
        var alunos = await _alunoRepository.BuscarPorNomeAsync(nome, includeMatriculas);
        return alunos.Adapt<IEnumerable<AlunoResumoDto>>();
    }

    public async Task<AlunoPerfilDto?> GetPerfilAsync(Guid id)
    {
        var aluno = await _alunoRepository.GetByIdAsync(id);
        return aluno?.Adapt<AlunoPerfilDto>();
    }

    public async Task<AlunoPerfilDto?> ObterPerfilAlunoAsync(Guid id)
    {
        return await GetPerfilAsync(id);
    }

    public async Task<AlunoDashboardDto?> GetDashboardAsync(Guid id)
    {
        var aluno = await _alunoRepository.GetByIdAsync(id);
        return aluno?.Adapt<AlunoDashboardDto>();
    }

    public async Task<AlunoDashboardDto?> ObterDashboardAlunoAsync(Guid id)
    {
        return await GetDashboardAsync(id);
    }

    public async Task<AlunoEstatisticasDto?> GetEstatisticasAsync(Guid id)
    {
        var aluno = await _alunoRepository.GetByIdAsync(id);
        return aluno?.Adapt<AlunoEstatisticasDto>();
    }

    public async Task<AlunoEstatisticasDto?> ObterEstatisticasAlunoAsync(Guid id)
    {
        return await GetEstatisticasAsync(id);
    }

    public async Task<AlunoDto> CreateAsync(AlunoCadastroDto dto)
    {
        try
        {
            // Verificar se já existe um aluno com o mesmo email
            if (await ExisteEmailAsync(dto.Email))
            {
                throw new ArgumentException($"Já existe um aluno cadastrado com o email {dto.Email}");
            }

            // Mapear DTO para entidade usando Mapster
            var aluno = dto.Adapt<Aluno>();

            var createdAluno = await _alunoRepository.AddAsync(aluno);
            await _alunoRepository.SaveChangesAsync();
            return createdAluno.Adapt<AlunoDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar aluno");
            throw;
        }
    }

    public async Task<AlunoDto> CadastrarAlunoAsync(AlunoCadastroDto dto)
    {
        return await CreateAsync(dto);
    }

    public async Task<AlunoDto?> AtualizarAlunoAsync(Guid id, AlunoAtualizarDto dto)
    {
        var aluno = await _alunoRepository.GetByIdAsync(id);
        if (aluno == null) return null;

        // Mapear DTO para entidade usando Mapster
        dto.Adapt(aluno);

        var updatedAluno = await _alunoRepository.UpdateAsync(aluno);
        await _alunoRepository.SaveChangesAsync();
        return updatedAluno?.Adapt<AlunoDto>();
    }

    public async Task<bool> AtivarAlunoAsync(Guid id)
    {
        var aluno = await _alunoRepository.GetByIdAsync(id);
        if (aluno == null) return false;

        aluno.Ativar();
        await _alunoRepository.UpdateAsync(aluno);
        await _alunoRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DesativarAlunoAsync(Guid id)
    {
        var aluno = await _alunoRepository.GetByIdAsync(id);
        if (aluno == null) return false;

        aluno.Desativar();
        await _alunoRepository.UpdateAsync(aluno);
        await _alunoRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExcluirAlunoAsync(Guid id)
    {
        var aluno = await _alunoRepository.GetByIdAsync(id);
        if (aluno == null) return false;

        await _alunoRepository.DeleteByIdAsync(id);
        await _alunoRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> VerificarExistenciaAlunoAsync(Guid id)
    {
        var aluno = await _alunoRepository.GetByIdAsync(id);
        return aluno != null;
    }

    public async Task<IEnumerable<MatriculaDto>> ListarMatriculasAlunoAsync(Guid id, string? status = null)
    {
        // Implementação simplificada - retorna lista vazia por enquanto
        return new List<MatriculaDto>();
    }

    public async Task<IEnumerable<CertificadoDto>> ListarCertificadosAlunoAsync(Guid id)
    {
        // Implementação simplificada - retorna lista vazia por enquanto
        return new List<CertificadoDto>();
    }

    public async Task<AlunoDto> AtivarAsync(Guid id)
    {
        var aluno = await _alunoRepository.GetByIdAsync(id);
        if (aluno == null)
            throw new ArgumentException($"Aluno com ID {id} não encontrado");

        aluno.Ativar();
        var updatedAluno = await _alunoRepository.UpdateAsync(aluno);
        await _alunoRepository.SaveChangesAsync();
        return updatedAluno.Adapt<AlunoDto>();
    }

    public async Task<AlunoDto> DesativarAsync(Guid id)
    {
        var aluno = await _alunoRepository.GetByIdAsync(id);
        if (aluno == null)
            throw new ArgumentException($"Aluno com ID {id} não encontrado");

        aluno.Desativar();
        var updatedAluno = await _alunoRepository.UpdateAsync(aluno);
        await _alunoRepository.SaveChangesAsync();
        return updatedAluno.Adapt<AlunoDto>();
    }

    public async Task DeleteAsync(Guid id)
    {
        var aluno = await _alunoRepository.GetByIdAsync(id);
        if (aluno == null)
            throw new ArgumentException($"Aluno com ID {id} não encontrado");

        await _alunoRepository.DeleteByIdAsync(id);
        await _alunoRepository.SaveChangesAsync();
    }

    public async Task<MatriculaDto> MatricularAsync(Guid alunoId, MatriculaCadastroDto dto)
    {
        // Implementação simplificada
        throw new NotImplementedException("Método MatricularAsync não implementado");
    }

    public async Task<IEnumerable<MatriculaDto>> GetMatriculasAsync(Guid alunoId)
    {
        return await ListarMatriculasAlunoAsync(alunoId);
    }

    public async Task<IEnumerable<CertificadoDto>> GetCertificadosAsync(Guid alunoId)
    {
        return await ListarCertificadosAlunoAsync(alunoId);
    }

    public async Task<HistoricoAlunoPaginadoDto> GetHistoricoAsync(Guid alunoId, HistoricoAlunoFiltroDto filtro)
    {
        // Implementação simplificada
        throw new NotImplementedException("Método GetHistoricoAsync não implementado");
    }

    public async Task<bool> ExisteEmailAsync(string email, Guid? excluirId = null)
    {
        return await _alunoRepository.ExisteEmailAsync(email, excluirId);
    }

    public async Task<int> GetCountAsync()
    {
        return await _alunoRepository.GetCountAsync();
    }

    public async Task<int> GetCountAtivosAsync()
    {
        return await _alunoRepository.GetCountAtivosAsync();
    }

    public async Task<AlunoDto> UpdateAsync(Guid id, AlunoAtualizarDto dto)
    {
        var aluno = await _alunoRepository.GetByIdAsync(id);
        if (aluno == null)
            throw new ArgumentException($"Aluno com ID {id} não encontrado");

        // Verificar se o email já existe em outro aluno
        if (await ExisteEmailAsync(dto.Email, id))
        {
            throw new ArgumentException($"Já existe um aluno cadastrado com o email {dto.Email}");
        }

        // Mapear DTO para entidade usando Mapster
        dto.Adapt(aluno);

        var updatedAluno = await _alunoRepository.UpdateAsync(aluno);
        await _alunoRepository.SaveChangesAsync();
        return updatedAluno.Adapt<AlunoDto>();
    }
}