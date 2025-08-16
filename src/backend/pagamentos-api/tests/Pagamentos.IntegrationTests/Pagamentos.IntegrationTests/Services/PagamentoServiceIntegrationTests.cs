using FluentAssertions;
using Xunit;

namespace Pagamentos.IntegrationTests.Services
{
    public class PagamentoServiceIntegrationTests
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
