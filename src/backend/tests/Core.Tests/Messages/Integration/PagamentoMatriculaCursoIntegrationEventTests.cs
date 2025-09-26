using Core.Messages.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Tests.Messages.Integration;
public class PagamentoMatriculaCursoIntegrationEventTests
{
    [Fact]
    public void Construtor_deve_definir_RaizAgregacao_com_AlunoId()
    {
        var alunoId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();

        var evt = new PagamentoMatriculaCursoIntegrationEvent(alunoId, cursoId);

        evt.AlunoId.Should().Be(alunoId);
        evt.CursoId.Should().Be(cursoId);
        evt.RaizAgregacao.Should().Be(alunoId);
    }
}
