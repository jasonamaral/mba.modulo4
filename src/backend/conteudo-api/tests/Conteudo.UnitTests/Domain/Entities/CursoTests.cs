using Conteudo.Domain.Entities;
using Conteudo.Domain.ValueObjects;
using Plataforma.Educacao.Core.Exceptions;

namespace Conteudo.UnitTests.Domain.Entities;

public class CursoTests : TestBase
{
    private readonly ConteudoProgramatico _conteudoProgramatico;

    public CursoTests()
    {
        _conteudoProgramatico = new ConteudoProgramatico(
            "Resumo do curso",
            "Descrição detalhada do curso",
            "Objetivos de aprendizagem",
            "Pré-requisitos básicos",
            "Público-alvo",
            "Metodologia de ensino",
            "Recursos necessários",
            "Avaliação contínua",
            "Bibliografia recomendada");
    }

    [Fact]
    public void Construtor_ComDadosValidos_DeveCriarCurso()
    {
        // Arrange
        var nome = "Curso de C#";
        var valor = 299.99m;
        var duracaoHoras = 40;
        var nivel = "Intermediário";
        var instrutor = "João Silva";
        var vagasMaximas = 30;
        var inicioTeste = DateTime.UtcNow;

        // Act
        var curso = new Curso(nome, valor, _conteudoProgramatico, duracaoHoras, nivel, instrutor, vagasMaximas);

        // Assert
        curso.Should().NotBeNull();
        curso.Nome.Should().Be(nome);
        curso.Valor.Should().Be(valor);
        curso.ConteudoProgramatico.Should().Be(_conteudoProgramatico);
        curso.DuracaoHoras.Should().Be(duracaoHoras);
        curso.Nivel.Should().Be(nivel);
        curso.Instrutor.Should().Be(instrutor);
        curso.VagasMaximas.Should().Be(vagasMaximas);
        curso.VagasOcupadas.Should().Be(0);
        curso.Ativo.Should().BeTrue();
        curso.Id.Should().NotBeEmpty();
        curso.CreatedAt.Should().BeAfter(inicioTeste).And.BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Construtor_ComNomeVazio_DeveLancarExcecao()
    {
        // Arrange & Act & Assert
        var action = () => new Curso("", 299.99m, _conteudoProgramatico, 40, "Intermediário", "João Silva", 30);
        action.Should().Throw<DomainException>().WithMessage("Nome do curso é obrigatório");
    }

    [Fact]
    public void Construtor_ComNomeMuitoLongo_DeveLancarExcecao()
    {
        // Arrange
        var nomeMuitoLongo = new string('A', 201);

        // Act & Assert
        var action = () => new Curso(nomeMuitoLongo, 299.99m, _conteudoProgramatico, 40, "Intermediário", "João Silva", 30);
        action.Should().Throw<DomainException>().WithMessage("Nome do curso não pode ter mais de 200 caracteres");
    }

    [Fact]
    public void Construtor_ComValorNegativo_DeveLancarExcecao()
    {
        // Arrange & Act & Assert
        var action = () => new Curso("Curso de C#", -100m, _conteudoProgramatico, 40, "Intermediário", "João Silva", 30);
        action.Should().Throw<DomainException>().WithMessage("Valor do curso não pode ser negativo");
    }

    [Fact]
    public void Construtor_ComConteudoProgramaticoNulo_DeveLancarExcecao()
    {
        // Arrange & Act & Assert
        var action = () => new Curso("Curso de C#", 299.99m, null!, 40, "Intermediário", "João Silva", 30);
        action.Should().Throw<DomainException>().WithMessage("Conteúdo programático é obrigatório");
    }

    [Fact]
    public void Construtor_ComDuracaoInvalida_DeveLancarExcecao()
    {
        // Arrange & Act & Assert
        var action = () => new Curso("Curso de C#", 299.99m, _conteudoProgramatico, 0, "Intermediário", "João Silva", 30);
        action.Should().Throw<DomainException>().WithMessage("Duração do curso deve ser maior que zero");
    }

    [Fact]
    public void Construtor_ComNivelVazio_DeveLancarExcecao()
    {
        // Arrange & Act & Assert
        var action = () => new Curso("Curso de C#", 299.99m, _conteudoProgramatico, 40, "", "João Silva", 30);
        action.Should().Throw<DomainException>().WithMessage("Nível do curso é obrigatório");
    }

    [Fact]
    public void Construtor_ComInstrutorVazio_DeveLancarExcecao()
    {
        // Arrange & Act & Assert
        var action = () => new Curso("Curso de C#", 299.99m, _conteudoProgramatico, 40, "Intermediário", "", 30);
        action.Should().Throw<DomainException>().WithMessage("Instrutor é obrigatório");
    }

    [Fact]
    public void Construtor_ComVagasMaximasInvalida_DeveLancarExcecao()
    {
        // Arrange & Act & Assert
        var action = () => new Curso("Curso de C#", 299.99m, _conteudoProgramatico, 40, "Intermediário", "João Silva", 0);
        action.Should().Throw<DomainException>().WithMessage("Número de vagas deve ser maior que zero");
    }

    [Fact]
    public void AtualizarInformacoes_ComDadosValidos_DeveAtualizarCurso()
    {
        // Arrange
        var curso = new Curso("Nome Original", 100m, _conteudoProgramatico, 20, "Básico", "Instrutor Original", 25);
        var novoNome = "Novo Nome";
        var novoValor = 399.99m;
        var novaDuracao = 60;
        var novoNivel = "Avançado";
        var novoInstrutor = "Novo Instrutor";
        var novasVagas = 40;
        var inicioOperacao = DateTime.UtcNow;

        // Act
        curso.AtualizarInformacoes(novoNome, novoValor, _conteudoProgramatico, novaDuracao, novoNivel, novoInstrutor, novasVagas);

        // Assert
        curso.Nome.Should().Be(novoNome);
        curso.Valor.Should().Be(novoValor);
        curso.DuracaoHoras.Should().Be(novaDuracao);
        curso.Nivel.Should().Be(novoNivel);
        curso.Instrutor.Should().Be(novoInstrutor);
        curso.VagasMaximas.Should().Be(novasVagas);
        curso.UpdatedAt.Should().BeAfter(inicioOperacao).And.BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(3));
    }

    [Fact]
    public void Ativar_DeveAtivarCurso()
    {
        // Arrange
        var curso = new Curso("Curso", 100m, _conteudoProgramatico, 20, "Básico", "Instrutor", 25);
        curso.Desativar();
        var inicioOperacao = DateTime.UtcNow;

        // Act
        curso.Ativar();

        // Assert
        curso.Ativo.Should().BeTrue();
        curso.UpdatedAt.Should().BeAfter(inicioOperacao).And.BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(3));
    }

    [Fact]
    public void Desativar_DeveDesativarCurso()
    {
        // Arrange
        var curso = new Curso("Curso", 100m, _conteudoProgramatico, 20, "Básico", "Instrutor", 25);
        var inicioOperacao = DateTime.UtcNow;

        // Act
        curso.Desativar();

        // Assert
        curso.Ativo.Should().BeFalse();
        curso.UpdatedAt.Should().BeAfter(inicioOperacao).And.BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(3));
    }

    [Fact]
    public void AdicionarMatricula_ComVagasDisponiveis_DeveIncrementarVagasOcupadas()
    {
        // Arrange
        var curso = new Curso("Curso", 100m, _conteudoProgramatico, 20, "Básico", "Instrutor", 25);
        var inicioOperacao = DateTime.UtcNow;

        // Act
        curso.AdicionarMatricula();

        // Assert
        curso.VagasOcupadas.Should().Be(1);
        curso.UpdatedAt.Should().BeAfter(inicioOperacao).And.BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(3));
    }

    [Fact]
    public void AdicionarMatricula_SemVagasDisponiveis_DeveLancarExcecao()
    {
        // Arrange
        var curso = new Curso("Curso", 100m, _conteudoProgramatico, 20, "Básico", "Instrutor", 1);
        curso.AdicionarMatricula(); // Ocupa a única vaga

        // Act & Assert
        var action = () => curso.AdicionarMatricula();
        action.Should().Throw<DomainException>().WithMessage("Não há vagas disponíveis para este curso");
    }

    [Fact]
    public void RemoverMatricula_DeveDecrementarVagasOcupadas()
    {
        // Arrange
        var curso = new Curso("Curso", 100m, _conteudoProgramatico, 20, "Básico", "Instrutor", 25);
        curso.AdicionarMatricula();
        curso.AdicionarMatricula();
        var inicioOperacao = DateTime.UtcNow;

        // Act
        curso.RemoverMatricula();

        // Assert
        curso.VagasOcupadas.Should().Be(1);
        curso.UpdatedAt.Should().BeAfter(inicioOperacao).And.BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(3));
    }

    [Fact]
    public void PropriedadesCalculadas_DeveRetornarValoresCorretos()
    {
        // Arrange
        var curso = new Curso("Curso", 100m, _conteudoProgramatico, 20, "Básico", "Instrutor", 25);
        curso.AdicionarMatricula();
        curso.AdicionarMatricula();

        // Act & Assert
        curso.TemVagasDisponiveis.Should().BeTrue();
        curso.VagasDisponiveis.Should().Be(23);
        curso.EstaExpirado.Should().BeFalse();
        curso.PodeSerMatriculado.Should().BeTrue();
    }

    [Fact]
    public void CursoExpirado_DeveRetornarEstaExpiradoTrue()
    {
        // Arrange
        var curso = new Curso("Curso", 100m, _conteudoProgramatico, 20, "Básico", "Instrutor", 25, "", DateTime.UtcNow.AddDays(-1));

        // Act & Assert
        curso.EstaExpirado.Should().BeTrue();
        curso.PodeSerMatriculado.Should().BeFalse();
    }
}
