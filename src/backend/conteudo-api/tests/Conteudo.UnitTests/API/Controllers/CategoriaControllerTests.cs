using Conteudo.API.Controllers;
using Conteudo.Application.Commands.AtualizarCategoria;
using Conteudo.Application.Commands.CadastrarCategoria;
using Conteudo.Application.DTOs;
using Conteudo.Application.Interfaces.Services;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace Conteudo.UnitTests.API.Controllers;

public class CategoriaControllerTests : TestBase
{
    private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
    private readonly Mock<ICategoriaAppService> _categoriaAppServiceMock;
    private readonly MockDomainNotificacaoHandler _notificationsMock;
    private readonly Mock<INotificador> _notificadorMock;
    private readonly CategoriaController _controller;

    public CategoriaControllerTests()
    {
        _mediatorHandlerMock = new Mock<IMediatorHandler>();
        _categoriaAppServiceMock = new Mock<ICategoriaAppService>();
        _notificationsMock = new MockDomainNotificacaoHandler();
        _notificadorMock = new Mock<INotificador>();

        // Configurar mocks para retornar listas vazias em vez de null
        _notificadorMock.Setup(x => x.ObterErros()).Returns(new List<string>());
        _notificadorMock.Setup(x => x.TemErros()).Returns(false);

        _controller = new CategoriaController(
            _mediatorHandlerMock.Object,
            _categoriaAppServiceMock.Object,
            _notificationsMock,
            _notificadorMock.Object);

        // Configurar contexto HTTP mock
        var httpContext = new DefaultHttpContext();
        var controllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
        _controller.ControllerContext = controllerContext;
    }

    [Fact]
    public async Task ObterPorId_ComIdValido_DeveRetornarCategoria()
    {
        // Arrange
        var id = Guid.NewGuid();
        var categoriaDto = new CategoriaDto
        {
            Id = id,
            Nome = "Programação",
            Descricao = "Cursos de programação",
            Cor = "#FF0000"
        };

        _categoriaAppServiceMock.Setup(x => x.ObterPorIdAsync(id))
            .ReturnsAsync(categoriaDto);

        // Act
        var result = await _controller.ObterPorId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ObjectResult>();

        var objectResult = result as ObjectResult;
        objectResult!.StatusCode.Should().Be(200);

        _categoriaAppServiceMock.Verify(x => x.ObterPorIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task ObterPorId_ComIdInexistente_DeveRetornarNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        _categoriaAppServiceMock.Setup(x => x.ObterPorIdAsync(id))
            .ReturnsAsync((CategoriaDto?)null);

        _notificadorMock.Setup(x => x.AdicionarErro(It.IsAny<string>()));
        _notificadorMock.Setup(x => x.TemErros()).Returns(true);
        _notificadorMock.Setup(x => x.ObterErros()).Returns(new List<string> { "Categoria não encontrada." });

        // Act
        var result = await _controller.ObterPorId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<BadRequestObjectResult>();

        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);

        _categoriaAppServiceMock.Verify(x => x.ObterPorIdAsync(id), Times.Once);
        _notificadorMock.Verify(x => x.AdicionarErro("Categoria não encontrada."), Times.Once);
    }

    [Fact]
    public async Task ObterTodos_DeveRetornarListaDeCategorias()
    {
        // Arrange
        var categorias = new List<CategoriaDto>
        {
            new CategoriaDto { Id = Guid.NewGuid(), Nome = "Programação", Descricao = "Cursos de programação", Cor = "#FF0000" },
            new CategoriaDto { Id = Guid.NewGuid(), Nome = "Design", Descricao = "Cursos de design", Cor = "#00FF00" }
        };

        _categoriaAppServiceMock.Setup(x => x.ObterTodasCategoriasAsync())
            .ReturnsAsync(categorias);

        // Act
        var result = await _controller.ObterTodos();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ObjectResult>();

        var objectResult = result as ObjectResult;
        objectResult!.StatusCode.Should().Be(200);

        _categoriaAppServiceMock.Verify(x => x.ObterTodasCategoriasAsync(), Times.Once);
    }

    [Fact]
    public async Task CadastrarCategoria_ComDadosValidos_DeveRetornarCreated()
    {
        // Arrange
        var dto = new CadastroCategoriaDto
        {
            Nome = "Nova Categoria",
            Descricao = "Descrição da nova categoria",
            Cor = "#FF0000"
        };

        var validationResult = new ValidationResult();
        var commandResult = new CommandResult(validationResult, Guid.NewGuid());
        _mediatorHandlerMock.Setup(x => x.ExecutarComando(It.IsAny<CadastrarCategoriaCommand>()))
            .ReturnsAsync(commandResult);

        // Act
        var result = await _controller.CadastrarCategoria(dto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ObjectResult>();

        var objectResult = result as ObjectResult;
        objectResult!.StatusCode.Should().Be(201);

        _mediatorHandlerMock.Verify(x => x.ExecutarComando(It.IsAny<CadastrarCategoriaCommand>()), Times.Once);
    }

    [Fact]
    public async Task AtualizarCategoria_ComIdValido_DeveRetornarOk()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new AtualizarCategoriaDto
        {
            Id = id,
            Nome = "Categoria Atualizada",
            Descricao = "Descrição atualizada",
            Cor = "#00FF00"
        };

        var validationResult = new ValidationResult();
        var commandResult = new CommandResult(validationResult, true);
        _mediatorHandlerMock.Setup(x => x.ExecutarComando(It.IsAny<AtualizarCategoriaCommand>()))
            .ReturnsAsync(commandResult);

        // Act
        var result = await _controller.AtualizarCategoria(id, dto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ObjectResult>();

        var objectResult = result as ObjectResult;
        objectResult!.StatusCode.Should().Be(200);

        _mediatorHandlerMock.Verify(x => x.ExecutarComando(It.IsAny<AtualizarCategoriaCommand>()), Times.Once);
    }

    [Fact]
    public async Task AtualizarCategoria_ComIdDiferente_DeveRetornarBadRequest()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new AtualizarCategoriaDto
        {
            Id = Guid.NewGuid(), // ID diferente
            Nome = "Categoria Atualizada",
            Descricao = "Descrição atualizada",
            Cor = "#00FF00"
        };

        // Act
        var result = await _controller.AtualizarCategoria(id, dto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ObjectResult>();

        var objectResult = result as ObjectResult;
        objectResult!.StatusCode.Should().Be(400);
        
        // Verificar que o valor é um ResponseResult<string> com a mensagem correta
        objectResult.Value.Should().BeOfType<ResponseResult<string>>();
        var responseResult = objectResult.Value as ResponseResult<string>;
        responseResult!.Data.Should().Be("ID da categoria não confere.");
    }
}

// Implementação mock do DomainNotificacaoHandler para testes
public class MockDomainNotificacaoHandler : DomainNotificacaoHandler
{
    private readonly List<DomainNotificacaoRaiz> _notificacoes = new();

    public new Task Handle(DomainNotificacaoRaiz notificacao, CancellationToken cancellationToken)
    {
        _notificacoes.Add(notificacao);
        return Task.CompletedTask;
    }

    public new List<string> ObterMensagens() => _notificacoes.Select(n => n.Valor).ToList();

    public new List<DomainNotificacaoRaiz> ObterNotificacoes() => _notificacoes;

    public new bool TemNotificacao() => _notificacoes.Count > 0;

    public new void Limpar() => _notificacoes.Clear();
}
