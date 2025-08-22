using Core.Messages;

namespace Conteudo.Application.Commands.PublicarAula
{
    public class PublicarAulaCommand : CommandRaiz
    {
        public Guid CursoId { get; set; }
        public Guid Id { get; private set; }

        public PublicarAulaCommand(Guid cursoId, Guid id)
        {
            CursoId = cursoId;
            Id = id;
            DefinirRaizAgregacao(Id);
        }
    }
}
