using AutoMapper;
using Conteudo.Application.Commands.CadastrarCategoria;
using Conteudo.Application.DTOs;
using Conteudo.Domain.Entities;

namespace Conteudo.Application.Mappings
{
    public class CategoriaMap : Profile
    {
        public CategoriaMap() 
        {
            CreateMap<Categoria, CategoriaDto>()
                .ForMember(dest => dest.TotalCursos, opt => opt.MapFrom(src => src.Cursos.Count))
                .ForMember(dest => dest.CursosAtivos, opt => opt.MapFrom(src => src.Cursos.Count(c => c.Ativo)));

            CreateMap<CadastroCategoriaDto, CadastrarCategoriaCommand>().ReverseMap();
        }
    }
}
