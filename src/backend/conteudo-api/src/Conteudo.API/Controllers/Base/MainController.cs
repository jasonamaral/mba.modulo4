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
                return new ObjectResult(new ApiSuccess
                {
                    Message = message ?? "Operação realizada com sucesso",
                    Data = data
                })
                {
                    StatusCode = (int)statusCode
                };
                    
            }

            return BadRequest(new ApiError
            {
                Details = notificador.ObterErros(),
                Message = message ?? "Ocorreu um ou mais erros durante a operação",
            });
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

    }
}
