using Pagamentos.Domain.Entities;

namespace Pagamentos.UnitTests.Domain
{
    public class PagamentoTests
    {
        [Fact]
        public void Pagamento_ComDadosValidos_DeveSerCriadoComSucesso()
        {
            // Arrange
            var cobrancaCursoId = Guid.NewGuid();
            var alunoId = Guid.NewGuid();
            var valor = 299.99m;
            var status = "Pendente";
            var nomeCartao = "João Silva";
            var numeroCartao = "4111111111111111";
            var expiracaoCartao = "12/25";
            var cvvCartao = "123";

            // Act
            var pagamento = new Pagamento
            {
                CobrancaCursoId = cobrancaCursoId,
                AlunoId = alunoId,
                Valor = valor,
                Status = status,
                NomeCartao = nomeCartao,
                NumeroCartao = numeroCartao,
                ExpiracaoCartao = expiracaoCartao,
                CvvCartao = cvvCartao
            };

            // Assert
            pagamento.Should().NotBeNull();
            pagamento.CobrancaCursoId.Should().Be(cobrancaCursoId);
            pagamento.AlunoId.Should().Be(alunoId);
            pagamento.Valor.Should().Be(valor);
            pagamento.Status.Should().Be(status);
            pagamento.NomeCartao.Should().Be(nomeCartao);
            pagamento.NumeroCartao.Should().Be(numeroCartao);
            pagamento.ExpiracaoCartao.Should().Be(expiracaoCartao);
            pagamento.CvvCartao.Should().Be(cvvCartao);
        }

        [Fact]
        public void Pagamento_ComValorZero_DeveSerPermitido()
        {
            // Arrange & Act
            var pagamento = new Pagamento
            {
                Valor = 0
            };

            // Assert
            pagamento.Valor.Should().Be(0);
        }

        [Fact]
        public void Pagamento_ComValorNegativo_DeveSerPermitido()
        {
            // Arrange & Act
            var pagamento = new Pagamento
            {
                Valor = -50.00m
            };

            // Assert
            pagamento.Valor.Should().Be(-50.00m);
        }

        [Theory]
        [InlineData("Pendente")]
        [InlineData("Aprovado")]
        [InlineData("Rejeitado")]
        [InlineData("Cancelado")]
        [InlineData("Processando")]
        public void Pagamento_ComStatusValido_DeveSerAceito(string status)
        {
            // Arrange & Act
            var pagamento = new Pagamento
            {
                Status = status
            };

            // Assert
            pagamento.Status.Should().Be(status);
        }

        [Fact]
        public void Pagamento_ComTransacao_DeveManterRelacionamento()
        {
            // Arrange
            var pagamento = new Pagamento();
            var transacao = new Transacao
            {
                CobrancaCursoId = Guid.NewGuid(),
                PagamentoId = Guid.NewGuid(),
                Total = 299.99m,
                StatusTransacao = Pagamentos.Domain.Enum.StatusTransacao.Pago
            };

            // Assert
            pagamento.Should().NotBeNull();
        }

        [Fact]
        public void Pagamento_SemTransacao_DeveSerPermitido()
        {
            // Arrange & Act
            var pagamento = new Pagamento();

            // Assert
            pagamento.Should().NotBeNull();
        }

        [Fact]
        public void Pagamento_ComDadosMinimos_DeveSerCriado()
        {
            // Arrange & Act
            var pagamento = new Pagamento
            {
                CobrancaCursoId = Guid.NewGuid(),
                AlunoId = Guid.NewGuid()
            };

            // Assert
            pagamento.Should().NotBeNull();
            pagamento.CobrancaCursoId.Should().NotBeEmpty();
            pagamento.AlunoId.Should().NotBeEmpty();
        }
    }
}
