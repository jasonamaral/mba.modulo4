using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace Core.Services.Controllers;

[ApiController]
public abstract class MainController(IMediatorHandler mediator
                                   , INotificationHandler<DomainNotificacaoRaiz> notifications
                                   , INotificador notificador = null) : ControllerBase
{
    protected readonly DomainNotificacaoHandler _notifications = (DomainNotificacaoHandler)notifications;
    protected readonly INotificador _notificador = notificador ?? new Notificador();

    protected bool OperacaoValida() => !_notifications.TemNotificacao() && !_notificador.TemErros();

    protected ActionResult RespostaPadraoApi<T>(HttpStatusCode statusCode = HttpStatusCode.OK, T? data = default, string? message = null)
    {
        if (OperacaoValida())
        {
            return StatusCode((int)statusCode, new ResponseResult<T>
            {
                Status = (int)statusCode,
                Title = message ?? string.Empty,
                Errors = new(),
                Data = data
            });
        }

        var mensagens = new List<string>();
        mensagens.AddRange(_notifications.ObterMensagens());
        mensagens.AddRange(_notificador.ObterErros());

        return BadRequest(new ResponseResult<T>
        {
            Status = (int)HttpStatusCode.BadRequest,
            Title = message ?? "Ocorreu um ou mais erros durante a operação",
            Errors = new ResponseErrorMessages
            {
                Mensagens = mensagens
            }
        });
    }

    protected ActionResult RespostaPadraoApi<T>(HttpStatusCode statusCode, string message)
    {
        return RespostaPadraoApi<T>(statusCode, default, message);
    }

    protected ActionResult RespostaPadraoApi<T>(ModelStateDictionary modelState)
    {
        foreach (var erro in modelState.Values.SelectMany(e => e.Errors))
        {
            mediator.PublicarNotificacaoDominio(new DomainNotificacaoRaiz("ModelState", erro.ErrorMessage));
        }

        return RespostaPadraoApi<T>(message: "Dados inválidos");
    }

    protected ActionResult RespostaPadraoApi<T>(ValidationResult validationResult)
    {
        foreach (var erro in validationResult.Errors)
        {
            mediator.PublicarNotificacaoDominio(new DomainNotificacaoRaiz("ValidationResult", erro.ErrorMessage));
        }

        return RespostaPadraoApi<T>();
    }

    protected ActionResult RespostaPadraoApi<T>(CommandResult result)
    {
        return RespostaPadraoApi(data: result.Data);
    }

    protected List<string> ObterMensagensDeErro()
    {
        var mensagens = new List<string>();
        if (_notifications.TemNotificacao())
        {
            mensagens.AddRange(_notifications.ObterMensagens());
        }
        if (_notificador.TemErros())
        {
            mensagens.AddRange(_notificador.ObterErros());
        }
        return mensagens;
    }
}