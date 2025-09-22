using Core.Communication;

namespace Core.Tests.Communication;

public class ApiSuccessTests : TestBase
{
    [Fact]
    public void ApiSuccess_DeveCriarComPropriedadesPadrao()
    {
        // Arrange & Act
        var apiSuccess = new ApiSuccess();

        // Assert
        apiSuccess.Should().NotBeNull();
        apiSuccess.Message.Should().Be(string.Empty);
        apiSuccess.Data.Should().BeNull();
    }

    [Fact]
    public void ApiSuccess_DevePermitirDefinirMensagem()
    {
        // Arrange
        var mensagem = "Operação realizada com sucesso";
        var apiSuccess = new ApiSuccess();

        // Act
        apiSuccess.Message = mensagem;

        // Assert
        apiSuccess.Message.Should().Be(mensagem);
    }

    [Fact]
    public void ApiSuccess_DevePermitirDefinirData()
    {
        // Arrange
        var data = new { Id = 1, Nome = "Teste" };
        var apiSuccess = new ApiSuccess();

        // Act
        apiSuccess.Data = data;

        // Assert
        apiSuccess.Data.Should().Be(data);
    }

    [Fact]
    public void ApiSuccess_DevePermitirDataNull()
    {
        // Arrange
        var apiSuccess = new ApiSuccess();

        // Act
        apiSuccess.Data = null;

        // Assert
        apiSuccess.Data.Should().BeNull();
    }

    [Fact]
    public void ApiSuccess_DevePermitirDataString()
    {
        // Arrange
        var data = "Dados de teste";
        var apiSuccess = new ApiSuccess();

        // Act
        apiSuccess.Data = data;

        // Assert
        apiSuccess.Data.Should().Be(data);
    }

    [Fact]
    public void ApiSuccess_DevePermitirDataInt()
    {
        // Arrange
        var data = 42;
        var apiSuccess = new ApiSuccess();

        // Act
        apiSuccess.Data = data;

        // Assert
        apiSuccess.Data.Should().Be(data);
    }
}
