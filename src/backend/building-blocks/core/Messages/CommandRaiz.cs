using Core.Communication;
using FluentValidation.Results;
using MediatR;

namespace Core.Messages;
public abstract class CommandRaiz : IRequest<CommandResult>
{
    public Guid RaizAgregacao { get; internal set; }
    public DateTime DataHora { get; private set; }
    public CommandResult CommandResult { get; set; }
    public ValidationResult Validacao { get; internal set; }

    protected CommandRaiz()
    {
        DataHora = DateTime.UtcNow;
    }

    public void DefinirRaizAgregacao(Guid raizAgregacao)
    {
        RaizAgregacao = raizAgregacao;
    }

    public void DefinirValidacao(ValidationResult validacao)
    {
        Validacao = validacao;
    }

    public ICollection<string> Erros => Validacao?.Errors?.Select(e => e.ErrorMessage).ToList() ?? new List<string>();
    public virtual bool EhValido() => Validacao == null || Validacao.IsValid;
}
