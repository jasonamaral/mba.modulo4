using Core.Communication;
using Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace Core.Mediator;

public class MediatorHandler : IMediatorHandler
{
    private readonly IMediator _mediator;

    public MediatorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ValidationResult> EnviarComando<T>(T comando) where T : CommandRaiz
    {
        var result = await _mediator.Send(comando);
        return result.ObterValidationResult();
    }

    public async Task<CommandResult> ExecutarComando<T>(T comando) where T : CommandRaiz
    {
        return await _mediator.Send(comando);
    }

    public async Task PublicarEvento<T>(T evento) where T : EventRaiz
    {
        await _mediator.Publish(evento);
    }

    public async Task PublicarNotificacaoDominio<T>(T notificacao) where T : DomainNotificacaoRaiz
    {
        await _mediator.Publish(notificacao);
    }
}