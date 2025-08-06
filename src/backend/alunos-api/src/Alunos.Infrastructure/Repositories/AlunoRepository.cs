using Alunos.Domain.Entities;
using Alunos.Domain.Interfaces;
using Alunos.Domain.ValueObjects;
using Alunos.Infrastructure.Data;
using Core.Data;
using Microsoft.EntityFrameworkCore;
using Core.Utils;

namespace Alunos.Infrastructure.Repositories;

public class AlunoRepository(AlunoDbContext context) : IAlunoRepository
{
    private readonly AlunoDbContext _context = context;
    public IUnitOfWork UnitOfWork => _context;

    #region Alunos
    public async Task AdicionarAsync(Aluno aluno)
    {
        await _context.Alunos.AddAsync(aluno);
    }

    public async Task AtualizarAsync(Aluno aluno)
    {
        aluno.RegistrarDataAlteracao();
        _context.Alunos.Update(aluno);
        await Task.CompletedTask;
    }

    public async Task<Aluno> ObterPorIdAsync(Guid alunoId)
    {
        return await _context.Alunos
            .Include(a => a.MatriculasCursos)
            .ThenInclude(m => m.Certificado)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == alunoId);
    }

    public async Task<Aluno> ObterPorEmailAsync(string email)
    {
        return await _context.Alunos
            .Include(a => a.MatriculasCursos)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Email == email);
    }

    public async Task<bool> ExisteEmailAsync(string email)
    {
        return await _context.Alunos.AnyAsync(a => a.Email == email);
    }

    public async Task<Aluno> ObterPorCodigoUsuarioAsync(Guid codigoUsuario)
    {
        return await _context.Alunos
            .Include(a => a.MatriculasCursos)
            .ThenInclude(m => m.Certificado)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.CodigoUsuarioAutenticacao == codigoUsuario);
    }
    #endregion

    #region Matricula Curso
    public async Task AdicionarMatriculaCursoAsync(MatriculaCurso matriculaCurso)
    {
        await _context.MatriculasCursos.AddAsync(matriculaCurso);
    }

    public async Task AdicionarCertificadoMatriculaCursoAsync(Certificado certificado)
    {
        await _context.Certificados.AddAsync(certificado);
    }

    public async Task<MatriculaCurso?> ObterMatriculaPorIdAsync(Guid matriculaId)
    {
        return await _context.MatriculasCursos
            .Include(m => m.HistoricoAprendizado)
            .Include(m => m.Certificado)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == matriculaId);
    }

    public async Task<MatriculaCurso?> ObterMatriculaPorAlunoECursoAsync(Guid alunoId, Guid cursoId)
    {
        return await _context.MatriculasCursos
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);
    }
    #endregion

    #region Certificado
    public async Task AtualizarEstadoHistoricoAprendizadoAsync(HistoricoAprendizado historicoAntigo, HistoricoAprendizado historicoNovo)
    {
        _context.AtualizarEstadoValueObject(historicoAntigo, historicoNovo);
        await Task.CompletedTask;
    }

    public async Task<Certificado?> ObterCertificadoPorMatriculaAsync(Guid matriculaId)
    {
        return await _context.Certificados
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.MatriculaCursoId == matriculaId);
    }
    #endregion

    public void Dispose()
    {
        _context?.Dispose();
        GC.SuppressFinalize(this);
    }

    //public async Task<IEnumerable<Aluno>> GetAllAsync(bool includeMatriculas = false)
    //{
    //    var query = _context.Alunos.AsQueryable();
    //    return await query.ToListAsync();
    //}

    //public async Task<IEnumerable<Aluno>> GetAlunosAtivosAsync(bool includeMatriculas = false)
    //{
    //    var query = _context.Alunos.Where(a => a.IsAtivo);
    //    return await query.ToListAsync();
    //}

    //public async Task<IEnumerable<Aluno>> BuscarPorNomeAsync(string nome, bool includeMatriculas = false)
    //{
    //    var query = _context.Alunos.Where(a => a.Nome.Contains(nome));
    //    return await query.ToListAsync();
    //}

    //public async Task<IEnumerable<Aluno>> GetAlunosMatriculadosNoCursoAsync(Guid cursoId)
    //{
    //    // Por enquanto retorna lista vazia, pois a relação com matrículas precisa ser configurada adequadamente
    //    return await Task.FromResult(new List<Aluno>());
    //}

    //public async Task<int> GetCountAsync()
    //{
    //    return await _context.Alunos.CountAsync();
    //}

    //public async Task<int> GetCountAtivosAsync()
    //{
    //    return await _context.Alunos.CountAsync(a => a.IsAtivo);
    //}
}