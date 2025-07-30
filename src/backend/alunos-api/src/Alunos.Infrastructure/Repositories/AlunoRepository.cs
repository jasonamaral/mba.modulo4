using Alunos.Application.Interfaces.Repositories;
using Alunos.Domain.Entities;
using Alunos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Alunos.Infrastructure.Repositories;

public class AlunoRepository : IAlunoRepository
{
    private readonly AlunosDbContext _context;

    public AlunoRepository(AlunosDbContext context)
    {
        _context = context;
    }

    public async Task<Aluno?> GetByIdAsync(Guid id)
    {
        return await _context.Alunos
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Aluno?> GetByCodigoUsuarioAsync(Guid codigoUsuario)
    {
        return await _context.Alunos
            .FirstOrDefaultAsync(a => a.CodigoUsuarioAutenticacao == codigoUsuario);
    }

    public async Task<Aluno?> GetByEmailAsync(string email)
    {
        return await _context.Alunos
            .FirstOrDefaultAsync(a => a.Email == email);
    }

    public async Task<IEnumerable<Aluno>> GetAllAsync(bool includeMatriculas = false)
    {
        var query = _context.Alunos.AsQueryable();
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Aluno>> GetAlunosAtivosAsync(bool includeMatriculas = false)
    {
        var query = _context.Alunos.Where(a => a.IsAtivo);
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Aluno>> BuscarPorNomeAsync(string nome, bool includeMatriculas = false)
    {
        var query = _context.Alunos.Where(a => a.Nome.Contains(nome));
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Aluno>> GetByCidadeAsync(string cidade, bool includeMatriculas = false)
    {
        var query = _context.Alunos.Where(a => a.Cidade == cidade);
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Aluno>> GetByEstadoAsync(string estado, bool includeMatriculas = false)
    {
        var query = _context.Alunos.Where(a => a.Estado == estado);
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Aluno>> GetAlunosMatriculadosNoCursoAsync(Guid cursoId)
    {
        // Por enquanto retorna lista vazia, pois a relação com matrículas precisa ser configurada adequadamente
        return await Task.FromResult(new List<Aluno>());
    }

    public async Task<int> GetCountAsync()
    {
        return await _context.Alunos.CountAsync();
    }

    public async Task<int> GetCountAtivosAsync()
    {
        return await _context.Alunos.CountAsync(a => a.IsAtivo);
    }

    public async Task<bool> ExisteEmailAsync(string email, Guid? excluirId = null)
    {
        var query = _context.Alunos.Where(a => a.Email == email);

        if (excluirId.HasValue)
            query = query.Where(a => a.Id != excluirId.Value);

        return await query.AnyAsync();
    }

    public async Task<Aluno> AddAsync(Aluno aluno)
    {
        await _context.Alunos.AddAsync(aluno);
        return aluno;
    }

    public async Task<Aluno> UpdateAsync(Aluno aluno)
    {
        _context.Alunos.Update(aluno);
        return aluno;
    }

    public async Task DeleteAsync(Aluno aluno)
    {
        _context.Alunos.Remove(aluno);
        await Task.CompletedTask;
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var aluno = await GetByIdAsync(id);
        if (aluno != null)
        {
            _context.Alunos.Remove(aluno);
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}