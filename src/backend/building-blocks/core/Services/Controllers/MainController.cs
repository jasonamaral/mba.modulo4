using Core.Communication;
using Core.Notification;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace Core.Services.Controllers
{
    [ApiController]
    public abstract class MainController(INotificador notificador) : ControllerBase
    {
        protected ActionResult RespostaPadraoApi<T>(HttpStatusCode statusCode = HttpStatusCode.OK, T? data = default, string? message = null)
        {
            if (!notificador.TemErros())
            {
                return new ObjectResult(new ResponseResult<T>
                {
                    Status = (int)statusCode,
                    Title = message ?? string.Empty,
                    Errors = new(),
                    Data = data
                });
            }

            return BadRequest(new ResponseResult<T>
            {
                Status = (int)HttpStatusCode.BadRequest,
                Title = message ?? "Ocorreu um ou mais erros durante a operação",
                Errors = new ResponseErrorMessages
                {
                    Mensagens = notificador.ObterErros()
                }
            });
        }
        protected ActionResult RespostaPadraoApi<T>(HttpStatusCode statusCode, string message)
        {
            return RespostaPadraoApi(statusCode,message);
        }

        protected ActionResult RespostaPadraoApi<T>(ModelStateDictionary modelState)
        {
            foreach (var erro in modelState.Values.SelectMany(e => e.Errors))
            {
                notificador.AdicionarErro(erro.ErrorMessage);
            }

            return RespostaPadraoApi<T>(message: "Dados inválidos");
        }

        protected ActionResult RespostaPadraoApi<T>(ValidationResult validationResult)
        {
            foreach (var erro in validationResult.Errors)
            {
                notificador.AdicionarErro(erro.ErrorMessage);
            }

            return RespostaPadraoApi<T>();
        }

        protected ActionResult RespostaPadraoApi<T>(CommandResult result)
        {
            foreach (var erro in result.ObterErros())
            {
                notificador.AdicionarErro(erro.ErrorMessage);
            }

            return RespostaPadraoApi(data: result);
        }

    }
}
