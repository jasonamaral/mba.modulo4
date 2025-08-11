using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Pagamentos.Core.Messages.CommonMessages.Notifications;
using System.Net;

namespace Pagamentos.API.Controllers
{
    public class MainController : ControllerBase
    {
        private readonly NotificationContext _notificationContext;
        private readonly IMediator _mediator;

        protected Guid UserId { get; set; }
        protected string UserName { get; set; }
        protected bool UserAdmin { get; set; }

        protected MainController(IMediator mediator, NotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
            _mediator = mediator;

        }

        protected bool OperacaoValida()
        {
            return !_notificationContext.TemNotificacao();
        }

        protected ActionResult CustomResponse(HttpStatusCode statusCode, object result = null)
        {
            if (OperacaoValida())
            {
                return new ObjectResult(result)
                {
                    StatusCode = Convert.ToInt32(statusCode),
                };
            }
            else
            {
                if ((int)statusCode >= 200 && (int)statusCode <= 299)
                {
                    return BadRequest(new
                    {
                        errors = _notificationContext.ObterNotificacoes().Select(n => n.Value)
                    });
                }

                return new ObjectResult(new
                {
                    errors = _notificationContext.ObterNotificacoes().Select(n => n.Value)
                })
                {
                    StatusCode = Convert.ToInt32(statusCode)
                };
            }

        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotificarErroModelInvalida(modelState);
            return CustomResponse(HttpStatusCode.OK);
        }

        protected void NotificarErroModelInvalida(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotificarErro(errorMsg);
            }
        }

        protected void NotificarErro(string mensagem)
        {
            _mediator.Publish(new DomainNotification(Guid.NewGuid().ToString(), mensagem), CancellationToken.None);
        }

    }
}
