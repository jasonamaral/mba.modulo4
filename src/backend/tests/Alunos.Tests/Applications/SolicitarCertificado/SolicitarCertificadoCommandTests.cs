using Alunos.Application.Commands.SolicitarCertificado;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alunos.Tests.Applications.SolicitarCertificado;
public class SolicitarCertificadoCommandTests
{
    [Fact]
    public void Ctor_deve_definir_RaizAgregacao_com_AlunoId()
    {
        var aluno = Guid.NewGuid();
        var cmd = new SolicitarCertificadoCommand(aluno, Guid.NewGuid());

        cmd.RaizAgregacao.Should().Be(aluno);
    }
}
