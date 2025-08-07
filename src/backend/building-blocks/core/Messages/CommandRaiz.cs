using Core.Communication;
using FluentValidation.Results;
using MediatR;

namespace Core.Messages;

public abstract class CommandRaiz : IRequest<CommandResult>
{
    public Guid RaizAgregacao { get; private set; }
    public DateTime DataHora { get; } = DateTime.UtcNow;
    public ValidationResult Validacao { get; private set; } = new();
    public CommandResult Resultado => new(Validacao);

    public void DefinirRaizAgregacao(Guid raizAgregacao) => RaizAgregacao = raizAgregacao;

    public void DefinirValidacao(ValidationResult validacao) => Validacao = validacao;

    public IEnumerable<string> Erros => Validacao?.Errors?.Select(e => e.ErrorMessage) ?? Enumerable.Empty<string>();

    public bool EhValido() => Validacao?.IsValid != false;
}
