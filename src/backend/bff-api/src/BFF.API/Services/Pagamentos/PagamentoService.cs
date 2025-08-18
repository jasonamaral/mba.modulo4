using BFF.API.Models.Request;
using BFF.API.Settings;
using BFF.Application.Interfaces.Services;
using BFF.Domain.DTOs;
using BFF.Infrastructure.Services;
using Core.Communication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace BFF.API.Services.Pagamentos
{
    public class PagamentoService : BaseApiService, IPagamentoService
    {
        private readonly ApiSettings _apiSettings;
        private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

        public PagamentoService(
            IOptions<ApiSettings> apiSettings,
            IApiClientService apiClient,
            ILogger<PagamentoService> logger) : base(apiClient, logger)
        {
            _apiSettings = apiSettings?.Value ?? throw new ArgumentNullException(nameof(apiSettings));
        }

        public async Task<ResponseResult<bool>> ExecutarPagamento(PagamentoCursoInputModel pagamentoCursoInputModel)
        {
            if (pagamentoCursoInputModel is null)
            {
                return Fail(400, "Payload de pagamento não pode ser nulo.");
            }

            _apiClient.SetBaseAddress(_apiSettings.PagamentosApiUrl);

            ApiResponse<ResponseResult<object>> apiResponse;
            try
            {
                apiResponse = await _apiClient.PostAsyncWithDetails<PagamentoCursoInputModel, ResponseResult<object>>("/api/v1/pagamentos/pagamento",pagamentoCursoInputModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao chamar Pagamentos API.");
                return Fail(502, "Falha ao comunicar com a API de pagamentos.");
            }

            return apiResponse.IsSuccess ? Success() : MapApiError(apiResponse);
        }




        private static ResponseResult<bool> Success() => new()
        {
            Status = 200,
            Data = true
        };

        private static ResponseResult<bool> Fail(int status, params string[] mensagens) => new()
        {
            Status = status,
            Data = false,
            Errors = new ResponseErrorMessages { Mensagens = mensagens?.ToList() ?? new List<string>() }
        };

        private ResponseResult<bool> MapApiError(ApiResponse<ResponseResult<object>> apiResponse)
        {
            if (apiResponse.Data is not null &&
                apiResponse.Data.Errors?.Mensagens is { Count: > 0 })
            {
                return Fail(apiResponse.StatusCode, apiResponse.Data.Errors.Mensagens.ToArray());
            }

            var mensagens = ParseMensagens(apiResponse.ErrorContent);
            if (mensagens.Count > 0)
            {
                return Fail(apiResponse.StatusCode, mensagens.ToArray());
            }

            return Fail(apiResponse.StatusCode,
                        string.IsNullOrWhiteSpace(apiResponse.ErrorContent)
                            ? "Erro desconhecido na API."
                            : apiResponse.ErrorContent);
        }

        private static List<string> ParseMensagens(string? errorContent)
        {
            var msgs = new List<string>();
            if (string.IsNullOrWhiteSpace(errorContent)) return msgs;

            try
            {
                var rr = JsonSerializer.Deserialize<ResponseResult<object>>(errorContent, _jsonOptions);
                if (rr?.Errors?.Mensagens is { Count: > 0 })
                {
                    msgs.AddRange(rr.Errors.Mensagens);
                    return msgs;
                }
            }
            catch {}

            try
            {
                var em = JsonSerializer.Deserialize<ResponseErrorMessages>(errorContent, _jsonOptions);
                if (em?.Mensagens is { Count: > 0 })
                {
                    msgs.AddRange(em.Mensagens);
                    return msgs;
                }
            }
            catch {}

            msgs.Add(errorContent);
            return msgs;
        }
    }
}