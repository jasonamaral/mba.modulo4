using BFF.API.Models.Request;
using BFF.API.Services.Conteudos;
using BFF.API.Settings;
using BFF.Application.Interfaces.Services;
using BFF.Infrastructure.Services;
using Core.Communication;
using Microsoft.Extensions.Options;

namespace BFF.API.Services.Pagamentos
{
    public class PagamentoService : BaseApiService, IPagamentoService
    {
        private readonly ApiSettings _apiSettings;
        public PagamentoService(IOptions<ApiSettings> apiSettings,
                                IApiClientService apiClient,
                                ILogger<ConteudoService> logger) : base(apiClient, logger)
        {
            _apiSettings = apiSettings.Value;
        }


        public async Task<ResponseResult<bool>> ExecutarPagamento(PagamentoCursoInputModel pagamentoCursoInputModel)
        {
            _apiClient.SetBaseAddress(_apiSettings.PagamentosApiUrl);

            var apiResponse = await _apiClient.PostAsyncWithDetails<PagamentoCursoInputModel, ResponseResult<bool>>("/api/v1/pagamentos/pagamento", pagamentoCursoInputModel);

            if (apiResponse.IsSuccess)
            {
                return apiResponse.Data;
            }

            return null;

        }
    }
}
