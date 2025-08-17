using FluentAssertions;
using Xunit;

namespace Conteudo.UnitTests;

public abstract class TestBase
{
    protected TestBase()
    {
        // Configurações comuns para todos os testes
    }

    protected static void AssertSuccess<T>(T result)
    {
        result.Should().NotBeNull();
    }

    protected static void AssertFailure<T>(T result)
    {
        result.Should().BeNull();
    }
}
