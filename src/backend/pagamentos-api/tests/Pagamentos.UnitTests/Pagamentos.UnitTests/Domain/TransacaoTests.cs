using FluentAssertions;
using Pagamentos.Domain.Entities;
using Pagamentos.Domain.Enum;
using Xunit;

namespace Pagamentos.UnitTests.Domain
{
    public class TransacaoTests
    {
        [Fact]
        public void Transacao_ComDadosValidos_DeveSerCriadaComSucesso()
        {
            // Arrange
            var cobrancaCursoId = Guid.NewGuid();
            var pagamentoId = Guid.NewGuid();
            var total = 299.99m;
            var status = StatusTransacao.Pago;

            // Act
            var transacao = new Transacao
            {
                CobrancaCursoId = cobrancaCursoId,
                PagamentoId = pagamentoId,
                Total = total,
                StatusTransacao = status
            };

            // Assert
            transacao.Should().NotBeNull();
            transacao.CobrancaCursoId.Should().Be(cobrancaCursoId);
            transacao.PagamentoId.Should().Be(pagamentoId);
            transacao.Total.Should().Be(total);
            transacao.StatusTransacao.Should().Be(status);
        }

        [Theory]
        [InlineData(StatusTransacao.Pago)]
        [InlineData(StatusTransacao.Recusado)]
        public void Transacao_ComStatusValido_DeveSerAceita(StatusTransacao status)
        {
            // Arrange & Act
            var transacao = new Transacao
            {
                StatusTransacao = status
            };

            // Assert
            transacao.StatusTransacao.Should().Be(status);
        }

        [Fact]
        public void Transacao_ComTotalZero_DeveSerPermitida()
        {
            // Arrange & Act
            var transacao = new Transacao
            {
                Total = 0
            };

            // Assert
            transacao.Total.Should().Be(0);
        }

        [Fact]
        public void Transacao_ComTotalNegativo_DeveSerPermitida()
        {
            // Arrange & Act
            var transacao = new Transacao
            {
                Total = -50.00m
            };

            // Assert
            transacao.Total.Should().Be(-50.00m);
        }

        [Fact]
        public void Transacao_ComRelacionamentoPagamento_DeveManterReferencia()
        {
            // Arrange
            var transacao = new Transacao();
            var pagamento = new Pagamento
            {
                CobrancaCursoId = Guid.NewGuid(),
                AlunoId = Guid.NewGuid()
            };

            // Act
            transacao.Pagamento = pagamento;

            // Assert
            transacao.Pagamento.Should().NotBeNull();
            transacao.Pagamento.Should().Be(pagamento);
        }

        [Fact]
        public void Transacao_SemRelacionamentoPagamento_DeveSerPermitida()
        {
            // Arrange & Act
            var transacao = new Transacao();

            // Assert
            transacao.Pagamento.Should().BeNull();
        }

        [Fact]
        public void Transacao_ComIdsVazios_DeveSerPermitida()
        {
            // Arrange & Act
            var transacao = new Transacao
            {
                CobrancaCursoId = Guid.Empty,
                PagamentoId = Guid.Empty
            };

            // Assert
            transacao.CobrancaCursoId.Should().Be(Guid.Empty);
            transacao.PagamentoId.Should().Be(Guid.Empty);
        }
    }
}
