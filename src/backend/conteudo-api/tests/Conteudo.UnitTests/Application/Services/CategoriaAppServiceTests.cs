using Conteudo.Application.DTOs;
using Conteudo.Application.Services;
using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;

namespace Conteudo.UnitTests.Application.Services;

public class CategoriaAppServiceTests : TestBase
{
    private readonly Mock<ICategoriaRepository> _categoriaRepositoryMock;
    private readonly CategoriaAppService _categoriaAppService;

    public CategoriaAppServiceTests()
    {
        _categoriaRepositoryMock = new Mock<ICategoriaRepository>();
        _categoriaAppService = new CategoriaAppService(_categoriaRepositoryMock.Object);
    }

    private Categoria CriarCategoria(string nome, string descricao, string cor)
    {
        return new Categoria(nome, descricao, cor, "", 0);
    }

    [Fact]
    public async Task ObterTodasCategoriasAsync_DeveRetornarListaDeCategorias()
    {
        // Arrange
        var categorias = new List<Categoria>
        {
            CriarCategoria("Programação", "Cursos de programação", "#FF0000"),
            CriarCategoria("Design", "Cursos de design", "#00FF00")
        };

        _categoriaRepositoryMock.Setup(x => x.ObterTodosAsync())
            .ReturnsAsync(categorias);

        // Act
        var result = await _categoriaAppService.ObterTodasCategoriasAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeAssignableTo<IEnumerable<CategoriaDto>>();

        var categoriasList = result.ToList();
        categoriasList[0].Nome.Should().Be("Programação");
        categoriasList[1].Nome.Should().Be("Design");

        _categoriaRepositoryMock.Verify(x => x.ObterTodosAsync(), Times.Once);
    }

    [Fact]
    public async Task ObterTodasCategoriasAsync_QuandoNaoHouverCategorias_DeveRetornarListaVazia()
    {
        // Arrange
        _categoriaRepositoryMock.Setup(x => x.ObterTodosAsync())
            .ReturnsAsync(new List<Categoria>());

        // Act
        var result = await _categoriaAppService.ObterTodasCategoriasAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _categoriaRepositoryMock.Verify(x => x.ObterTodosAsync(), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_ComIdValido_DeveRetornarCategoria()
    {
        // Arrange
        var id = Guid.NewGuid();
        var categoria = CriarCategoria("Programação", "Cursos de programação", "#FF0000");

        _categoriaRepositoryMock.Setup(x => x.ObterPorIdAsync(id, true))
            .ReturnsAsync(categoria);

        // Act
        var result = await _categoriaAppService.ObterPorIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CategoriaDto>();
        result!.Nome.Should().Be("Programação");
        result.Descricao.Should().Be("Cursos de programação");
        result.Cor.Should().Be("#FF0000");

        _categoriaRepositoryMock.Verify(x => x.ObterPorIdAsync(id, true), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_ComIdInexistente_DeveRetornarNull()
    {
        // Arrange
        var id = Guid.NewGuid();

        _categoriaRepositoryMock.Setup(x => x.ObterPorIdAsync(id, true))
            .ReturnsAsync((Categoria?)null);

        // Act
        var result = await _categoriaAppService.ObterPorIdAsync(id);

        // Assert
        result.Should().BeNull();

        _categoriaRepositoryMock.Verify(x => x.ObterPorIdAsync(id, true), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_ComIdVazio_DeveRetornarNull()
    {
        // Arrange
        var id = Guid.Empty;

        _categoriaRepositoryMock.Setup(x => x.ObterPorIdAsync(id, true))
            .ReturnsAsync((Categoria?)null);

        // Act
        var result = await _categoriaAppService.ObterPorIdAsync(id);

        // Assert
        result.Should().BeNull();

        _categoriaRepositoryMock.Verify(x => x.ObterPorIdAsync(id, true), Times.Once);
    }
}
