using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Conteudo.Infrastructure.Data;
using Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Conteudo.Infrastructure.Repositories
{
    public class MaterialRepository(ConteudoDbContext dbContext) : IMaterialRepository
    {
        private readonly DbSet<Material> _material = dbContext.Set<Material>();
        public IUnitOfWork UnitOfWork => dbContext;

        public async Task<IEnumerable<Material>> ObterTodosAsync()
        {
            return await _material
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Material?> ObterPorIdAsync(Guid id)
        {
            return await _material
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Material>> ObterPorAulaIdAsync(Guid aulaId)
        {
            return await _material
                .Where(m => m.AulaId == aulaId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Material>> ObterAtivosAsync()
        {
            return await _material
                .Where(m => m.IsAtivo)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Material>> ObterAtivosPorAulaIdAsync(Guid aulaId)
        {
            return await _material
                .Where(m => m.AulaId == aulaId && m.IsAtivo)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Material>> ObterObrigatoriosPorAulaIdAsync(Guid aulaId)
        {
            return await _material
                .Where(m => m.AulaId == aulaId && m.IsObrigatorio)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> ExisteAsync(Guid id)
        {
            return await _material.AnyAsync(m => m.Id == id);
        }

        public async Task<bool> ExistePorNomeAsync(Guid aulaId, string nome, Guid? excludeId = null)
        {
            return await _material.AnyAsync(m =>
                m.AulaId == aulaId &&
                m.Nome == nome &&
                (excludeId == null || m.Id != excludeId));
        }

        public async Task<Material> CadastrarMaterialAsync(Material material)
        {
            await _material.AddAsync(material);
            return material;
        }

        public async Task<Material> AtualizarMaterialAsync(Material material)
        {
            _material.Update(material);
            await Task.CompletedTask;
            return material;
        }

        public async Task AtivarMaterialAsync(Guid id)
        {
            var material = await ObterPorIdAsync(id);
            if (material != null)
            {
                material.Ativar();
                _material.Update(material);
                await Task.CompletedTask;
            }
        }

        public async Task DesativarMaterialAsync(Guid id)
        {
            var material = await ObterPorIdAsync(id);
            if (material != null)
            {
                material.Desativar();
                _material.Update(material);
                await Task.CompletedTask;
            }
        }

        public async Task ExcluirMaterialAsync(Guid id)
        {
            var material = await ObterPorIdAsync(id);
            if (material != null)
            {
                _material.Remove(material);
                await Task.CompletedTask;
            }
        }

        public async Task<int> ContarMateriaisAsync()
        {
            return await _material.CountAsync();
        }

        public async Task<int> ContarMateriaisPorAulaAsync(Guid aulaId)
        {
            return await _material.CountAsync(m => m.AulaId == aulaId);
        }

        public async Task<int> ContarMateriaisAtivosPorAulaAsync(Guid aulaId)
        {
            return await _material.CountAsync(m => m.AulaId == aulaId && m.IsAtivo);
        }

        public void Dispose()
        {
            dbContext?.Dispose();
        }
    }
}
