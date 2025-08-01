using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Conteudo.Infrastructure.Data;
using Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Conteudo.Infrastructure.Repositories
{
    public class CursoRepository(ConteudoDbContext dbContext) : ICursoRepository
    {   
        private readonly DbSet<Curso> _curso = dbContext.Set<Curso>();
        public IUnitOfWork UnitOfWork => dbContext;
        public async Task<IEnumerable<Curso>> ObterTodosAsync(bool includeAulas = false)
        {   
            var query = _curso.AsQueryable();

            if (includeAulas)
                query = query.Include(c => c.Aulas);

            return await query
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Curso?> ObterPorIdAsync(Guid id, bool includeAulas = false, bool noTracking = true)
        {
            var query = _curso.AsQueryable();

            if (includeAulas)
                query = query.Include(c => c.Aulas);

            if (noTracking)
                query = query.AsNoTracking();

            return await query
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Curso>> ObterPorCategoriaIdAsync(Guid categoriaId, bool includeAulas = false)
        {
            return await _curso
                .Where(c => c.CategoriaId == categoriaId)
                .AsNoTracking()
                .Include(c => includeAulas ? c.Aulas : null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Curso>> ObterAtivosAsync(bool includeAulas = false)
        {
            return await _curso
                .Where(c => c.Ativo)
                .AsNoTracking()
                .Include(c => includeAulas ? c.Aulas : null)
                .ToListAsync();
        }

        public Task<IEnumerable<Curso>> ObterPorPesquisaAsync(string searchTerm, bool includeAulas = false)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistePorIdAsync(Guid id)
        {
            return await _curso.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> ExistePorNomeAsync(string nome, Guid? excludeId = null)
        {
            return await _curso.AnyAsync(c => c.Nome == nome && (excludeId == null || c.Id != excludeId));
        }

        public void Adicionar(Curso curso)
        {
            _curso.Add(curso);
        }

        public void Atualizar(Curso curso)
        {
            _curso.Update(curso);
        }

        public void Deletar(Curso curso)
        {
            _curso.Remove(curso);
        }

        public async Task<int> ContarAsync()
        {
            return await _curso.CountAsync();
        }

        public async Task<int> ContarAtivosAsync()
        {
            return await _curso.CountAsync(c => c.Ativo);
        }

        public async Task<int> CountByCategoriaAsync(Guid categoriaId)
        {
            return await _curso.CountAsync(c => c.CategoriaId == categoriaId);
        }

        public void Dispose()
        {
            dbContext?.Dispose();
        }
    }
}
