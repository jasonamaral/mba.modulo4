using Core.Communication;
using Core.Notification;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace Conteudo.API.Controllers.Base
{
    [ApiController]
    public abstract class MainController(INotificador notificador) : ControllerBase
    {
        protected ActionResult RespostaPadraoApi(HttpStatusCode statusCode = HttpStatusCode.OK, object? data = null, string? message = null)
        {
            if (!notificador.TemErros())
            {
                return new ObjectResult(new ResponseResult
                {   
                    Status = (int)statusCode,
                    Title = message ?? "Operação realizada com sucesso",
                    Errors = new(),
                    Data = data
                });  
            }

            return BadRequest(new ResponseResult
            {
                Status = (int)HttpStatusCode.BadRequest,
                Title = message ?? "Ocorreu um ou mais erros durante a operação",
                Errors = new ResponseErrorMessages
                {
                    Mensagens = notificador.ObterErros()
                }
            });
        }
        protected ActionResult RespostaPadraoApi(HttpStatusCode statusCode, string message)
        {
            return RespostaPadraoApi(statusCode, null, message);
        }

        protected ActionResult RespostaPadraoApi(ModelStateDictionary modelState)
        {
            foreach (var erro in modelState.Values.SelectMany(e => e.Errors))
            {
                notificador.AdicionarErro(erro.ErrorMessage);
            }

            return RespostaPadraoApi(message: "Dados inválidos");
        }

        protected ActionResult RespostaPadraoApi(ValidationResult validationResult)
        {
            foreach (var erro in validationResult.Errors)
            {
                notificador.AdicionarErro(erro.ErrorMessage);
            }

            return RespostaPadraoApi();
        }

        protected ActionResult RespostaPadraoApi(CommandResult result)
        {
            foreach (var erro in result.ObterErros())
            {
                notificador.AdicionarErro(erro.ErrorMessage);
            }

            return RespostaPadraoApi(data: result.Data);
        }

    }
}
