using Mapster;
using Pagamentos.Application.Interfaces;
using Pagamentos.Application.ViewModels;
using Pagamentos.Domain.Interfaces;

namespace Pagamentos.Application.Services
{
    public class PagamentoAppService : IPagamentoConsultaAppService, IPagamentoComandoAppService
    {

        private readonly IPagamentoRepository _pagamentoRepository;

        public PagamentoAppService(IPagamentoRepository pagamentoRepository)
        {
            _pagamentoRepository = pagamentoRepository;
        }

        public async Task<PagamentoViewModel> ObterPorId(Guid id)
        {
            var pagamento = await _pagamentoRepository.ObterPorId(id);
            return pagamento.Adapt<PagamentoViewModel>();
        }

        public async Task<IEnumerable<PagamentoViewModel>> ObterTodos()
        {
            var pagamentos = await _pagamentoRepository.ObterTodos();

            return pagamentos.Adapt<IEnumerable<PagamentoViewModel>>();
        }

        public void Dispose()
        {
            _pagamentoRepository?.Dispose();
        }
    }
}
