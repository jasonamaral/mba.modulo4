using Alunos.API.Controllers;
using Alunos.Application.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Alunos.UnitTests.Controllers;

public class AlunoQueryControllerTests : TestBase
{
    private readonly AlunoController _controller;

    public AlunoQueryControllerTests()
    {
        _controller = new AlunoController(
            MockMediatorHandler.Object,
            MockAlunoQueryService.Object,
            Notifications,
            MockNotificador.Object);
    }

    private void SetUserContext(Guid alunoId)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, alunoId.ToString())
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };
    }

    [Fact]
    public async Task ObterAlunoPorId_ComAlunoExistente_DeveRetornarSucesso()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        var aluno = new AlunoDto
        {
            Id = alunoId,
            Nome = "João Silva",
            Email = "joao@teste.com",
            Cpf = "12345678901",
            DataNascimento = DateTime.Now.AddYears(-25),
            Genero = "Masculino",
            Cidade = "São Paulo",
            Estado = "SP",
            Cep = "01234-567",
            Ativo = true
        };

        MockAlunoQueryService.Setup(x => x.ObterAlunoPorIdAsync(alunoId))
            .ReturnsAsync(aluno);

        // Não configurar SetupMockNotificador para este teste de sucesso

        // Act
        var result = await _controller.ObterAlunoPorId(alunoId);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult!.StatusCode.Should().Be(200);

        MockAlunoQueryService.Verify(x => x.ObterAlunoPorIdAsync(alunoId), Times.Once);
    }

    [Fact]
    public async Task ObterAlunoPorId_ComAlunoNaoExistente_DeveRetornarBadRequest()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        MockAlunoQueryService.Setup(x => x.ObterAlunoPorIdAsync(alunoId))
            .ReturnsAsync((AlunoDto?)null);

        ConfigurarMockNotificador();

        // Act
        var result = await _controller.ObterAlunoPorId(alunoId);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);

        MockNotificador.Verify(x => x.AdicionarErro("Aluno não encontrado."), Times.Once);
    }

    [Fact]
    public async Task ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync_ComEvolucaoExistente_DeveRetornarSucesso()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        var evolucao = new EvolucaoAlunoDto
        {
            Id = alunoId,
            Nome = "João Silva",
            Email = "joao@teste.com",
            DataNascimento = DateTime.Now.AddYears(-25),
            MatriculasCursos = new List<EvolucaoMatriculaCursoDto>
            {
                new EvolucaoMatriculaCursoDto
                {
                    Id = Guid.NewGuid(),
                    CursoId = Guid.NewGuid(),
                    NomeCurso = "Curso 1",
                    EstadoMatricula = "Concluído"
                },
                new EvolucaoMatriculaCursoDto
                {
                    Id = Guid.NewGuid(),
                    CursoId = Guid.NewGuid(),
                    NomeCurso = "Curso 2",
                    EstadoMatricula = "Em Andamento"
                }
            }
        };

        MockAlunoQueryService.Setup(x => x.ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(alunoId))
            .ReturnsAsync(evolucao);

        // Não configurar SetupMockNotificador para este teste de sucesso

        // Act
        var result = await _controller.ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(alunoId);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult!.StatusCode.Should().Be(200);

        MockAlunoQueryService.Verify(x => x.ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(alunoId), Times.Once);
    }

    [Fact]
    public async Task ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync_ComEvolucaoNaoExistente_DeveRetornarBadRequest()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        MockAlunoQueryService.Setup(x => x.ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(alunoId))
            .ReturnsAsync((EvolucaoAlunoDto?)null);

        ConfigurarMockNotificador();

        // Act
        var result = await _controller.ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(alunoId);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);

        MockNotificador.Verify(x => x.AdicionarErro("Evolução da matricula do aluno não encontrado."), Times.Once);
    }

    [Fact]
    public async Task ObterMatriculasPorAlunoId_ComMatriculasExistentes_DeveRetornarSucesso()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        var matriculas = new List<MatriculaCursoDto>
        {
            new MatriculaCursoDto
            {
                Id = Guid.NewGuid(),
                AlunoId = alunoId,
                CursoId = Guid.NewGuid(),
                NomeCurso = "Curso de Teste 1",
                DataMatricula = DateTime.Now.AddDays(-30),
                EstadoMatricula = "Em Andamento"
            },
            new MatriculaCursoDto
            {
                Id = Guid.NewGuid(),
                AlunoId = alunoId,
                CursoId = Guid.NewGuid(),
                NomeCurso = "Curso de Teste 2",
                DataMatricula = DateTime.Now.AddDays(-15),
                EstadoMatricula = "Concluído"
            }
        };

        MockAlunoQueryService.Setup(x => x.ObterMatriculasPorAlunoIdAsync(alunoId))
            .ReturnsAsync(matriculas);

        // Não configurar SetupMockNotificador para este teste de sucesso

        // Act
        SetUserContext(alunoId);
        var result = await _controller.ObterMatriculasPorAlunoId();

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult!.StatusCode.Should().Be(200);

        MockAlunoQueryService.Verify(x => x.ObterMatriculasPorAlunoIdAsync(alunoId), Times.Once);
    }

    [Fact]
    public async Task ObterMatriculasPorAlunoId_ComMatriculasVazias_DeveRetornarBadRequest()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        MockAlunoQueryService.Setup(x => x.ObterMatriculasPorAlunoIdAsync(alunoId))
            .ReturnsAsync(new List<MatriculaCursoDto>());

        ConfigurarMockNotificador();

        // Act
        SetUserContext(alunoId);
        var result = await _controller.ObterMatriculasPorAlunoId();

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);

        MockNotificador.Verify(x => x.AdicionarErro("Matrícula do aluno não encontrada."), Times.Once);
    }

    [Fact]
    public async Task ObterMatriculasPorAlunoId_ComExcecao_DeveRetornarBadRequest()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        MockAlunoQueryService.Setup(x => x.ObterMatriculasPorAlunoIdAsync(alunoId))
            .ThrowsAsync(new Exception("Erro de teste"));

        ConfigurarMockNotificador();

        // Act
        SetUserContext(alunoId);
        var result = await _controller.ObterMatriculasPorAlunoId();

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);

        MockAlunoQueryService.Verify(x => x.ObterMatriculasPorAlunoIdAsync(alunoId), Times.Once);
    }

    [Fact]
    public async Task ObterCertificadoPorMatriculaId_ComCertificadoExistente_DeveRetornarSucesso()
    {
        // Arrange
        var matriculaId = Guid.NewGuid();
        var certificado = new CertificadoDto
        {
            Id = Guid.NewGuid(),
            MatriculaCursoId = matriculaId,
            NomeCurso = "Curso de Teste",
            DataSolicitacao = DateTime.Now,
            DataEmissao = DateTime.Now,
            CargaHoraria = 40,
            PathCertificado = "/certificados/cert-001.pdf",
            NomeInstrutor = "Professor Silva"
        };

        MockAlunoQueryService.Setup(x => x.ObterCertificadoPorMatriculaIdAsync(matriculaId))
            .ReturnsAsync(certificado);

        // Não configurar SetupMockNotificador para este teste de sucesso

        // Act
        var result = await _controller.ObterCertificadoPorMatriculaId(matriculaId);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult!.StatusCode.Should().Be(200);

        MockAlunoQueryService.Verify(x => x.ObterCertificadoPorMatriculaIdAsync(matriculaId), Times.Once);
    }

    [Fact]
    public async Task ObterCertificadoPorMatriculaId_ComCertificadoNaoExistente_DeveRetornarBadRequest()
    {
        // Arrange
        var matriculaId = Guid.NewGuid();
        MockAlunoQueryService.Setup(x => x.ObterCertificadoPorMatriculaIdAsync(matriculaId))
            .ReturnsAsync((CertificadoDto?)null);

        ConfigurarMockNotificador();

        // Act
        var result = await _controller.ObterCertificadoPorMatriculaId(matriculaId);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);

        MockNotificador.Verify(x => x.AdicionarErro("Certificado não encontrado."), Times.Once);
    }

    [Fact]
    public async Task ObterAulasPorMatriculaId_ComAulasExistentes_DeveRetornarSucesso()
    {
        // Arrange
        var matriculaId = Guid.NewGuid();
        var aulas = new List<AulaCursoDto>
        {
            new AulaCursoDto
            {
                AulaId = Guid.NewGuid(),
                CursoId = Guid.NewGuid(),
                NomeAula = "Aula 1 - Introdução",
                OrdemAula = 1,
                Ativo = true,
                DataTermino = DateTime.Now.AddDays(-1)
            },
            new AulaCursoDto
            {
                AulaId = Guid.NewGuid(),
                CursoId = Guid.NewGuid(),
                NomeAula = "Aula 2 - Conceitos Básicos",
                OrdemAula = 2,
                Ativo = true,
                DataTermino = null
            }
        };

        MockAlunoQueryService.Setup(x => x.ObterAulasPorMatriculaIdAsync(matriculaId))
            .ReturnsAsync(aulas);

        // Não configurar SetupMockNotificador para este teste de sucesso

        // Act
        var result = await _controller.ObterAulasPorMatriculaId(matriculaId);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult!.StatusCode.Should().Be(200);

        MockAlunoQueryService.Verify(x => x.ObterAulasPorMatriculaIdAsync(matriculaId), Times.Once);
    }

    [Fact]
    public async Task ObterAulasPorMatriculaId_ComAulasNaoExistentes_DeveRetornarBadRequest()
    {
        // Arrange
        var matriculaId = Guid.NewGuid();
        MockAlunoQueryService.Setup(x => x.ObterAulasPorMatriculaIdAsync(matriculaId))
            .ReturnsAsync(new List<AulaCursoDto>());

        ConfigurarMockNotificador();

        // Act
        var result = await _controller.ObterAulasPorMatriculaId(matriculaId);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);

        MockAlunoQueryService.Verify(x => x.ObterAulasPorMatriculaIdAsync(matriculaId), Times.Once);
    }
}
