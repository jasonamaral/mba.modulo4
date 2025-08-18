namespace Alunos.UnitTests;

public class SimpleTest
{
    [Fact]
    public void TesteSimples_DevePassar()
    {
        // Arrange
        var valor = 1;

        // Act
        var resultado = valor + 1;

        // Assert
        Assert.Equal(2, resultado);
    }

    [Fact]
    public void TesteString_DevePassar()
    {
        // Arrange
        var texto = "Hello World";

        // Act & Assert
        Assert.Contains("Hello", texto);
        Assert.Contains("World", texto);
        Assert.Equal(11, texto.Length);
    }

    [Fact]
    public void TesteBoolean_DevePassar()
    {
        // Arrange
        var verdadeiro = true;
        var falso = false;

        // Act & Assert
        Assert.True(verdadeiro);
        Assert.False(falso);
        Assert.NotEqual(verdadeiro, falso);
    }
}
