using Xunit;

namespace BFF.UnitTests;

public class SimpleTest
{
    [Fact]
    public void TesteSimples_DevePassar()
    {
        // Arrange
        var expected = true;

        // Act
        var actual = true;

        // Assert
        Assert.Equal(expected, actual);
    }
}
