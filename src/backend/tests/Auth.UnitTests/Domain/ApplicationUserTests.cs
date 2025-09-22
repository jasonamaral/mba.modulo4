using Auth.Domain.Entities;

namespace Auth.UnitTests.Domain;

public class ApplicationUserTests : TestBase
{
    [Fact]
    public void ApplicationUser_DeveCriarUsuarioComDadosValidos()
    {
        // Arrange
        var nome = Faker.Person.FullName;
        var email = Faker.Person.Email;
        var cpf = Faker.Random.Replace("###.###.###-##");
        var dataNascimento = Faker.Person.DateOfBirth;
        var telefone = Faker.Phone.PhoneNumber();
        var genero = Faker.PickRandom("Masculino", "Feminino", "Outro");
        var cidade = Faker.Address.City();
        var estado = Faker.Address.State();
        var cep = Faker.Random.Replace("#####-###");

        // Act
        var user = new ApplicationUser
        {
            Nome = nome,
            UserName = email,
            Email = email,
            CPF = cpf,
            DataNascimento = dataNascimento,
            Telefone = telefone,
            Genero = genero,
            Cidade = cidade,
            Estado = estado,
            CEP = cep
        };

        // Assert
        user.Should().NotBeNull();
        user.Nome.Should().Be(nome);
        user.Email.Should().Be(email);
        user.CPF.Should().Be(cpf);
        user.DataNascimento.Should().Be(dataNascimento);
        user.Telefone.Should().Be(telefone);
        user.Genero.Should().Be(genero);
        user.Cidade.Should().Be(cidade);
        user.Estado.Should().Be(estado);
        user.CEP.Should().Be(cep);
        user.DataCadastro.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        user.Ativo.Should().BeTrue();
    }

    [Fact]
    public void ApplicationUser_DeveDefinirDataCadastroAutomaticamente()
    {
        // Arrange & Act
        var user = new ApplicationUser();

        // Assert
        user.DataCadastro.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
    }

    [Fact]
    public void ApplicationUser_DeveIniciarAtivoComoTrue()
    {
        // Arrange & Act
        var user = new ApplicationUser();

        // Assert
        user.Ativo.Should().BeTrue();
    }

    [Fact]
    public void ApplicationUser_DevePermitirAlterarStatusAtivo()
    {
        // Arrange
        var user = new ApplicationUser();

        // Act
        user.Ativo = false;

        // Assert
        user.Ativo.Should().BeFalse();
    }

    [Fact]
    public void ApplicationUser_DevePermitirDefinirRefreshToken()
    {
        // Arrange
        var user = new ApplicationUser();
        var refreshToken = Guid.NewGuid();
        var expiryTime = DateTime.UtcNow.AddHours(1);

        // Act
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = expiryTime;

        // Assert
        user.RefreshToken.Should().Be(refreshToken);
        user.RefreshTokenExpiryTime.Should().Be(expiryTime);
    }

    [Theory]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("ab")]
    [InlineData("abc")]
    [InlineData("abcd")]
    [InlineData("abcde")]
    [InlineData("abcdef")]
    [InlineData("abcdefg")]
    [InlineData("abcdefgh")]
    [InlineData("abcdefghi")]
    [InlineData("abcdefghij")]
    public void ApplicationUser_NomeDeveTerMinimo11Caracteres(string nome)
    {
        // Arrange & Act
        var user = new ApplicationUser { Nome = nome };

        // Assert
        user.Nome.Should().Be(nome);
    }

    [Theory]
    [InlineData("12345678901")]
    [InlineData("123.456.789-01")]
    public void ApplicationUser_CPFDeveTerTamanhoValido(string cpf)
    {
        // Arrange & Act
        var user = new ApplicationUser { CPF = cpf };

        // Assert
        user.CPF.Should().Be(cpf);
    }
}
