using Core.Messages.Integration;

namespace Pagamentos.UnitTests.Domain
{
    public class PagamentoCursoEventTests
    {
        [Fact]
        public void PagamentoCursoEvent_ComDadosValidos_DeveSerCriadoComSucesso()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var alunoId = Guid.NewGuid();
            var total = 299.99m;
            var nomeCartao = "João Silva";
            var numeroCartao = "4111111111111111";
            var expiracaoCartao = "12/25";
            var cvvCartao = "123";

            // Act
            var evento = new PagamentoCursoEvent(
                cursoId,
                alunoId,
                total,
                nomeCartao,
                numeroCartao,
                expiracaoCartao,
                cvvCartao
            );

            // Assert
            evento.Should().NotBeNull();
            evento.CursoId.Should().Be(cursoId);
            evento.AlunoId.Should().Be(alunoId);
            evento.Total.Should().Be(total);
            evento.NomeCartao.Should().Be(nomeCartao);
            evento.NumeroCartao.Should().Be(numeroCartao);
            evento.ExpiracaoCartao.Should().Be(expiracaoCartao);
            evento.CvvCartao.Should().Be(cvvCartao);
        }

        [Fact]
        public void PagamentoCursoEvent_ComValorZero_DeveSerPermitido()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var alunoId = Guid.NewGuid();
            var total = 0m;

            // Act
            var evento = new PagamentoCursoEvent(
                cursoId,
                alunoId,
                total,
                "João Silva",
                "4111111111111111",
                "12/25",
                "123"
            );

            // Assert
            evento.Total.Should().Be(0);
        }

        [Fact]
        public void PagamentoCursoEvent_ComValorNegativo_DeveSerPermitido()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var alunoId = Guid.NewGuid();
            var total = -50.00m;

            // Act
            var evento = new PagamentoCursoEvent(
                cursoId,
                alunoId,
                total,
                "João Silva",
                "4111111111111111",
                "12/25",
                "123"
            );

            // Assert
            evento.Total.Should().Be(-50.00m);
        }

        [Fact]
        public void PagamentoCursoEvent_ComIdsVazios_DeveSerPermitido()
        {
            // Arrange
            var cursoId = Guid.Empty;
            var alunoId = Guid.Empty;

            // Act
            var evento = new PagamentoCursoEvent(
                cursoId,
                alunoId,
                100.00m,
                "João Silva",
                "4111111111111111",
                "12/25",
                "123"
            );

            // Assert
            evento.CursoId.Should().Be(Guid.Empty);
            evento.AlunoId.Should().Be(Guid.Empty);
        }

        [Fact]
        public void PagamentoCursoEvent_ComDadosMinimos_DeveSerCriado()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var alunoId = Guid.NewGuid();

            // Act
            var evento = new PagamentoCursoEvent(
                cursoId,
                alunoId,
                0,
                "",
                "",
                "",
                ""
            );

            // Assert
            evento.Should().NotBeNull();
            evento.CursoId.Should().Be(cursoId);
            evento.AlunoId.Should().Be(alunoId);
        }
    }
}
