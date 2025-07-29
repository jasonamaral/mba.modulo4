using AutoMapper;
using Conteudo.Application.Commands;
using Conteudo.Application.DTOs;
using Conteudo.Domain.Entities;

namespace Conteudo.Application.Mappings
{
    public class CursoMap : Profile
    {
        public CursoMap()
        {
            CreateMap<CadastroCursoDto, CadastrarCursoCommand>().ReverseMap();
            CreateMap<AtualizarCursoDto, AtualizarCursoCommand>().ReverseMap();

            CreateMap<Curso, CursoDto>()
            .ForMember(dest => dest.NomeCategoria,
                opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.Nome : string.Empty))
            .ForMember(dest => dest.Resumo,
                opt => opt.MapFrom(src => src.ConteudoProgramatico.Resumo))
            .ForMember(dest => dest.Descricao,
                opt => opt.MapFrom(src => src.ConteudoProgramatico.Descricao))
            .ForMember(dest => dest.Objetivos,
                opt => opt.MapFrom(src => src.ConteudoProgramatico.Objetivos))
            .ForMember(dest => dest.PreRequisitos,
                opt => opt.MapFrom(src => src.ConteudoProgramatico.PreRequisitos))
            .ForMember(dest => dest.PublicoAlvo,
                opt => opt.MapFrom(src => src.ConteudoProgramatico.PublicoAlvo))
            .ForMember(dest => dest.Metodologia,
                opt => opt.MapFrom(src => src.ConteudoProgramatico.Metodologia))
            .ForMember(dest => dest.Recursos,
                opt => opt.MapFrom(src => src.ConteudoProgramatico.Recursos))
            .ForMember(dest => dest.Avaliacao,
                opt => opt.MapFrom(src => src.ConteudoProgramatico.Avaliacao))
            .ForMember(dest => dest.Bibliografia,
                opt => opt.MapFrom(src => src.ConteudoProgramatico.Bibliografia))
            .ForMember(dest => dest.VagasDisponiveis,
                opt => opt.MapFrom(src => src.VagasDisponiveis))
            .ForMember(dest => dest.PodeSerMatriculado,
                opt => opt.MapFrom(src => src.PodeSerMatriculado))
            .ForMember(dest => dest.Aulas,
                opt => opt.MapFrom(src => src.Aulas));

            CreateMap<Aula, AulaDto>();
        }
    }
}
