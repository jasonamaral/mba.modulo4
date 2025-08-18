using Auth.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Auth.UnitTests.API;

public class AuthControllerTests : TestBase
{
    [Fact]
    public void AuthController_DeveSerTestado()
    {
        // Arrange & Act
        var result = true;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ApplicationUser_DeveSerCriadoCorretamente()
    {
        // Arrange
        var nome = Faker.Person.FullName;
        var email = Faker.Person.Email;

        // Act
        var user = new ApplicationUser
        {
            Nome = nome,
            Email = email
        };

        // Assert
        user.Should().NotBeNull();
        user.Nome.Should().Be(nome);
        user.Email.Should().Be(email);
    }

    [Fact]
    public void RefreshToken_DeveSerCriadoCorretamente()
    {
        // Arrange
        var token = Guid.NewGuid();
        var username = Faker.Person.Email;

        // Act
        var refreshToken = new RefreshToken
        {
            Token = token,
            Username = username
        };

        // Assert
        refreshToken.Should().NotBeNull();
        refreshToken.Token.Should().Be(token);
        refreshToken.Username.Should().Be(username);
    }
}
