using Conteudo.Application.Commands;
using Conteudo.Application.DTOs;
using Conteudo.Domain.Entities;
using Mapster;

namespace Conteudo.Application.Mappings;

public class ConteudoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Mapeamento de DTOs para Commands
        config.NewConfig<CadastroCursoDto, CadastrarCursoCommand>();
        config.NewConfig<AtualizarCursoDto, AtualizarCursoCommand>();
        config.NewConfig<CadastroCategoriaDto, CadastrarCategoriaCommand>();

        // Mapeamento de Entidades para DTOs
        config.NewConfig<Curso, CursoDto>()
            .Map(dest => dest.NomeCategoria, src => src.Categoria != null ? src.Categoria.Nome : string.Empty)
            .Map(dest => dest.Resumo, src => src.ConteudoProgramatico.Resumo)
            .Map(dest => dest.Descricao, src => src.ConteudoProgramatico.Descricao)
            .Map(dest => dest.Objetivos, src => src.ConteudoProgramatico.Objetivos)
            .Map(dest => dest.PreRequisitos, src => src.ConteudoProgramatico.PreRequisitos)
            .Map(dest => dest.PublicoAlvo, src => src.ConteudoProgramatico.PublicoAlvo)
            .Map(dest => dest.Metodologia, src => src.ConteudoProgramatico.Metodologia)
            .Map(dest => dest.Recursos, src => src.ConteudoProgramatico.Recursos)
            .Map(dest => dest.Avaliacao, src => src.ConteudoProgramatico.Avaliacao)
            .Map(dest => dest.Bibliografia, src => src.ConteudoProgramatico.Bibliografia)
            .Map(dest => dest.VagasDisponiveis, src => src.VagasDisponiveis)
            .Map(dest => dest.PodeSerMatriculado, src => src.PodeSerMatriculado)
            .Map(dest => dest.Aulas, src => src.Aulas);

        config.NewConfig<Aula, AulaDto>();

        config.NewConfig<Categoria, CategoriaDto>()
            .Map(dest => dest.TotalCursos, src => src.Cursos.Count)
            .Map(dest => dest.CursosAtivos, src => src.Cursos.Count(c => c.Ativo));
    }
} 