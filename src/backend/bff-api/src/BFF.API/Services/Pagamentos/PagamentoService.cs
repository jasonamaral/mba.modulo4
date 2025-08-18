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

            var apiResponse = await _apiClient.PostAsyncWithDetails<PagamentoCursoInputModel, ResponseResult<object>>("/api/v1/pagamentos/pagamento", pagamentoCursoInputModel);

            if (apiResponse.IsSuccess)
            {
                return new ResponseResult<bool> { Status = 200, Data = true };
            }

            if (!string.IsNullOrEmpty(apiResponse.ErrorContent))
            {
                try
                {
                    return new ResponseResult<bool>
                    {
                        Status = 400,
                        Errors = new ResponseErrorMessages { Mensagens = new List<string> { apiResponse.ErrorContent } }
                    };
                }
                catch
                {
                    return new ResponseResult<bool>
                    {
                        Status = apiResponse.StatusCode,
                        Errors = new ResponseErrorMessages { Mensagens = new List<string> { apiResponse.ErrorContent } }
                    };
                }
            }

            return new ResponseResult<bool>
            {
                Status = apiResponse.StatusCode,
                Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Erro desconhecido na API" } }
            };

        }
    }
}
