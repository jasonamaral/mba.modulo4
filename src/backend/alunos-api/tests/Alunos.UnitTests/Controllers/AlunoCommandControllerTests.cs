using Alunos.API.Controllers;
using Alunos.Application.Commands.ConcluirCurso;
using Alunos.Application.Commands.MatricularAluno;
using Alunos.Application.Commands.RegistrarHistoricoAprendizado;
using Alunos.Application.Commands.SolicitarCertificado;
using Alunos.Application.DTOs.Request;
using Alunos.Application.DTOs.Response;
using Core.Communication;
using Core.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Alunos.UnitTests.Controllers;

public class AlunoCommandControllerTests : TestBase
{
    private readonly AlunoController _controller;

    public AlunoCommandControllerTests()
    {
        _controller = new AlunoController(
            MockMediatorHandler.Object,
            MockAlunoQueryService.Object,
            Notifications,
            MockNotificador.Object);
    }

    [Fact]
    public async Task MatricularAluno_ComDadosValidos_DeveRetornarSucesso()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();
        var dto = new MatriculaCursoRequest
        {
            AlunoId = alunoId,
            CursoId = cursoId,
            CursoDisponivel = true,
            Nome = "Curso de Teste",
            Valor = 100.00m,
            Observacao = "Observação teste"
        };

        var commandResult = new CommandResult(new FluentValidation.Results.ValidationResult(), Guid.NewGuid());
        SetupMockMediatorHandler(commandResult);

        // Act
        var result = await _controller.MatricularAluno(alunoId, dto);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult!.StatusCode.Should().Be(200);

        MockMediatorHandler.Verify(x => x.ExecutarComando(It.IsAny<MatricularAlunoCommand>()), Times.Once);
    }

    [Fact]
    public async Task MatricularAluno_ComIdAlunoDiferente_DeveRetornarBadRequest()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        var dto = new MatriculaCursoRequest
        {
            AlunoId = Guid.NewGuid(), // ID diferente
            CursoId = Guid.NewGuid(),
            CursoDisponivel = true,
            Nome = "Curso de Teste",
            Valor = 100.00m,
            Observacao = "Observação teste"
        };

        SetupMockNotificador();

        // Act
        var result = await _controller.MatricularAluno(alunoId, dto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);

        MockNotificador.Verify(x => x.AdicionarErro(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task MatricularAluno_ComModelStateInvalido_DeveRetornarBadRequest()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        var dto = new MatriculaCursoRequest
        {
            AlunoId = alunoId,
            CursoId = Guid.NewGuid(),
            CursoDisponivel = true,
            Nome = "", // Nome vazio para invalidar
            Valor = 100.00m,
            Observacao = "Observação teste"
        };

        _controller.ModelState.AddModelError("Nome", "Nome é obrigatório");

        // Configurar mocks para notificações de domínio
        SetupMockMediatorHandlerForNotifications();

        // Popular notificações simulando mensagens de ModelState
        await Notifications.Handle(new DomainNotificacaoRaiz("ModelState", "Nome é obrigatório"), CancellationToken.None);

        // Act
        var result = await _controller.MatricularAluno(alunoId, dto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);

        // Deve chamar PublicarNotificacaoDominio para cada erro do ModelState
        MockMediatorHandler.Verify(x => x.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
    }

    [Fact]
    public async Task RegistrarHistoricoAprendizado_ComDadosValidos_DeveRetornarSucesso()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        var matriculaId = Guid.NewGuid();
        var dto = new RegistroHistoricoAprendizadoRequest
        {
            AlunoId = alunoId,
            MatriculaCursoId = matriculaId,
            AulaId = Guid.NewGuid(),
            NomeAula = "Aula de Teste",
            DuracaoMinutos = 60,
            DataTermino = DateTime.Now
        };

        var matriculaCurso = new MatriculaCursoDto { Id = matriculaId };
        MockAlunoQueryService.Setup(x => x.ObterInformacaoMatriculaCursoAsync(matriculaId))
            .ReturnsAsync(matriculaCurso);

        var commandResult = new CommandResult(new FluentValidation.Results.ValidationResult(), true);
        SetupMockMediatorHandler(commandResult);

        // Act
        var result = await _controller.RegistrarHistoricoAprendizado(alunoId, dto);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult!.StatusCode.Should().Be(200);

        MockMediatorHandler.Verify(x => x.ExecutarComando(It.IsAny<RegistrarHistoricoAprendizadoCommand>()), Times.Once);
    }

    [Fact]
    public async Task RegistrarHistoricoAprendizado_ComMatriculaNaoEncontrada_DeveRetornarBadRequest()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        var matriculaId = Guid.NewGuid();
        var dto = new RegistroHistoricoAprendizadoRequest
        {
            AlunoId = alunoId,
            MatriculaCursoId = matriculaId,
            AulaId = Guid.NewGuid(),
            NomeAula = "Aula de Teste",
            DuracaoMinutos = 60,
            DataTermino = DateTime.Now
        };

        MockAlunoQueryService.Setup(x => x.ObterInformacaoMatriculaCursoAsync(matriculaId))
            .ReturnsAsync((MatriculaCursoDto?)null);

        SetupMockNotificador();

        // Act
        var result = await _controller.RegistrarHistoricoAprendizado(alunoId, dto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);

        MockNotificador.Verify(x => x.AdicionarErro(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ConcluirCurso_ComDadosValidos_DeveRetornarSucesso()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        var matriculaId = Guid.NewGuid();
        var dto = new ConcluirCursoRequest
        {
            AlunoId = alunoId,
            MatriculaCursoId = matriculaId,
            CursoDto = new Core.SharedDtos.Conteudo.CursoDto
            {
                Id = Guid.NewGuid(),
                Nome = "Curso Teste",
                Aulas = new List<Core.SharedDtos.Conteudo.AulaDto>()
            }
        };

        var matriculaCurso = new MatriculaCursoDto { Id = matriculaId };
        MockAlunoQueryService.Setup(x => x.ObterInformacaoMatriculaCursoAsync(matriculaId))
            .ReturnsAsync(matriculaCurso);

        var commandResult = new CommandResult(new FluentValidation.Results.ValidationResult(), true);
        SetupMockMediatorHandler(commandResult);

        // Act
        var result = await _controller.ConcluirCurso(alunoId, dto);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult!.StatusCode.Should().Be(200);

        MockMediatorHandler.Verify(x => x.ExecutarComando(It.IsAny<ConcluirCursoCommand>()), Times.Once);
    }

    [Fact]
    public async Task ConcluirCurso_ComCursoDtoNulo_DeveRetornarBadRequest()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        var matriculaId = Guid.NewGuid();
        var dto = new ConcluirCursoRequest
        {
            AlunoId = alunoId,
            MatriculaCursoId = matriculaId,
            CursoDto = null // CursoDto nulo
        };

        SetupMockNotificador();

        // Act
        var result = await _controller.ConcluirCurso(alunoId, dto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);

        MockNotificador.Verify(x => x.AdicionarErro(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task SolicitarCertificado_ComDadosValidos_DeveRetornarSucesso()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        var matriculaId = Guid.NewGuid();
        var dto = new SolicitaCertificadoRequest
        {
            AlunoId = alunoId,
            MatriculaCursoId = matriculaId
        };

        var commandResult = new CommandResult(new FluentValidation.Results.ValidationResult(), Guid.NewGuid());
        SetupMockMediatorHandler(commandResult);

        // Act
        var result = await _controller.SolicitarCertificado(alunoId, dto);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult!.StatusCode.Should().Be(200);

        MockMediatorHandler.Verify(x => x.ExecutarComando(It.IsAny<SolicitarCertificadoCommand>()), Times.Once);
    }

    [Fact]
    public async Task SolicitarCertificado_ComExcecao_DeveRetornarInternalServerError()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        var matriculaId = Guid.NewGuid();
        var dto = new SolicitaCertificadoRequest
        {
            AlunoId = alunoId,
            MatriculaCursoId = matriculaId
        };

        var exception = new Exception("Erro interno");
        SetupMockMediatorHandlerWithException(exception);

        // Act & Assert
        var action = () => _controller.SolicitarCertificado(alunoId, dto);
        await action.Should().ThrowAsync<Exception>()
            .WithMessage("Erro interno");
    }
}
