using Core.Messages;

namespace Conteudo.Application.Commands.ExcluirCurso;

public class ExcluirCursoCommand : CommandRaiz
{
    public Guid Id { get; }

    public ExcluirCursoCommand(Guid id)
    {
        Id = id;
        DefinirRaizAgregacao(Id);
    }
}
