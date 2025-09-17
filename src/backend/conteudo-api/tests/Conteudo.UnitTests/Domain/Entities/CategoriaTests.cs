using Conteudo.Domain.Entities;
using Plataforma.Educacao.Core.Exceptions;

namespace Conteudo.UnitTests.Domain.Entities;

public class CategoriaTests : TestBase
{
    [Fact]
    public void Construtor_ComDadosValidos_DeveCriarCategoria()
    {
        // Arrange
        var nome = "Programação";
        var descricao = "Cursos de programação";
        var cor = "#FF0000";
        var iconeUrl = "https://example.com/icon.png";
        var ordem = 1;
        var inicioTeste = DateTime.UtcNow;

        // Act
        var categoria = new Categoria(nome, descricao, cor, iconeUrl, ordem);

        // Assert
        categoria.Should().NotBeNull();
        categoria.Nome.Should().Be(nome);
        categoria.Descricao.Should().Be(descricao);
        categoria.Cor.Should().Be(cor);
        categoria.IconeUrl.Should().Be(iconeUrl);
        categoria.Ordem.Should().Be(ordem);
        categoria.IsAtiva.Should().BeTrue();
        categoria.Id.Should().NotBeEmpty();
        categoria.CreatedAt.Should().BeAfter(inicioTeste).And.BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Construtor_ComNomeVazio_DeveLancarExcecao()
    {
        // Arrange & Act & Assert
        var action = () => new Categoria("", "Descrição", "#FF0000");
        action.Should().Throw<DomainException>().WithMessage("Nome da categoria é obrigatório");
    }

    [Fact]
    public void Construtor_ComNomeNulo_DeveLancarExcecao()
    {
        // Arrange & Act & Assert
        var action = () => new Categoria(null!, "Descrição", "#FF0000");
        action.Should().Throw<DomainException>().WithMessage("Nome da categoria é obrigatório");
    }

    [Fact]
    public void Construtor_ComNomeMuitoLongo_DeveLancarExcecao()
    {
        // Arrange
        var nomeMuitoLongo = new string('A', 101);

        // Act & Assert
        var action = () => new Categoria(nomeMuitoLongo, "Descrição", "#FF0000");
        action.Should().Throw<DomainException>().WithMessage("Nome da categoria não pode ter mais de 100 caracteres");
    }

    [Fact]
    public void Construtor_ComDescricaoVazia_DeveLancarExcecao()
    {
        // Arrange & Act & Assert
        var action = () => new Categoria("Nome", "", "#FF0000");
        action.Should().Throw<DomainException>().WithMessage("Descrição da categoria é obrigatória");
    }

    [Fact]
    public void Construtor_ComCorVazia_DeveLancarExcecao()
    {
        // Arrange & Act & Assert
        var action = () => new Categoria("Nome", "Descrição", "");
        action.Should().Throw<DomainException>().WithMessage("Cor da categoria é obrigatória");
    }

    [Fact]
    public void AtualizarInformacoes_ComDadosValidos_DeveAtualizarCategoria()
    {
        // Arrange
        var categoria = new Categoria("Nome Original", "Descrição Original", "#FF0000");
        var novoNome = "Novo Nome";
        var novaDescricao = "Nova Descrição";
        var novaCor = "#00FF00";
        var novoIconeUrl = "https://example.com/new-icon.png";
        var novaOrdem = 5;
        var inicioOperacao = DateTime.UtcNow;

        // Act
        categoria.AtualizarInformacoes(novoNome, novaDescricao, novaCor, novoIconeUrl, novaOrdem);

        // Assert
        categoria.Nome.Should().Be(novoNome);
        categoria.Descricao.Should().Be(novaDescricao);
        categoria.Cor.Should().Be(novaCor);
        categoria.IconeUrl.Should().Be(novoIconeUrl);
        categoria.Ordem.Should().Be(novaOrdem);
        categoria.UpdatedAt.Should().BeAfter(inicioOperacao).And.BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(3));
    }

    [Fact]
    public void Ativar_DeveAtivarCategoria()
    {
        // Arrange
        var categoria = new Categoria("Nome", "Descrição", "#FF0000");
        categoria.Desativar();
        var inicioOperacao = DateTime.UtcNow;

        // Act
        categoria.Ativar();

        // Assert
        categoria.IsAtiva.Should().BeTrue();
        categoria.UpdatedAt.Should().BeAfter(inicioOperacao).And.BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(3));
    }

    [Fact]
    public void Desativar_DeveDesativarCategoria()
    {
        // Arrange
        var categoria = new Categoria("Nome", "Descrição", "#FF0000");
        var inicioOperacao = DateTime.UtcNow;

        // Act
        categoria.Desativar();

        // Assert
        categoria.IsAtiva.Should().BeFalse();
        categoria.UpdatedAt.Should().BeAfter(inicioOperacao).And.BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(3));
    }

    [Fact]
    public void AlterarOrdem_ComOrdemValida_DeveAlterarOrdem()
    {
        // Arrange
        var categoria = new Categoria("Nome", "Descrição", "#FF0000");
        var novaOrdem = 10;
        var inicioOperacao = DateTime.UtcNow;

        // Act
        categoria.AlterarOrdem(novaOrdem);

        // Assert
        categoria.Ordem.Should().Be(novaOrdem);
        categoria.UpdatedAt.Should().BeAfter(inicioOperacao).And.BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(3));
    }

    [Fact]
    public void AlterarOrdem_ComOrdemNegativa_DeveLancarExcecao()
    {
        // Arrange
        var categoria = new Categoria("Nome", "Descrição", "#FF0000");

        // Act & Assert
        var action = () => categoria.AlterarOrdem(-1);
        action.Should().Throw<DomainException>().WithMessage("Ordem não pode ser negativa");
    }

    [Fact]
    public void Propriedades_DeveRetornarValoresCorretos()
    {
        // Arrange
        var categoria = new Categoria("Nome", "Descrição", "#FF0000");

        // Act & Assert
        categoria.TotalCursos.Should().Be(0);
        categoria.CursosAtivos.Should().Be(0);
        categoria.Cursos.Should().NotBeNull();
        categoria.Cursos.Should().BeEmpty();
    }
}
