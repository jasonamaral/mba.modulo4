using BFF.API.Controllers;
using BFF.API.Models.Response;
using Core.Mediator;
using Core.Notification;
using Core.Messages;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MediatR;
using Xunit;

namespace BFF.IntegrationTests.Controllers;

public class HealthControllerIntegrationTests
{
    [Fact]
    public void HealthController_DeveTerTipoCorreto()
    {
        // Arrange & Act
        var controllerType = typeof(HealthController);

        // Assert
        controllerType.Should().NotBeNull();
        controllerType.Should().BeAssignableTo<ControllerBase>();
    }

    [Fact]
    public void HealthController_DeveHerdarDeBffController()
    {
        // Arrange & Act
        var controllerType = typeof(HealthController);

        // Assert
        controllerType.BaseType.Should().Be(typeof(BffController));
    }

    [Fact]
    public void HealthController_DeveTerConstrutorCorreto()
    {
        // Arrange & Act
        var controllerType = typeof(HealthController);
        var constructors = controllerType.GetConstructors();

        // Assert
        constructors.Should().HaveCount(1);
        var constructor = constructors.First();
        var parameters = constructor.GetParameters();
        parameters.Should().HaveCount(4);
        parameters[0].ParameterType.Should().Be(typeof(ILogger<HealthController>));
        parameters[1].ParameterType.Should().Be(typeof(IMediatorHandler));
        parameters[2].ParameterType.Should().Be(typeof(INotificationHandler<DomainNotificacaoRaiz>));
        parameters[3].ParameterType.Should().Be(typeof(INotificador));
    }

    [Fact]
    public void HealthController_DeveTerMetodosPublicos()
    {
        // Arrange & Act
        var controllerType = typeof(HealthController);
        var publicMethods = controllerType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        // Assert
        publicMethods.Should().NotBeEmpty();
        publicMethods.Should().Contain(m => m.Name == "Get");
        publicMethods.Should().Contain(m => m.Name == "GetStatus");
    }

    [Fact]
    public void HealthController_DeveTerAtributosCorretos()
    {
        // Arrange & Act
        var controllerType = typeof(HealthController);

        // Assert
        controllerType.GetCustomAttributes(typeof(ApiControllerAttribute), false).Should().NotBeEmpty();
        controllerType.GetCustomAttributes(typeof(RouteAttribute), false).Should().NotBeEmpty();
    }
}
