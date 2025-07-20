using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Data;

public class AuthDbContext : IdentityDbContext<ApplicationUser>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configurações específicas do ApplicationUser podem ser adicionadas aqui
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DataNascimento).IsRequired();
            entity.Property(e => e.DataCadastro).IsRequired();
            entity.Property(e => e.Ativo).IsRequired();
        });
    }
} 