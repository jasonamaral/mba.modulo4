using Core.Messages;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Xunit;

namespace Core.Tests.Messages;

public class ResponseMessageTests : TestBase
{
    [Fact]
    public void ResponseMessage_DeveCriarComValidationResult()
    {
        // Arrange
        var validationResult = new ValidationResult();

        // Act
        var responseMessage = new ResponseMessage(validationResult);

        // Assert
        responseMessage.Should().NotBeNull();
        responseMessage.ValidationResult.Should().Be(validationResult);
    }

    [Fact]
    public void ResponseMessage_DevePermitirValidationResultComErros()
    {
        // Arrange
        var validationResult = new ValidationResult();
        validationResult.Errors.Add(new ValidationFailure("", "Erro de teste"));

        // Act
        var responseMessage = new ResponseMessage(validationResult);

        // Assert
        responseMessage.Should().NotBeNull();
        responseMessage.ValidationResult.Should().Be(validationResult);
        responseMessage.ValidationResult.Errors.Should().HaveCount(1);
    }

    [Fact]
    public void ResponseMessage_DevePermitirValidationResultVazio()
    {
        // Arrange
        var validationResult = new ValidationResult();

        // Act
        var responseMessage = new ResponseMessage(validationResult);

        // Assert
        responseMessage.Should().NotBeNull();
        responseMessage.ValidationResult.Should().Be(validationResult);
        responseMessage.ValidationResult.Errors.Should().BeEmpty();
    }

    [Fact]
    public void ResponseMessage_DevePermitirAlteracaoValidationResult()
    {
        // Arrange
        var validationResult1 = new ValidationResult();
        var validationResult2 = new ValidationResult();
        validationResult2.Errors.Add(new ValidationFailure("", "Novo erro"));

        var responseMessage = new ResponseMessage(validationResult1);

        // Act
        responseMessage.ValidationResult = validationResult2;

        // Assert
        responseMessage.ValidationResult.Should().Be(validationResult2);
        responseMessage.ValidationResult.Errors.Should().HaveCount(1);
    }

    [Fact]
    public void ResponseMessage_DevePermitirValidationResultNull()
    {
        // Arrange
        var validationResult = new ValidationResult();
        var responseMessage = new ResponseMessage(validationResult);

        // Act
        responseMessage.ValidationResult = null!;

        // Assert
        responseMessage.ValidationResult.Should().BeNull();
    }
}
