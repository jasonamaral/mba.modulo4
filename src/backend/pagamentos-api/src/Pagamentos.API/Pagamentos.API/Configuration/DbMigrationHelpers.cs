using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pagamentos.API.Context;
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

            var contextId = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var contextPagamento = scope.ServiceProvider.GetRequiredService<PagamentoContext>();

            if (env.IsDevelopment() || env.IsEnvironment("Test"))
            {
                await MigrarBancosAsync(contextId, contextPagamento);
                await EnsureSeedProducts(serviceProvider, contextId);
            }
        }

        private static async Task MigrarBancosAsync(DbContext contextId,
                                                    DbContext contextPagamento)

        {
            await contextId.Database.MigrateAsync();
            await contextPagamento.Database.MigrateAsync();
        }


        private static async Task EnsureSeedProducts(IServiceProvider serviceProvider,
                                                     ApplicationDbContext contextId)
        {
            if (contextId.Users.Any())
                return;


            #region Aluno Admin Identity

            var id = Guid.NewGuid();

            var user = new Microsoft.AspNetCore.Identity.IdentityUser
            {
                Id = id.ToString(),
                UserName = "marco@imperiumsolucoes.com.br",
                NormalizedUserName = "MARCO@IMPERIUMSOLUCOES.COM.BR",
                Email = "marco@imperiumsolucoes.com.br",
                NormalizedEmail = "MARCO@IMPERIUMSOLUCOES.COM.BR",
                AccessFailedCount = 0,
                LockoutEnabled = false,
                PasswordHash = "AQAAAAIAAYagAAAAEK2wx+k3kW2104aBWullMN7JJ6VTreIIcBpiyzNVRhRONj2J5GX9ig8EIA9TQcqn9w==",
                TwoFactorEnabled = false,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            await contextId.Users.AddAsync(user);

            await contextId.SaveChangesAsync();

            #endregion

            #region Roles

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();


            string[] roleNames = { "Admin", "User", "Manager" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var _userManager = serviceProvider.GetService<UserManager<Microsoft.AspNetCore.Identity.IdentityUser>>();

            user = await _userManager.FindByIdAsync(id.ToString());

            await _userManager.AddToRoleAsync(user, "Admin");


            #endregion



        }
    }
}
