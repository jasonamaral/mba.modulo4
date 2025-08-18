using BFF.API.Models.Request;
using BFF.Domain.DTOs;
using Core.Communication;
using Core.Communication.Filters;

namespace BFF.API.Services.Pagamentos
{
    public interface IPagamentoService
    {
        Task<ResponseResult<bool>> ExecutarPagamento(PagamentoCursoInputModel pagamentoCursoInput);
        Task<ResponseResult<IEnumerable<PagamentoDto>>> ObterTodos();
        Task<ResponseResult<PagamentoDto>>ObterPorIdPagamento(Guid idPagamento);

    }
}
