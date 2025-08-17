using Alunos.Application.Interfaces;
using Alunos.Application.DTOs.Response;
using Core.Communication;
using Core.Mediator;
using Core.Notification;
using Core.Services.Controllers;
using Core.Messages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Alunos.UnitTests;

public abstract class TestBase
{
    protected readonly Mock<IMediatorHandler> MockMediatorHandler;
    protected readonly Mock<IAlunoQueryService> MockAlunoQueryService;
    protected readonly Mock<IDomainNotificacaoHandler> MockNotifications;
    protected readonly Mock<INotificador> MockNotificador;

    protected TestBase()
    {
        MockMediatorHandler = new Mock<IMediatorHandler>();
        MockAlunoQueryService = new Mock<IAlunoQueryService>();
        MockNotifications = new Mock<IDomainNotificacaoHandler>();
        MockNotificador = new Mock<INotificador>();
    }

    protected MainController CreateMainController()
    {
        return new TestMainController(
            MockMediatorHandler.Object,
            MockNotifications.Object,
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
        MockNotifications
            .Setup(x => x.TemNotificacao())
            .Returns(false);
    }

    protected void SetupMockMediatorHandlerForNotifications()
    {
        MockMediatorHandler
            .Setup(x => x.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()));
    }

    private class TestMainController : MainController
    {
        public TestMainController(
            IMediatorHandler mediator,
            IDomainNotificacaoHandler notifications,
            INotificador notificador)
            : base(mediator, notifications, notificador)
        {
        }
    }
}
