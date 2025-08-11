using AutoMapper;
using Pagamentos.Application.ViewModels;
using Pagamentos.Domain.Entities;

namespace Pagamentos.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<PagamentoViewModel, Pagamento>()
                     .ForMember(dest => dest.Transacao, opt => opt.MapFrom(src => src.Transacao));

            CreateMap<TransacaoViewModel, Transacao>();
        }

    }
}
