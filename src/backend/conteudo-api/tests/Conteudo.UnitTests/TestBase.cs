namespace Conteudo.UnitTests;

public abstract class TestBase
{
    protected TestBase()
    {
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
