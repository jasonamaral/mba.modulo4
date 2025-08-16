using BFF.API.Models.Request;
using Core.Communication;

namespace BFF.API.Services.Pagamentos
{
    public interface IPagamentoService
    {
        Task<ResponseResult<bool>> ExecutarPagamento(PagamentoCursoInputModel pagamentoCursoInput);
    }
}
