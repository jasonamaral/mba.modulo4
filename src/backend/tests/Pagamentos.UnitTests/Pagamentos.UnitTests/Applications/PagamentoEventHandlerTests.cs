using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Messages.Integration;
using FluentAssertions;
using global::Pagamentos.Domain.Events;
using global::Pagamentos.Domain.Models;
using MediatR;
using Moq;
using Pagamentos.Domain.Events;
using Pagamentos.Domain.Interfaces;
using Pagamentos.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Pagamentos.UnitTests.Applications;
public class PagamentoEventHandlerTests
{
    [Fact]
    public async Task Handle_deve_mapear_evento_e_chamar_servico()
    {
        var svc = new Mock<IPagamentoService>();
        svc.Setup(s => s.RealizarPagamento(It.IsAny<PagamentoCurso>()))
           .ReturnsAsync(new Pagamentos.Domain.Entities.Transacao());

        var handler = new PagamentoEventHandler(svc.Object);

        var evt = new PagamentoCursoEvent(
            cursoId: Guid.NewGuid(),
            clienteId: Guid.NewGuid(),
            total: 123.45m,
            nomeCartao: "NOME",
            numeroCartao: "4111111111111111",
            expiracaoCartao: "12/30",
            cvvCartao: "123");

        await handler.Handle(evt, CancellationToken.None);

        svc.Verify(s => s.RealizarPagamento(It.Is<PagamentoCurso>(p =>
            p.CursoId == evt.CursoId &&
            p.ClienteId == evt.AlunoId &&
            p.Total == evt.Total &&
            p.NomeCartao == evt.NomeCartao &&
            p.NumeroCartao == evt.NumeroCartao &&
            p.ExpiracaoCartao == evt.ExpiracaoCartao &&
            p.CvvCartao == evt.CvvCartao
        )), Times.Once);
    }
}
