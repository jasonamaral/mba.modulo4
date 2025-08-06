using Core.Notification;
using Core.Services.Controllers;
using Core.Mediator;
using Core.Messages;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace BFF.API.Controllers;

/// <summary>
/// Controller base para o BFF que herda do MainController para padronizar respostas
/// </summary>
[ApiController]
public abstract class BffController : MainController
{
    protected readonly INotificador _notificador;

    protected BffController(IMediatorHandler mediator, INotificationHandler<DomainNotificacaoRaiz> notifications, INotificador notificador) : base(mediator, notifications)
    {
        _notificador = notificador;
    }


    protected async Task<ActionResult> ProcessarRespostaApi(HttpResponseMessage response, string? successMessage = null)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            try
            {
                // Tenta deserializar como ResponseResult (formato das APIs)
                var responseResult = System.Text.Json.JsonSerializer.Deserialize<Core.Communication.ResponseResult<object>>(
                    content,
                    new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (responseResult != null)
                {
                    // Se for um ResponseResult, retorna os dados no formato padronizado
                    return RespostaPadraoApi<object>(
                        (System.Net.HttpStatusCode)responseResult.Status,
                        responseResult.Data,
                        successMessage ?? responseResult.Title
                    );
                }
                else
                {
                    // Se não for ResponseResult, retorna o conteúdo direto
                    return RespostaPadraoApi<object>(
                        response.StatusCode,
                        content,
                        successMessage ?? "Operação realizada com sucesso"
                    );
                }
            }
            catch
            {
                // Se falhar na deserialização, retorna o conteúdo direto
                return RespostaPadraoApi<object>(
                    response.StatusCode,
                    content,
                    successMessage ?? "Operação realizada com sucesso"
                );
            }
        }
        else
        {
            try
            {
                // Tenta deserializar como ResponseResult para erros
                var responseResult = System.Text.Json.JsonSerializer.Deserialize<Core.Communication.ResponseResult<object>>(
                    content,
                    new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (responseResult != null && responseResult.Errors?.Mensagens?.Any() == true)
                {
                    // Adiciona os erros ao notificador
                    foreach (var erro in responseResult.Errors.Mensagens)
                    {
                        _notificador.AdicionarErro(erro);
                    }
                }
                else
                {
                    // Se não conseguir extrair erros específicos, adiciona mensagem genérica
                    _notificador.AdicionarErro($"Erro na API externa: {response.StatusCode} - {content}");
                }
            }
            catch
            {
                // Se falhar na deserialização, adiciona mensagem genérica
                _notificador.AdicionarErro($"Erro na API externa: {response.StatusCode} - {content}");
            }

            return RespostaPadraoApi<object>(
                response.StatusCode,
                message: $"Erro na comunicação com a API externa: {response.StatusCode}"
            );
        }
    }

    protected ActionResult ProcessarErro(System.Net.HttpStatusCode statusCode, string message)
    {
        _notificador.AdicionarErro(message);
        return RespostaPadraoApi<object>(statusCode, message: message);
    }
}