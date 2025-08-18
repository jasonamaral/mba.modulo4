using Alunos.Application.DTOs.Response;
using Alunos.Application.Interfaces;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using Core.Services.Controllers;
using MediatR;

namespace Alunos.UnitTests;

public abstract class TestBase
{
    protected readonly Mock<IMediatorHandler> MockMediatorHandler;
    protected readonly Mock<IAlunoQueryService> MockAlunoQueryService;
    protected readonly TestableDomainNotificacaoHandler Notifications;
    protected readonly Mock<INotificador> MockNotificador;

    protected TestBase()
    {
        MockMediatorHandler = new Mock<IMediatorHandler>();
        MockAlunoQueryService = new Mock<IAlunoQueryService>();
        Notifications = new TestableDomainNotificacaoHandler();
        MockNotificador = new Mock<INotificador>();

        // Configuração padrão para evitar listas nulas
        MockNotificador
            .Setup(x => x.ObterErros())
            .Returns(new List<string>());
        MockNotificador
            .Setup(x => x.TemErros())
            .Returns(false);
    }

    protected MainController CreateMainController()
    {
        return new TestMainController(
            MockMediatorHandler.Object,
            Notifications,
            MockNotificador.Object);
    }

    protected void SetupMockMediatorHandler(CommandResult result)
    {
        MockMediatorHandler
            .Setup(x => x.ExecutarComando(It.IsAny<CommandRaiz>()))
            .Returns(Task.FromResult(result));
    }

    protected void SetupMockMediatorHandlerWithException(Exception exception)
    {
        MockMediatorHandler
            .Setup(x => x.ExecutarComando(It.IsAny<CommandRaiz>()))
            .ThrowsAsync(exception);
    }

    protected void SetupMockAlunoQueryService(AlunoDto result)
    {
        MockAlunoQueryService
            .Setup(x => x.ObterAlunoPorIdAsync(It.IsAny<Guid>()))
            .Returns(Task.FromResult(result));
    }

    protected void SetupMockAlunoQueryServiceWithException(Exception exception)
    {
        MockAlunoQueryService
            .Setup(x => x.ObterAlunoPorIdAsync(It.IsAny<Guid>()))
            .ThrowsAsync(exception);
    }

    protected void SetupMockNotificador()
    {
        MockNotificador
            .Setup(x => x.AdicionarErro(It.IsAny<string>()));

        MockNotificador
            .Setup(x => x.TemErros())
            .Returns(true);

        MockNotificador
            .Setup(x => x.ObterErros())
            .Returns(new List<string> { "Erro de teste" });
    }

    protected void SetupMockNotifications()
    {
        Notifications.Limpar();
    }

    protected void SetupMockMediatorHandlerForNotifications()
    {
        MockMediatorHandler
            .Setup(x => x.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()))
            .Callback<DomainNotificacaoRaiz>(n => Notifications.Handle(n, CancellationToken.None).Wait());
    }

    private class TestMainController : MainController
    {
        public TestMainController(
            IMediatorHandler mediator,
            INotificationHandler<DomainNotificacaoRaiz> notifications,
            INotificador notificador)
            : base(mediator, notifications, notificador)
        {
        }
    }

    // Classe de teste que herda de DomainNotificacaoHandler para evitar problemas de cast
    public class TestableDomainNotificacaoHandler : DomainNotificacaoHandler
    {
    }
}
