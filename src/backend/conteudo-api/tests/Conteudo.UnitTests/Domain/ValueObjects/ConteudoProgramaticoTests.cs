using Conteudo.Domain.ValueObjects;

namespace Conteudo.UnitTests.Domain.ValueObjects;

public class ConteudoProgramaticoTests : TestBase
{
    [Fact]
    public void Construtor_ComDadosValidos_DeveCriarConteudoProgramatico()
    {
        // Arrange
        var resumo = "Resumo do curso";
        var descricao = "Descrição detalhada do curso";
        var objetivos = "Objetivos de aprendizagem";
        var preRequisitos = "Pré-requisitos básicos";
        var publicoAlvo = "Público-alvo";
        var metodologia = "Metodologia de ensino";
        var recursos = "Recursos necessários";
        var avaliacao = "Avaliação contínua";
        var bibliografia = "Bibliografia recomendada";

        // Act
        var conteudo = new ConteudoProgramatico(
            resumo, descricao, objetivos, preRequisitos, publicoAlvo,
            metodologia, recursos, avaliacao, bibliografia);

        // Assert
        conteudo.Should().NotBeNull();
        conteudo.Resumo.Should().Be(resumo);
        conteudo.Descricao.Should().Be(descricao);
        conteudo.Objetivos.Should().Be(objetivos);
        conteudo.PreRequisitos.Should().Be(preRequisitos);
        conteudo.PublicoAlvo.Should().Be(publicoAlvo);
        conteudo.Metodologia.Should().Be(metodologia);
        conteudo.Recursos.Should().Be(recursos);
        conteudo.Avaliacao.Should().Be(avaliacao);
        conteudo.Bibliografia.Should().Be(bibliografia);
    }

    [Fact]
    public void Construtor_ComResumoVazio_DeveLancarExcecao()
    {
        // Arrange & Act & Assert
        var action = () => new ConteudoProgramatico("", "Descrição", "Objetivos", "Pré-requisitos", "Público-alvo", "Metodologia", "Recursos", "Avaliação", "Bibliografia");
        action.Should().Throw<ArgumentException>().WithMessage("Resumo é obrigatório*");
    }

    [Fact]
    public void Construtor_ComDescricaoVazia_DeveLancarExcecao()
    {
        // Arrange & Act & Assert
        var action = () => new ConteudoProgramatico("Resumo", "", "Objetivos", "Pré-requisitos", "Público-alvo", "Metodologia", "Recursos", "Avaliação", "Bibliografia");
        action.Should().Throw<ArgumentException>().WithMessage("Descrição é obrigatória*");
    }

    [Fact]
    public void Construtor_ComObjetivosVazios_DeveLancarExcecao()
    {
        // Arrange & Act & Assert
        var action = () => new ConteudoProgramatico("Resumo", "Descrição", "", "Pré-requisitos", "Público-alvo", "Metodologia", "Recursos", "Avaliação", "Bibliografia");
        action.Should().Throw<ArgumentException>().WithMessage("Objetivos são obrigatórios*");
    }

    [Fact]
    public void Construtor_ComCamposOpcionaisNulos_DeveDefinirComoStringVazia()
    {
        // Arrange
        var resumo = "Resumo";
        var descricao = "Descrição";
        var objetivos = "Objetivos";

        // Act
        var conteudo = new ConteudoProgramatico(
            resumo, descricao, objetivos, null, null, null, null, null, null);

        // Assert
        conteudo.PreRequisitos.Should().Be(string.Empty);
        conteudo.PublicoAlvo.Should().Be(string.Empty);
        conteudo.Metodologia.Should().Be(string.Empty);
        conteudo.Recursos.Should().Be(string.Empty);
        conteudo.Avaliacao.Should().Be(string.Empty);
        conteudo.Bibliografia.Should().Be(string.Empty);
    }

    [Fact]
    public void Atualizar_DeveRetornarNovoObjeto()
    {
        // Arrange
        var conteudoOriginal = new ConteudoProgramatico(
            "Resumo Original", "Descrição Original", "Objetivos Originais", "Pré-requisitos", "Público-alvo", "Metodologia", "Recursos", "Avaliação", "Bibliografia");

        var novoResumo = "Novo Resumo";
        var novaDescricao = "Nova Descrição";
        var novosObjetivos = "Novos Objetivos";
        var novosPreRequisitos = "Novos Pré-requisitos";
        var novoPublicoAlvo = "Novo Público-alvo";
        var novaMetodologia = "Nova Metodologia";
        var novosRecursos = "Novos Recursos";
        var novaAvaliacao = "Nova Avaliação";
        var novaBibliografia = "Nova Bibliografia";

        // Act
        var novoConteudo = conteudoOriginal.Atualizar(
            novoResumo, novaDescricao, novosObjetivos, novosPreRequisitos,
            novoPublicoAlvo, novaMetodologia, novosRecursos, novaAvaliacao, novaBibliografia);

        // Assert
        novoConteudo.Should().NotBeNull();
        novoConteudo.Should().NotBeSameAs(conteudoOriginal);
        novoConteudo.Resumo.Should().Be(novoResumo);
        novoConteudo.Descricao.Should().Be(novaDescricao);
        novoConteudo.Objetivos.Should().Be(novosObjetivos);
        novoConteudo.PreRequisitos.Should().Be(novosPreRequisitos);
        novoConteudo.PublicoAlvo.Should().Be(novoPublicoAlvo);
        novoConteudo.Metodologia.Should().Be(novaMetodologia);
        novoConteudo.Recursos.Should().Be(novosRecursos);
        novoConteudo.Avaliacao.Should().Be(novaAvaliacao);
        novoConteudo.Bibliografia.Should().Be(novaBibliografia);

        // Verificar que o objeto original não foi alterado
        conteudoOriginal.Resumo.Should().Be("Resumo Original");
        conteudoOriginal.Descricao.Should().Be("Descrição Original");
        conteudoOriginal.Objetivos.Should().Be("Objetivos Originais");
    }

    [Fact]
    public void Equals_ComObjetosIguais_DeveRetornarTrue()
    {
        // Arrange
        var conteudo1 = new ConteudoProgramatico(
            "Resumo", "Descrição", "Objetivos", "Pré-requisitos", "Público-alvo",
            "Metodologia", "Recursos", "Avaliação", "Bibliografia");

        var conteudo2 = new ConteudoProgramatico(
            "Resumo", "Descrição", "Objetivos", "Pré-requisitos", "Público-alvo",
            "Metodologia", "Recursos", "Avaliação", "Bibliografia");

        // Act & Assert
        conteudo1.Equals(conteudo2).Should().BeTrue();
    }

    [Fact]
    public void Equals_ComObjetosDiferentes_DeveRetornarFalse()
    {
        // Arrange
        var conteudo1 = new ConteudoProgramatico(
            "Resumo 1", "Descrição", "Objetivos", "Pré-requisitos", "Público-alvo",
            "Metodologia", "Recursos", "Avaliação", "Bibliografia");

        var conteudo2 = new ConteudoProgramatico(
            "Resumo 2", "Descrição", "Objetivos", "Pré-requisitos", "Público-alvo",
            "Metodologia", "Recursos", "Avaliação", "Bibliografia");

        // Act & Assert
        conteudo1.Equals(conteudo2).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_DeveRetornarHashCodeConsistente()
    {
        // Arrange
        var conteudo1 = new ConteudoProgramatico(
            "Resumo", "Descrição", "Objetivos", "Pré-requisitos", "Público-alvo",
            "Metodologia", "Recursos", "Avaliação", "Bibliografia");

        var conteudo2 = new ConteudoProgramatico(
            "Resumo", "Descrição", "Objetivos", "Pré-requisitos", "Público-alvo",
            "Metodologia", "Recursos", "Avaliação", "Bibliografia");

        // Act & Assert
        conteudo1.GetHashCode().Should().Be(conteudo2.GetHashCode());
    }
}
