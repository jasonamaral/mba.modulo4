using Core.Messages;
using FluentValidation.Results;

namespace Core.Tests.Messages;

public class EventRaizTests : TestBase
{
    private class EventoTeste : EventRaiz
    {
        public string Nome { get; set; } = string.Empty;
    }

    [Fact]
    public void EventRaiz_DeveCriarComPropriedadesPadrao()
    {
        // Arrange & Act
        var evento = new EventoTeste();

        // Assert
        evento.Should().NotBeNull();
        evento.RaizAgregacao.Should().Be(Guid.Empty);
        evento.DataHora.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        evento.Validacao.Should().BeNull();
        evento.Erros.Should().BeEmpty();
        evento.EhValido().Should().BeTrue();
    }

    [Fact]
    public void EventRaiz_DeveDefinirRaizAgregacao()
    {
        // Arrange
        var evento = new EventoTeste();
        var raizAgregacao = Guid.NewGuid();

        // Act
        evento.DefinirRaizAgregacao(raizAgregacao);

        // Assert
        evento.RaizAgregacao.Should().Be(raizAgregacao);
    }

    [Fact]
    public void EventRaiz_DeveDefinirValidacao()
    {
        // Arrange
        var evento = new EventoTeste();
        var validacao = new ValidationResult();
        validacao.Errors.Add(new ValidationFailure("", "Erro de teste"));

        // Act
        evento.DefinirValidacao(validacao);

        // Assert
        evento.Validacao.Should().Be(validacao);
        evento.Erros.Should().Contain("Erro de teste");
        evento.EhValido().Should().BeFalse();
    }

    [Fact]
    public void EventRaiz_DeveSerValidoSemValidacao()
    {
        // Arrange
        var evento = new EventoTeste();

        // Act & Assert
        evento.EhValido().Should().BeTrue();
        evento.Erros.Should().BeEmpty();
    }

    [Fact]
    public void EventRaiz_DeveSerValidoComValidacaoValida()
    {
        // Arrange
        var evento = new EventoTeste();
        var validacao = new ValidationResult();

        // Act
        evento.DefinirValidacao(validacao);

        // Assert
        evento.EhValido().Should().BeTrue();
        evento.Erros.Should().BeEmpty();
    }

    [Fact]
    public void EventRaiz_DeveSerInvalidoComValidacaoInvalida()
    {
        // Arrange
        var evento = new EventoTeste();
        var validacao = new ValidationResult();
        validacao.Errors.Add(new ValidationFailure("", "Erro 1"));
        validacao.Errors.Add(new ValidationFailure("", "Erro 2"));

        // Act
        evento.DefinirValidacao(validacao);

        // Assert
        evento.EhValido().Should().BeFalse();
        evento.Erros.Should().HaveCount(2);
        evento.Erros.Should().Contain("Erro 1");
        evento.Erros.Should().Contain("Erro 2");
    }

    [Fact]
    public void EventRaiz_DeveManterDataHoraOriginal()
    {
        // Arrange
        var evento = new EventoTeste();
        var dataHoraOriginal = evento.DataHora;

        // Act
        Thread.Sleep(100);
        evento.DefinirRaizAgregacao(Guid.NewGuid());

        // Assert
        evento.DataHora.Should().Be(dataHoraOriginal);
    }

    [Fact]
    public void EventRaiz_DevePermitirValidacaoNull()
    {
        // Arrange
        var evento = new EventoTeste();

        // Act
        evento.DefinirValidacao(null!);

        // Assert
        evento.Validacao.Should().BeNull();
        evento.Erros.Should().BeEmpty();
        evento.EhValido().Should().BeTrue();
    }
}
