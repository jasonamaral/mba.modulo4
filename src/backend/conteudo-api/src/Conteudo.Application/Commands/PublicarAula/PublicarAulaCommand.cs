using Core.Messages;


namespace Conteudo.Application.Commands.PublicarAula
{
    public class PublicarAulaCommand : CommandRaiz
    {
        public Guid Id { get; private set; }

        public PublicarAulaCommand(Guid id)
        {
            Id = id;
            DefinirRaizAgregacao(Id);
        }
    }
}
