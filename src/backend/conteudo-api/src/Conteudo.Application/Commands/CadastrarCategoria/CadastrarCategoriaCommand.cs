using Core.Messages;

namespace Conteudo.Application.Commands.CadastrarCategoria;

public class CadastrarCategoriaCommand : CommandRaiz
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Cor { get; set; } = string.Empty;
    public string IconeUrl { get; set; } = string.Empty;
    public int Ordem { get; set; }

    public CadastrarCategoriaCommand()
    {
        // TODO :: Karina, podemos falar depois deste ponto? O Command precisa de uma raiz de agregação, normalmente o ID
        DefinirRaizAgregacao(Guid.NewGuid());
    }
}
