using Core.Notification;
using FluentAssertions;
using Xunit;

namespace Core.Tests.Notification;

public class NotificadorTests : TestBase
{
    [Fact]
    public void Notificador_DeveCriarSemNotificacoes()
    {
        // Arrange & Act
        var notificador = new Notificador();

        // Assert
        notificador.Should().NotBeNull();
        notificador.TemNotificacoes().Should().BeFalse();
        notificador.TemErros().Should().BeFalse();
        notificador.ObterErros().Should().BeEmpty();
        notificador.ObterInformacoes().Should().BeEmpty();
    }

    [Fact]
    public void Notificador_DeveAdicionarErro()
    {
        // Arrange
        var notificador = new Notificador();
        var mensagem = "Erro de validação";

        // Act
        notificador.AdicionarErro(mensagem);

        // Assert
        notificador.TemNotificacoes().Should().BeTrue();
        notificador.TemErros().Should().BeTrue();
        notificador.ObterErros().Should().Contain(mensagem);
        notificador.ObterInformacoes().Should().BeEmpty();
    }

    [Fact]
    public void Notificador_DeveAdicionarInformacao()
    {
        // Arrange
        var notificador = new Notificador();
        var mensagem = "Informação importante";

        // Act
        notificador.Adicionar(TipoNotificacao.Informacao, mensagem);

        // Assert
        notificador.TemNotificacoes().Should().BeTrue();
        notificador.TemErros().Should().BeFalse();
        notificador.ObterInformacoes().Should().Contain(mensagem);
        notificador.ObterErros().Should().BeEmpty();
    }

    [Fact]
    public void Notificador_DeveAdicionarMultiplasNotificacoes()
    {
        // Arrange
        var notificador = new Notificador();
        var erro1 = "Erro 1";
        var erro2 = "Erro 2";
        var info1 = "Info 1";

        // Act
        notificador.AdicionarErro(erro1);
        notificador.AdicionarErro(erro2);
        notificador.Adicionar(TipoNotificacao.Informacao, info1);

        // Assert
        notificador.TemNotificacoes().Should().BeTrue();
        notificador.TemErros().Should().BeTrue();
        notificador.ObterErros().Should().HaveCount(2);
        notificador.ObterErros().Should().Contain(erro1);
        notificador.ObterErros().Should().Contain(erro2);
        notificador.ObterInformacoes().Should().HaveCount(1);
        notificador.ObterInformacoes().Should().Contain(info1);
    }

    [Fact]
    public void Notificador_DeveAdicionarErroComMetodoGenerico()
    {
        // Arrange
        var notificador = new Notificador();
        var mensagem = "Erro genérico";

        // Act
        notificador.Adicionar(TipoNotificacao.Erro, mensagem);

        // Assert
        notificador.TemErros().Should().BeTrue();
        notificador.ObterErros().Should().Contain(mensagem);
    }

    [Fact]
    public void Notificador_DevePermitirMensagemVazia()
    {
        // Arrange
        var notificador = new Notificador();

        // Act
        notificador.AdicionarErro(string.Empty);
        notificador.Adicionar(TipoNotificacao.Informacao, string.Empty);

        // Assert
        notificador.TemNotificacoes().Should().BeTrue();
        notificador.ObterErros().Should().Contain(string.Empty);
        notificador.ObterInformacoes().Should().Contain(string.Empty);
    }

    [Fact]
    public void Notificador_DevePermitirMensagemNull()
    {
        // Arrange
        var notificador = new Notificador();

        // Act
        notificador.AdicionarErro(null!);
        notificador.Adicionar(TipoNotificacao.Informacao, null!);

        // Assert
        notificador.TemNotificacoes().Should().BeTrue();
        notificador.ObterErros().Should().Contain((string?)null);
        notificador.ObterInformacoes().Should().Contain((string?)null);
    }

    [Fact]
    public void Notificador_DeveRetornarListasVaziasQuandoSemNotificacoes()
    {
        // Arrange
        var notificador = new Notificador();

        // Act & Assert
        notificador.ObterErros().Should().BeEmpty();
        notificador.ObterInformacoes().Should().BeEmpty();
    }

    [Fact]
    public void Notificador_DeveManterNotificacoesIndependentes()
    {
        // Arrange
        var notificador1 = new Notificador();
        var notificador2 = new Notificador();

        // Act
        notificador1.AdicionarErro("Erro 1");
        notificador2.Adicionar(TipoNotificacao.Informacao, "Info 2");

        // Assert
        notificador1.TemErros().Should().BeTrue();
        notificador1.TemNotificacoes().Should().BeTrue();
        notificador2.TemErros().Should().BeFalse();
        notificador2.TemNotificacoes().Should().BeTrue();
    }
}
