using AutoMapper;
using Pagamentos.Application.Interfaces;
using Pagamentos.Application.ViewModels;
using Pagamentos.Domain.Interfaces;

namespace Pagamentos.Application.Services
{
    public class PagamentoAppService : IPagamentoConsultaAppService, IPagamentoComandoAppService
    {

        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IMapper _mapper;

        public PagamentoAppService(IPagamentoRepository pagamentoRepository,
                                   IMapper mapper)
        {
            _pagamentoRepository = pagamentoRepository;
            _mapper = mapper;
        }


        public async Task<PagamentoViewModel> ObterPorId(Guid id)
        {
            return _mapper.Map<PagamentoViewModel>(await _pagamentoRepository.ObterPorId(id));
        }

        public async Task<IEnumerable<PagamentoViewModel>> ObterTodos()
        {
            return _mapper.Map<IEnumerable<PagamentoViewModel>>(await _pagamentoRepository.ObterTodos());
        }

        public void Dispose()
        {
            _pagamentoRepository?.Dispose();
        }
    }
}
