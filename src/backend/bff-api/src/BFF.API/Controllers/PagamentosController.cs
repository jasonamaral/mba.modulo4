using BFF.API.Extensions;
using BFF.API.Models.Request;
using BFF.API.Models.Response;
using BFF.API.Settings;
using BFF.Application.Interfaces.Services;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.Messages.Integration;
using Core.Notification;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace BFF.API.Controllers
{
    /// <summary>
    /// Controller de Pagamentos no BFF - Orquestra chamadas para Pagamento API
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class PagamentosController : BffController
    {

        private readonly IApiClientService _apiClient;
        private readonly ApiSettings _apiSettings;
        private readonly ILogger<PagamentosController> _logger;

        public PagamentosController(IApiClientService apiClient,
                                    IOptions<ApiSettings> apiSettings,
                                    ILogger<PagamentosController> logger,
                                    IMediatorHandler mediator,
                                    INotificationHandler<DomainNotificacaoRaiz> notifications,
                                    INotificador notificador) : base(mediator, notifications, notificador)
        {
            _apiClient = apiClient;
            _apiSettings = apiSettings.Value;
            _logger = logger;
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPost("pagamento")]
        public async Task<IActionResult> Pagamento([FromBody] PagamentoCursoInputModel pagamento)
        {

            try
            {
                var userId = User.GetUserId();
                if (userId == Guid.Empty)
                {
                    return ProcessarErro(System.Net.HttpStatusCode.Unauthorized, "Token inválido");
                }

                var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                _apiClient.SetBaseAddress(_apiSettings.AlunosApiUrl);
                _apiClient.ClearDefaultHeaders();
                _apiClient.AddDefaultHeader("Authorization", $"Bearer {token}");
                _apiClient.AddDefaultHeader("Accept", "application/json");
                _apiClient.AddDefaultHeader("Content-Type", "application/json");

                var _pagamento = pagamento;

                var apiResponse = await _apiClient.PostAsyncWithDetails<CursoCriarRequest, ResponseResult<string>>("/api/v1/pagamentos/pagamento", null);

                if (apiResponse.IsSuccess)
                {
                    return null;
                    //return apiResponse.Data;
                }


                //if (response?.Data != null)
                //{
                //    return RespostaPadraoApi(System.Net.HttpStatusCode.OK, response.Data, "MSG SUCESSO TODO");
                //}

                return ProcessarErro(System.Net.HttpStatusCode.NotFound, "MSG ERRO TODO");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar pagamento via BFF");
                return ProcessarErro(System.Net.HttpStatusCode.InternalServerError, "Erro interno do servidor");
            }

        }


    }
}
