using Conteudo.Application.Commands.AtualizarCurso;
using Conteudo.Application.Commands.CadastrarCategoria;
using Conteudo.Application.Commands.CadastrarCurso;
using Conteudo.Application.DTOs;
using Conteudo.Domain.Entities;
using Core.Communication;
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
            .Map(dest => dest.Resumo, src => src.ConteudoProgramatico.Resumo)
            .Map(dest => dest.Descricao, src => src.ConteudoProgramatico.Descricao)
            .Map(dest => dest.Objetivos, src => src.ConteudoProgramatico.Objetivos)
            .Map(dest => dest.PreRequisitos, src => src.ConteudoProgramatico.PreRequisitos)
            .Map(dest => dest.PublicoAlvo, src => src.ConteudoProgramatico.PublicoAlvo)
            .Map(dest => dest.Metodologia, src => src.ConteudoProgramatico.Metodologia)
            .Map(dest => dest.Recursos, src => src.ConteudoProgramatico.Recursos)
            .Map(dest => dest.Avaliacao, src => src.ConteudoProgramatico.Avaliacao)
            .Map(dest => dest.Bibliografia, src => src.ConteudoProgramatico.Bibliografia);

        config.NewConfig<PagedResult<Curso>, PagedResult<CursoDto>>()
        .Map(dest => dest.Items, src => src.Items.Adapt<List<CursoDto>>(config));
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