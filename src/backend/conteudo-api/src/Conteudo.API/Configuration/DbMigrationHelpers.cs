using Conteudo.Domain.Entities;
using Conteudo.Domain.ValueObjects;
using Conteudo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Conteudo.API.Configuration
{
    public static class DbMigrationHelpers
    {
        public static void UseDbMigrationHelper(this WebApplication app)
        {
            EnsureSeedData(app).Wait();
        }

        public static async Task EnsureSeedData(WebApplication application)
        {
            var service = application.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(service);
        }

        private static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ConteudoDbContext>();
            var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

            if (env.IsDevelopment())
            {
                await EnsureSeedData(context);
            }
        }

        private static async Task EnsureSeedData(ConteudoDbContext context)
        {   
            await context.Database.EnsureDeletedAsync();
            await context.Database.MigrateAsync();

            // Categoria
            if (!context.Categorias.Any())
            {
                var categoria = new Categoria(nome: "Programação",
                        descricao: "Cursos voltados para programação e desenvolvimento de software",
                        cor: "#4CAF50",
                        iconeUrl: "https://exemplo.com/icones/programacao.png",
                        ordem: 1);
                categoria.DefinirId(Guid.Parse("b2cfb629-db43-41f2-8318-f3c52a648b39"));

                context.Categorias.Add(categoria);
                await context.SaveChangesAsync();
            }

            // Curso
            if (!context.Cursos.Any())
            {
                var categoriaId = context.Categorias.First().Id;

                var conteudo = new ConteudoProgramatico(
                    resumo: "Curso introdutório de C# e .NET",
                    descricao: "Aprenda os fundamentos de C# e .NET para aplicações modernas",
                    objetivos: "Fornecer base sólida para desenvolvimento em .NET",
                    preRequisitos: "Lógica de programação",
                    publicoAlvo: "Iniciantes em programação",
                    metodologia: "Aulas gravadas e exercícios práticos",
                    recursos: "Vídeos, PDFs e fóruns",
                    avaliacao: "Provas e projetos",
                    bibliografia: "Documentação oficial Microsoft"
                );

                var curso = new Curso(nome: "C# para Iniciantes",
                        valor: 299.90m,
                        conteudoProgramatico: conteudo,
                        duracaoHoras: 40,
                        nivel: "Básico",
                        instrutor: "João Silva",
                        vagasMaximas: 20,
                        imagemUrl: "https://exemplo.com/imagens/curso-csharp.jpg",
                        validoAte: DateTime.UtcNow.AddYears(1),
                        categoriaId: categoriaId
                    );
                curso.DefinirId(Guid.Parse("a1e5f8c3-3d2b-4f1e-9c6e-8f9b7c6d5e4f"));

                context.Cursos.Add(curso);
                await context.SaveChangesAsync();
            }

            // Aula
            if (!context.Aulas.Any())
            {
                var cursoId = context.Cursos.First().Id;

                var aula = new Aula(cursoId: cursoId,
                        nome: "Introdução ao C#",
                        descricao: "Primeiros passos com a linguagem C#",
                        numero: 1,
                        duracaoMinutos: 45,
                        videoUrl: "https://exemplo.com/videos/aula1.mp4",
                        tipoAula: "Teórica",
                        isObrigatoria: true,
                        observacoes: "Assistir antes de prosseguir"
                    );
                aula.DefinirId(Guid.Parse("c3d9f1e2-5b6a-4f7e-8d9c-0a1b2c3d4e5f"));

                context.Aulas.Add(aula);
                await context.SaveChangesAsync();
            }

            // Material
            if (!context.Materiais.Any())
            {
                var aulaId = context.Aulas.First().Id;

                var material = new Material(aulaId: aulaId,
                        nome: "Slide da Aula 1",
                        descricao: "Apresentação utilizada na aula introdutória",
                        tipoMaterial: "PDF",
                        url: "https://exemplo.com/materiais/slide-aula1.pdf",
                        isObrigatorio: false,
                        tamanhoBytes: 1024 * 200, // ~200 KB
                        extensao: ".pdf",
                        ordem: 1
                    );
                material.DefinirId(Guid.Parse("d4e5f6a7-b8c9-0a1b-2c3d-4e5f6a7b8c9d"));

                context.Materiais.Add(material);
                await context.SaveChangesAsync();
            }
        }
    }
}
