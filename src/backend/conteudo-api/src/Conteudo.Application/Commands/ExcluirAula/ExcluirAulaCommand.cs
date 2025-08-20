using Core.Communication;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands.ExcluirAula
{
    public class ExcluirAulaCommand : CommandRaiz, IRequest<CommandResult>
    {
        public Guid Id { get; }

        public ExcluirAulaCommand(Guid id)
        {
            Id = id;
            DefinirRaizAgregacao(Id);
        }
    }
}
