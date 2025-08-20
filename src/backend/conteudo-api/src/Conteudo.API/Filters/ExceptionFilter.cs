using Core.Communication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Conteudo.API.Filters;

public class ExceptionFilter(IActionResultExecutor<ObjectResult> executor, ILogger<ExceptionFilter> logger) : IExceptionFilter
{
    private readonly IActionResultExecutor<ObjectResult> _executor = executor;
    private readonly ILogger _logger = logger;

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is Exception ex)
        {
            context.ExceptionHandled = true;
            string erro = $"Erro: {context?.Exception?.Message ?? context.Exception?.ToString()}";
            _logger.LogError(context?.Exception, erro);

            var outputResponse = new
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Ops, aconteceu um erro inesperado",
                Errors = new ResponseErrorMessages { Mensagens = [erro] }
            };

            ObjectResult output = new ObjectResult(outputResponse)
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Value = outputResponse
            };

            _executor.ExecuteAsync(new ActionContext(context.HttpContext, context.RouteData, context.ActionDescriptor), output)
                .GetAwaiter()
                .GetResult();
        }
    }
}
