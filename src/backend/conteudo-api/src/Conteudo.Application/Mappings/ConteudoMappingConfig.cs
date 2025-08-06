using Conteudo.Application.Commands.AtualizarCurso;
using Conteudo.Application.Commands.CadastrarCategoria;
using Conteudo.Application.Commands.CadastrarCurso;
using Conteudo.Application.DTOs;
using Conteudo.Domain.Entities;
using Core.SharedDtos.Conteudo;
using Mapster;

namespace Conteudo.Application.Mappings;

public class ConteudoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        ConfigureCommandMappings(config);
        ConfigureCourseMappings(config);
        ConfigureLessonMappings(config);
        ConfigureCategoryMappings(config);
    }

    private static void ConfigureCommandMappings(TypeAdapterConfig config)
    {
        config.NewConfig<CadastroCursoDto, CadastrarCursoCommand>().TwoWays();

        config.NewConfig<AtualizarCursoDto, AtualizarCursoCommand>().TwoWays();

        config.NewConfig<CadastroCategoriaDto, CadastrarCategoriaCommand>().TwoWays();
    }

    private static void ConfigureCourseMappings(TypeAdapterConfig config)
    {
        config.NewConfig<Curso, CursoDto>()
            .Map(dest => dest.NomeCategoria, src => src.Categoria != null ? src.Categoria.Nome : string.Empty)
            .Map(dest => dest.VagasDisponiveis, src => src.VagasDisponiveis)
            .Map(dest => dest.PodeSerMatriculado, src => src.PodeSerMatriculado)
            .Map(dest => dest.Aulas, src => src.Aulas)
            .Map(dest => dest, src => src.ConteudoProgramatico);
    }

    private static void ConfigureLessonMappings(TypeAdapterConfig config)
    {
        config.NewConfig<Aula, AulaDto>()
            .PreserveReference(true);
    }

    private static void ConfigureCategoryMappings(TypeAdapterConfig config)
    {
        config.NewConfig<Categoria, CategoriaDto>()
            .Map(dest => dest.TotalCursos, src => src.Cursos.Count)
            .Map(dest => dest.CursosAtivos, src => src.Cursos.Count(c => c.Ativo))
            .PreserveReference(true);
    }
}