using Alunos.Application.DTOs;
using Alunos.Application.Interfaces.Repositories;
using Alunos.Application.Interfaces.Services;
using Alunos.Domain.Entities;
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
        return aluno != null ? MapToDto(aluno) : null;
    }

    public async Task<AlunoDto?> ObterAlunoPorIdAsync(Guid id)
    {
        return await GetByIdAsync(id);
    }

    public async Task<AlunoDto?> GetByCodigoUsuarioAsync(Guid codigoUsuario)
    {
        var aluno = await _alunoRepository.GetByCodigoUsuarioAsync(codigoUsuario);
        return aluno != null ? MapToDto(aluno) : null;
    }

    public async Task<AlunoDto?> ObterAlunoPorCodigoUsuarioAsync(Guid codigoUsuario)
    {
        return await GetByCodigoUsuarioAsync(codigoUsuario);
    }

    public async Task<AlunoDto?> GetByEmailAsync(string email)
    {
        var aluno = await _alunoRepository.GetByEmailAsync(email);
        return aluno != null ? MapToDto(aluno) : null;
    }

    public async Task<IEnumerable<AlunoResumoDto>> GetAllAsync(bool includeMatriculas = false)
    {
        var alunos = await _alunoRepository.GetAllAsync(includeMatriculas);
        return alunos.Select(MapToResumoDto);
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

        return alunos.Select(MapToResumoDto);
    }

    public async Task<IEnumerable<AlunoResumoDto>> GetAlunosAtivosAsync(bool includeMatriculas = false)
    {
        var alunos = await _alunoRepository.GetAlunosAtivosAsync(includeMatriculas);
        return alunos.Select(MapToResumoDto);
    }

    public async Task<IEnumerable<AlunoResumoDto>> BuscarPorNomeAsync(string nome, bool includeMatriculas = false)
    {
        var alunos = await _alunoRepository.BuscarPorNomeAsync(nome, includeMatriculas);
        return alunos.Select(MapToResumoDto);
    }

    public async Task<AlunoPerfilDto?> GetPerfilAsync(Guid id)
    {
        var aluno = await _alunoRepository.GetByIdAsync(id);
        return aluno != null ? MapToPerfilDto(aluno) : null;
    }

    public async Task<AlunoPerfilDto?> ObterPerfilAlunoAsync(Guid id)
    {
        return await GetPerfilAsync(id);
    }

    public async Task<AlunoDashboardDto?> GetDashboardAsync(Guid id)
    {
        var aluno = await _alunoRepository.GetByIdAsync(id);
        return aluno != null ? MapToDashboardDto(aluno) : null;
    }

    public async Task<AlunoDashboardDto?> ObterDashboardAlunoAsync(Guid id)
    {
        return await GetDashboardAsync(id);
    }

    public async Task<AlunoEstatisticasDto?> GetEstatisticasAsync(Guid id)
    {
        var aluno = await _alunoRepository.GetByIdAsync(id);
        return aluno != null ? MapToEstatisticasDto(aluno) : null;
    }

    public async Task<AlunoEstatisticasDto?> ObterEstatisticasAlunoAsync(Guid id)
    {
        return await GetEstatisticasAsync(id);
    }

    public async Task<AlunoDto> CreateAsync(AlunoCadastroDto dto)
    {
        var aluno = new Aluno(
            Guid.NewGuid(), // Será substituído pelo ID real do usuário autenticado
            dto.Nome,
            dto.Email,
            dto.DataNascimento,
            dto.Telefone ?? "",
            dto.Genero ?? "",
            dto.Cidade ?? "",
            dto.Estado ?? "",
            dto.CEP ?? ""
        );

        await _alunoRepository.AddAsync(aluno);
        await _alunoRepository.SaveChangesAsync();

        return MapToDto(aluno);
    }

    public async Task<AlunoDto> CadastrarAlunoAsync(AlunoCadastroDto dto)
    {
        return await CreateAsync(dto);
    }



    public async Task<AlunoDto?> AtualizarAlunoAsync(Guid id, AlunoAtualizarDto dto)
    {
        try
        {
            return await UpdateAsync(id, dto);
        }
        catch (ArgumentException)
        {
            return null;
        }
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

        await _alunoRepository.DeleteAsync(aluno);
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
        // Por enquanto retorna lista vazia - implementação futura
        return new List<MatriculaDto>();
    }

    public async Task<IEnumerable<CertificadoDto>> ListarCertificadosAlunoAsync(Guid id)
    {
        // Por enquanto retorna lista vazia - implementação futura
        return new List<CertificadoDto>();
    }

    // Métodos adicionais da interface
    public async Task<AlunoDto> AtivarAsync(Guid id)
    {
        var aluno = await _alunoRepository.GetByIdAsync(id);
        if (aluno == null) throw new ArgumentException("Aluno não encontrado");

        aluno.Ativar();
        await _alunoRepository.UpdateAsync(aluno);
        await _alunoRepository.SaveChangesAsync();

        return MapToDto(aluno);
    }

    public async Task<AlunoDto> DesativarAsync(Guid id)
    {
        var aluno = await _alunoRepository.GetByIdAsync(id);
        if (aluno == null) throw new ArgumentException("Aluno não encontrado");

        aluno.Desativar();
        await _alunoRepository.UpdateAsync(aluno);
        await _alunoRepository.SaveChangesAsync();

        return MapToDto(aluno);
    }

    public async Task DeleteAsync(Guid id)
    {
        var aluno = await _alunoRepository.GetByIdAsync(id);
        if (aluno == null) throw new ArgumentException("Aluno não encontrado");

        await _alunoRepository.DeleteAsync(aluno);
        await _alunoRepository.SaveChangesAsync();
    }

    public async Task<MatriculaDto> MatricularAsync(Guid alunoId, MatriculaCadastroDto dto)
    {
        // Implementação futura
        throw new NotImplementedException("Funcionalidade de matrícula não implementada ainda");
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
        // Implementação futura - retorna histórico vazio por enquanto
        return new HistoricoAlunoPaginadoDto();
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
        if (aluno == null) throw new ArgumentException("Aluno não encontrado");

        aluno.AtualizarDados(
            dto.Nome,
            dto.Email,
            dto.DataNascimento,
            dto.Telefone ?? "",
            dto.Genero ?? "",
            dto.Cidade ?? "",
            dto.Estado ?? "",
            dto.CEP ?? ""
        );

        await _alunoRepository.UpdateAsync(aluno);
        await _alunoRepository.SaveChangesAsync();

        return MapToDto(aluno);
    }

    // Métodos de mapeamento privados
    private static AlunoDto MapToDto(Aluno aluno)
    {
        var idade = DateTime.Now.Year - aluno.DataNascimento.Year;
        if (DateTime.Now.DayOfYear < aluno.DataNascimento.DayOfYear)
            idade--;

        return new AlunoDto
        {
            Id = aluno.Id,
            CodigoUsuarioAutenticacao = aluno.CodigoUsuarioAutenticacao,
            Nome = aluno.Nome,
            Email = aluno.Email,
            DataNascimento = aluno.DataNascimento,
            Idade = idade,
            Telefone = aluno.Telefone,
            Genero = aluno.Genero,
            Cidade = aluno.Cidade,
            Estado = aluno.Estado,
            CEP = aluno.CEP,
            IsAtivo = aluno.IsAtivo,
            CreatedAt = aluno.CreatedAt,
            UpdatedAt = aluno.UpdatedAt,
            Matriculas = new List<MatriculaDto>() // Implementação futura
        };
    }

    private static AlunoResumoDto MapToResumoDto(Aluno aluno)
    {
        var idade = DateTime.Now.Year - aluno.DataNascimento.Year;
        if (DateTime.Now.DayOfYear < aluno.DataNascimento.DayOfYear)
            idade--;

        return new AlunoResumoDto
        {
            Id = aluno.Id,
            Nome = aluno.Nome,
            Email = aluno.Email,
            Idade = idade,
            Cidade = aluno.Cidade,
            Estado = aluno.Estado,
            IsAtivo = aluno.IsAtivo,
            QuantidadeMatriculasAtivas = 0, // Implementação futura
            QuantidadeCursosConcluidos = 0, // Implementação futura
            CreatedAt = aluno.CreatedAt
        };
    }

    private static AlunoPerfilDto MapToPerfilDto(Aluno aluno)
    {
        return new AlunoPerfilDto
        {
            Aluno = MapToDto(aluno),
            Estatisticas = new AlunoEstatisticasDto
            {
                TotalMatriculas = 0,
                MatriculasAtivas = 0,
                CursosConcluidos = 0,
                CertificadosEmitidos = 0,
                HorasEstudoTotal = 0,
                MediaNotas = 0,
                PercentualConclusaoMedio = 0,
                UltimoAcesso = null
            },
            HistoricoRecente = new List<HistoricoAlunoDto>()
        };
    }

    private static AlunoDashboardDto MapToDashboardDto(Aluno aluno)
    {
        return new AlunoDashboardDto
        {
            Aluno = MapToResumoDto(aluno),
            Estatisticas = new AlunoEstatisticasDto
            {
                TotalMatriculas = 0,
                MatriculasAtivas = 0,
                CursosConcluidos = 0,
                CertificadosEmitidos = 0,
                HorasEstudoTotal = 0,
                MediaNotas = 0,
                PercentualConclusaoMedio = 0,
                UltimoAcesso = null
            },
            MatriculasEmAndamento = new List<MatriculaDto>(),
            ProximasAulas = new List<ProximaAulaDto>(),
            CertificadosRecentes = new List<CertificadoDto>(),
            AtividadesRecentes = new List<HistoricoAlunoDto>()
        };
    }

    private static AlunoEstatisticasDto MapToEstatisticasDto(Aluno aluno)
    {
        return new AlunoEstatisticasDto
        {
            TotalMatriculas = 0,
            MatriculasAtivas = 0,
            CursosConcluidos = 0,
            CertificadosEmitidos = 0,
            HorasEstudoTotal = 0,
            MediaNotas = 0,
            PercentualConclusaoMedio = 0,
            UltimoAcesso = null
        };
    }
} 