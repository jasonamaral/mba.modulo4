using Alunos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Conteudo.API.Configuration;
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
        var context = scope.ServiceProvider.GetRequiredService<AlunoDbContext>();
        var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

        if (env.IsDevelopment())
        {
            await EnsureSeedData(context);
        }
    }

    private static async Task EnsureSeedData(AlunoDbContext context)
    {   
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();

        //// Categoria
        //if (!context.Categorias.Any())
        //{
        //    context.Categorias.AddRange(
        //        new Categoria(
        //            nome: "Programação",
        //            descricao: "Cursos voltados para programação e desenvolvimento de software",
        //            cor: "#4CAF50",
        //            iconeUrl: "https://exemplo.com/icones/programacao.png",
        //            ordem: 1
        //        )
        //    );
        //    await context.SaveChangesAsync();
        //}

        //// Curso
        //if (!context.Cursos.Any())
        //{
        //    var categoriaId = context.Categorias.First().Id;

        //    var conteudo = new ConteudoProgramatico(
        //        resumo: "Curso introdutório de C# e .NET",
        //        descricao: "Aprenda os fundamentos de C# e .NET para aplicações modernas",
        //        objetivos: "Fornecer base sólida para desenvolvimento em .NET",
        //        preRequisitos: "Lógica de programação",
        //        publicoAlvo: "Iniciantes em programação",
        //        metodologia: "Aulas gravadas e exercícios práticos",
        //        recursos: "Vídeos, PDFs e fóruns",
        //        avaliacao: "Provas e projetos",
        //        bibliografia: "Documentação oficial Microsoft"
        //    );

        //    context.Cursos.AddRange(
        //        new Curso(
        //            nome: "C# para Iniciantes",
        //            valor: 299.90m,
        //            conteudoProgramatico: conteudo,
        //            duracaoHoras: 40,
        //            nivel: "Básico",
        //            instrutor: "João Silva",
        //            vagasMaximas: 20,
        //            imagemUrl: "https://exemplo.com/imagens/curso-csharp.jpg",
        //            validoAte: DateTime.UtcNow.AddYears(1),
        //            categoriaId: categoriaId
        //        )
        //    );
        //    await context.SaveChangesAsync();
        //}

        //// Aula
        //if (!context.Aulas.Any())
        //{
        //    var cursoId = context.Cursos.First().Id;

        //    context.Aulas.AddRange(
        //        new Aula(
        //            cursoId: cursoId,
        //            nome: "Introdução ao C#",
        //            descricao: "Primeiros passos com a linguagem C#",
        //            numero: 1,
        //            duracaoMinutos: 45,
        //            videoUrl: "https://exemplo.com/videos/aula1.mp4",
        //            tipoAula: "Teórica",
        //            isObrigatoria: true,
        //            observacoes: "Assistir antes de prosseguir"
        //        )
        //    );
        //    await context.SaveChangesAsync();
        //}

        //// Material
        //if (!context.Materiais.Any())
        //{
        //    var aulaId = context.Aulas.First().Id;

        //    context.Materiais.AddRange(
        //        new Material(
        //            aulaId: aulaId,
        //            nome: "Slide da Aula 1",
        //            descricao: "Apresentação utilizada na aula introdutória",
        //            tipoMaterial: "PDF",
        //            url: "https://exemplo.com/materiais/slide-aula1.pdf",
        //            isObrigatorio: false,
        //            tamanhoBytes: 1024 * 200, // ~200 KB
        //            extensao: ".pdf",
        //            ordem: 1
        //        )
        //    );
        //    await context.SaveChangesAsync();
        //}
    }

}
