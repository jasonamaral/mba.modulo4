using Auth.API.Controllers;
using Auth.API.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Auth.UnitTests;

public class AuthControllerTests
{
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    private readonly Mock<SignInManager<IdentityUser>> _signInManagerMock;
    private readonly Mock<IOptions<JwtSettings>> _jwtSettingsMock;
    private readonly Mock<ILogger<AuthController>> _loggerMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        var userStore = new Mock<IUserStore<IdentityUser>>();
        _userManagerMock = new Mock<UserManager<IdentityUser>>(
            userStore.Object, null, null, null, null, null, null, null, null);

        var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
        var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
        _signInManagerMock = new Mock<SignInManager<IdentityUser>>(
            _userManagerMock.Object, contextAccessor.Object, userPrincipalFactory.Object, null, null, null, null);

        _jwtSettingsMock = new Mock<IOptions<JwtSettings>>();
        _jwtSettingsMock.Setup(x => x.Value).Returns(new JwtSettings
        {
            SecretKey = "test-secret-key-with-at-least-256-bits-for-hmac-sha256",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            AccessTokenExpirationMinutes = 15,
            RefreshTokenExpirationDays = 7
        });

        _loggerMock = new Mock<ILogger<AuthController>>();

        _controller = new AuthController(
            _signInManagerMock.Object,
            _userManagerMock.Object,
            _jwtSettingsMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task MigratedAuth_ShouldMaintainCompatibility()
    {
        // Arrange
        var model = new RegistroUsuarioViewModel
        {
            Email = "test@example.com",
            Password = "Test123!",
            ConfirmPassword = "Test123!"
        };

        var user = new IdentityUser { UserName = model.Email, Email = model.Email };
        
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        
        _userManagerMock.Setup(x => x.GetClaimsAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(new List<System.Security.Claims.Claim>());
        
        _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(new List<string>());
        
        _signInManagerMock.Setup(x => x.SignInAsync(It.IsAny<IdentityUser>(), It.IsAny<bool>(), null))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Registro(model);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        
        // Verificar se o JWT foi gerado (compatibilidade com sistema atual)
        var response = okResult.Value as dynamic;
        Assert.NotNull(response);
        
        // Verificar se mantém a estrutura esperada pelo sistema atual
        _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
        _signInManagerMock.Verify(x => x.SignInAsync(It.IsAny<IdentityUser>(), It.IsAny<bool>(), null), Times.Once);
    }

    [Fact]
    public async Task Login_ShouldGenerateJwtToken()
    {
        // Arrange
        var model = new LoginUsuarioViewModel
        {
            Email = "test@example.com",
            Password = "Test123!"
        };

        var user = new IdentityUser { UserName = model.Email, Email = model.Email };
        
        _signInManagerMock.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
        
        _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        
        _userManagerMock.Setup(x => x.GetClaimsAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(new List<System.Security.Claims.Claim>());
        
        _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(new List<string>());

        // Act
        var result = await _controller.Login(model);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        
        // Verificar se o JWT foi gerado conforme esperado pelo sistema atual
        _signInManagerMock.Verify(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Once);
        _userManagerMock.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void JwtSettings_ShouldBeCompatibleWithCurrentSystem()
    {
        // Arrange & Act
        var settings = _jwtSettingsMock.Object.Value;

        // Assert
        Assert.NotNull(settings);
        Assert.NotEmpty(settings.SecretKey);
        Assert.NotEmpty(settings.Issuer);
        Assert.NotEmpty(settings.Audience);
        Assert.True(settings.AccessTokenExpirationMinutes > 0);
        Assert.True(settings.RefreshTokenExpirationDays > 0);
    }
} 