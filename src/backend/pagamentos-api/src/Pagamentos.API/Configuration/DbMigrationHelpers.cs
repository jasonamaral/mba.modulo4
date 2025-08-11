using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pagamentos.Infrastructure.Context;

namespace Pagamentos.API.Configuration
{
    public static class DbMigrationHelperExtension
    {
        public static void UseDbMigrationHelper(this WebApplication app)
        {
            DbMigrationHelpers.EnsureSeedData(app).Wait();
        }
    }

    public static class DbMigrationHelpers
    {
        public static async Task EnsureSeedData(WebApplication serviceScope)
        {
            var services = serviceScope.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var contextPagamento = scope.ServiceProvider.GetRequiredService<PagamentoContext>();

            if (env.IsDevelopment() || env.IsEnvironment("Test"))
            {
                await MigrarBancosAsync(contextPagamento);
                await EnsureSeedProducts(serviceProvider);
            }
        }

        private static async Task MigrarBancosAsync(DbContext contextPagamento)
        {
            await contextPagamento.Database.MigrateAsync();
        }


        private static async Task EnsureSeedProducts(IServiceProvider serviceProvider)
        {
                return;
        }
    }
}
