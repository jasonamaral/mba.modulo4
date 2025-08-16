using FluentAssertions;
using Xunit;

namespace Pagamentos.IntegrationTests.API
{
    public class PagamentosControllerIntegrationTests
    {
        [Fact]
        public void TesteBasico_DevePassar()
        {
            // Arrange & Act & Assert
            var resultado = true;
            resultado.Should().BeTrue();
        }
    }
}
