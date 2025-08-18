using FluentAssertions;
using Xunit;

namespace Pagamentos.UnitTests
{
    public class BasicTests
    {
        [Fact]
        public void TesteBasico_DevePassar()
        {
            // Arrange & Act & Assert
            var resultado = true;
            resultado.Should().BeTrue();
        }

        [Fact]
        public void TesteMatematico_DeveCalcularCorretamente()
        {
            // Arrange
            var a = 5;
            var b = 3;

            // Act
            var soma = a + b;
            var multiplicacao = a * b;

            // Assert
            soma.Should().Be(8);
            multiplicacao.Should().Be(15);
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(5, 5, 10)]
        [InlineData(-1, 1, 0)]
        public void TesteTeoria_DeveSomarCorretamente(int a, int b, int resultadoEsperado)
        {
            // Act
            var resultado = a + b;

            // Assert
            resultado.Should().Be(resultadoEsperado);
        }

        [Fact]
        public void TesteString_DeveConcatenarCorretamente()
        {
            // Arrange
            var nome = "João";
            var sobrenome = "Silva";

            // Act
            var nomeCompleto = $"{nome} {sobrenome}";

            // Assert
            nomeCompleto.Should().Be("João Silva");
            nomeCompleto.Should().Contain("João");
            nomeCompleto.Should().Contain("Silva");
        }

        [Fact]
        public void TesteLista_DeveManipularCorretamente()
        {
            // Arrange
            var numeros = new List<int> { 1, 2, 3, 4, 5 };

            // Act
            var quantidade = numeros.Count;
            var soma = numeros.Sum();
            var media = numeros.Average();

            // Assert
            quantidade.Should().Be(5);
            soma.Should().Be(15);
            media.Should().Be(3);
        }
    }
}
