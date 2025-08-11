using AutoMapper;
using Pagamentos.Application.ViewModels;
using Pagamentos.Domain.Entities;

namespace Pagamentos.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Pagamento, PagamentoViewModel>()
                      .ForMember(dest => dest.Transacao, opt => opt.MapFrom(src => src.Transacao));

            CreateMap<Transacao, TransacaoViewModel>();
        }

    }
}
