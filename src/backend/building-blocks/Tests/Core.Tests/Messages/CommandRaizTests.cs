using Core.Messages;
using FluentValidation.Results;

namespace Core.Tests.Messages;

public class CommandRaizTests : TestBase
{
    private class ComandoTeste : CommandRaiz
    {
        public string Nome { get; set; } = string.Empty;
    }

    [Fact]
    public void CommandRaiz_DeveCriarComPropriedadesPadrao()
    {
        // Arrange & Act
        var comando = new ComandoTeste();

        // Assert
        comando.Should().NotBeNull();
        comando.RaizAgregacao.Should().Be(Guid.Empty);
        comando.DataHora.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        comando.Validacao.Should().NotBeNull();
        comando.Resultado.Should().NotBeNull();
        comando.Erros.Should().BeEmpty();
        comando.EhValido().Should().BeTrue();
    }

    [Fact]
    public void CommandRaiz_DeveDefinirRaizAgregacao()
    {
        // Arrange
        var comando = new ComandoTeste();
        var raizAgregacao = Guid.NewGuid();

        // Act
        comando.DefinirRaizAgregacao(raizAgregacao);

        // Assert
        comando.RaizAgregacao.Should().Be(raizAgregacao);
    }

    [Fact]
    public void CommandRaiz_DeveDefinirValidacao()
    {
        // Arrange
        var comando = new ComandoTeste();
        var validacao = new ValidationResult();
        validacao.Errors.Add(new ValidationFailure("", "Erro de teste"));

        // Act
        comando.DefinirValidacao(validacao);

        // Assert
        comando.Validacao.Should().Be(validacao);
        comando.Resultado.ObterValidationResult().Should().Be(validacao);
        comando.Erros.Should().Contain("Erro de teste");
        comando.EhValido().Should().BeFalse();
    }

    [Fact]
    public void CommandRaiz_DeveSerValidoSemErros()
    {
        // Arrange
        var comando = new ComandoTeste();

        // Act & Assert
        comando.EhValido().Should().BeTrue();
        comando.Erros.Should().BeEmpty();
    }

    [Fact]
    public void CommandRaiz_DeveSerInvalidoComErros()
    {
        // Arrange
        var comando = new ComandoTeste();
        var validacao = new ValidationResult();
        validacao.Errors.Add(new ValidationFailure("", "Erro 1"));
        validacao.Errors.Add(new ValidationFailure("", "Erro 2"));

        // Act
        comando.DefinirValidacao(validacao);

        // Assert
        comando.EhValido().Should().BeFalse();
        comando.Erros.Should().HaveCount(2);
        comando.Erros.Should().Contain("Erro 1");
        comando.Erros.Should().Contain("Erro 2");
    }

    [Fact]
    public void CommandRaiz_DeveManterDataHoraOriginal()
    {
        // Arrange
        var comando = new ComandoTeste();
        var dataHoraOriginal = comando.DataHora;

        // Act
        Thread.Sleep(100); // Aguarda um pouco
        comando.DefinirRaizAgregacao(Guid.NewGuid());

        // Assert
        comando.DataHora.Should().Be(dataHoraOriginal);
    }

    [Fact]
    public void CommandRaiz_DevePermitirValidacaoNull()
    {
        // Arrange
        var comando = new ComandoTeste();

        // Act
        comando.DefinirValidacao(null!);

        // Assert
        comando.Validacao.Should().BeNull();
        comando.Erros.Should().BeEmpty();
        comando.EhValido().Should().BeTrue();
    }
}
