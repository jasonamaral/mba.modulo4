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

            var contextId = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var contextCurso = scope.ServiceProvider.GetRequiredService<CursoContext>();
            var contextPagamento = scope.ServiceProvider.GetRequiredService<PagamentoContext>();
            var contextAluno = scope.ServiceProvider.GetRequiredService<AlunoContext>();

            if (env.IsDevelopment() || env.IsEnvironment("Test"))
            {
                await MigrarBancosAsync(contextId, contextCurso, contextPagamento, contextAluno);
                await EnsureSeedProducts(serviceProvider, contextId, contextAluno, contextCurso);
            }
        }

        private static async Task MigrarBancosAsync(DbContext contextId,
                                                    DbContext contextCurso,
                                                    DbContext contextPagamento,
                                                    DbContext contextAluno)
        {
            await contextId.Database.MigrateAsync();
            await contextCurso.Database.MigrateAsync();
            await contextPagamento.Database.MigrateAsync();
            await contextAluno.Database.MigrateAsync();
        }


        private static async Task EnsureSeedProducts(IServiceProvider serviceProvider,
                                                     ApplicationDbContext contextId,
                                                     AlunoContext contextAluno,
                                                     CursoContext cursoContext)
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

            #region Aluno

            var aluno = new Aluno(id: Guid.Parse(user.Id), nome: user.UserName, email: user.Email, ativo: true);

            await contextAluno.Alunos.AddAsync(aluno);

            await contextAluno.SaveChangesAsync();


            #endregion

            #region Curso

            Curso curso;
            Modulo modulo;
            Aula aula;

            #region Curso 1
            curso = new Curso("Explorando novas linguagens de programação", "Uma abordagem moderna para desenvolvimento de software, incluindo práticas de programação e metodologias ágeis.", 99.99m, CategoriaCurso.Negocios, NivelDificuldade.Avancado);
            curso.Ativar();

            modulo = new Modulo("Fundamentos Básicos");

            aula = new Aula("Introdução à programação", "https://example.com/video1", TimeSpan.Parse("00:45:00"));
            modulo.AdicionarAula(aula);

            aula = new Aula("Conceitos de algoritmos", "https://example.com/video2", TimeSpan.Parse("00:30:00"));
            modulo.AdicionarAula(aula);

            curso.AdicionarModulo(modulo);

            await cursoContext.Cursos.AddAsync(curso);
            await cursoContext.SaveChangesAsync();


            #endregion

            #region Curso 2
            curso = new Curso("Fundamentos de Machine Learning", "Aprenda as bases da inteligência artificial aplicadas em projetos reais.", 99.99m, CategoriaCurso.Marketing, NivelDificuldade.Intermediario);
            curso.Ativar();

            modulo = new Modulo("Introdução ao ML");
            aula = new Aula("História do Machine Learning", "https://example.com/ml1", TimeSpan.Parse("00:35:00"));
            modulo.AdicionarAula(aula);


            curso.AdicionarModulo(modulo);

            await cursoContext.Cursos.AddAsync(curso);
            await cursoContext.SaveChangesAsync();


            #endregion

            #region Curso 3
            curso = new Curso("Curso de ASP.NET Core Web API", "Construa APIs robustas e escaláveis com ASP.NET Core.", 99.99m, CategoriaCurso.Design, NivelDificuldade.Avancado);
            curso.Ativar();

            modulo = new Modulo("Web APIs na prática");

            aula = new Aula("Criando o primeiro projeto", "https://example.com/aspnet1", TimeSpan.Parse("01:00:00"));
            modulo.AdicionarAula(aula);

            aula = new Aula("Configurando rotas", "https://example.com/aspnet2", TimeSpan.Parse("00:50:00"));
            modulo.AdicionarAula(aula);

            curso.AdicionarModulo(modulo);

            await cursoContext.Cursos.AddAsync(curso);
            await cursoContext.SaveChangesAsync();

            #endregion

            #region Curso 4
            curso = new Curso("Desenvolvimento Mobile com Flutter", "Crie aplicativos incríveis para Android e iOS com Flutter e Dart.", 99.99m, CategoriaCurso.Idiomas, NivelDificuldade.Intermediario);
            curso.Ativar();

            modulo = new Modulo("Primeiros passos no Flutter");

            aula = new Aula("Configurando o ambiente", "https://example.com/flutter1", TimeSpan.Parse("00:40:00"));
            modulo.AdicionarAula(aula);

            aula = new Aula("Widgets Básicos", "https://example.com/flutter2", TimeSpan.Parse("00:55:00"));
            modulo.AdicionarAula(aula);

            curso.AdicionarModulo(modulo);

            await cursoContext.Cursos.AddAsync(curso);
            await cursoContext.SaveChangesAsync();

            #endregion

            #region Curso 5
            curso = new Curso("Administração de Bancos de Dados", "Administre bancos de dados SQL de maneira eficiente e segura.", 99.99m, CategoriaCurso.Idiomas, NivelDificuldade.Intermediario);
            curso.Ativar();

            modulo = new Modulo("Banco de Dados Relacional");

            aula = new Aula("Introdução ao SQL", "https://example.com/sql1", TimeSpan.Parse("00:30:00"));
            modulo.AdicionarAula(aula);

            aula = new Aula("Consultas avançadas", "https://example.com/sql2", TimeSpan.Parse("00:45:00"));
            modulo.AdicionarAula(aula);

            aula = new Aula("Stored Procedures", "https://example.com/sql3", TimeSpan.Parse("00:40:00"));
            modulo.AdicionarAula(aula);

            curso.AdicionarModulo(modulo);

            await cursoContext.Cursos.AddAsync(curso);
            await cursoContext.SaveChangesAsync();

            #endregion

            #region Curso 6
            curso = new Curso("Especialista em Desenvolvimento Web", "Domine o desenvolvimento web completo, do frontend ao backend.", 99.99m, CategoriaCurso.Programacao, NivelDificuldade.Intermediario);
            curso.Ativar();

            modulo = new Modulo("HTML e CSS");
            aula = new Aula("HTML5 - O básico", "https://example.com/html1", TimeSpan.Parse("00:30:00"));
            modulo.AdicionarAula(aula);
            aula = new Aula("CSS3 - Estilizando páginas", "https://example.com/css1", TimeSpan.Parse("00:40:00"));
            modulo.AdicionarAula(aula);
            curso.AdicionarModulo(modulo);


            modulo = new Modulo("JavaScript Essencial");
            aula = new Aula("Conceitos de JavaScript", "https://example.com/js1", TimeSpan.Parse("00:50:00"));
            modulo.AdicionarAula(aula);
            aula = new Aula("Manipulação de DOM", "https://example.com/js2", TimeSpan.Parse("01:00:00"));
            modulo.AdicionarAula(aula);
            curso.AdicionarModulo(modulo);

            modulo = new Modulo("JavaScript Essencial");
            aula = new Aula("Conceitos de JavaScript", "https://example.com/js1", TimeSpan.Parse("00:50:00"));
            modulo.AdicionarAula(aula);
            aula = new Aula("Manipulação de DOM", "https://example.com/js2", TimeSpan.Parse("01:00:00"));
            modulo.AdicionarAula(aula);
            curso.AdicionarModulo(modulo);

            modulo = new Modulo("Backend com Node.js");
            aula = new Aula("Servidor HTTP básico", "https://example.com/node1", TimeSpan.Parse("00:45:00"));
            modulo.AdicionarAula(aula);
            aula = new Aula("API REST com Express", "https://example.com/node2", TimeSpan.Parse("01:10:00"));
            modulo.AdicionarAula(aula);
            curso.AdicionarModulo(modulo);


            await cursoContext.Cursos.AddAsync(curso);
            await cursoContext.SaveChangesAsync();

            #endregion




            #endregion

        }
    }
}
