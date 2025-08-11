using Microsoft.EntityFrameworkCore;
using Pagamentos.API.Context;
using Pagamentos.Infrastructure.Context;

namespace Pagamentos.API.Configuration
{
    public static class DbContextConfig
    {
        public static WebApplicationBuilder AddDbContextConfig(this WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment() || builder.Environment.IsEnvironment("Test"))
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

                builder.Services.AddDbContext<PagamentoContext>(options =>
                    options.UseSqlite(connectionString));

                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite(connectionString));

                return builder;
            }
            else
            {
                builder.Services.AddDbContext<PagamentoContext>(options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });

                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });

                return builder;
            }

        }
    }
}
