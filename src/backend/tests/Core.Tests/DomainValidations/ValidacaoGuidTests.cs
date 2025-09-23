using Core.DomainValidations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Tests.DomainValidations;
public class ValidacaoGuidTests
{
    private class Dummy { }

    [Fact]
    public void ValidacaoGuid_deve_adicionar_erro_quando_empty()
    {
        var r = new ResultadoValidacao<Dummy>();
        ValidacaoGuid.DeveSerValido(Guid.Empty, "guid inválido", r);

        r.Invoking(x => x.DispararExcecaoDominioSeInvalido()).Should().Throw<Exception>();
    }

    [Fact]
    public void ValidacaoGuid_nao_deve_adicionar_erro_quando_ok()
    {
        var r = new ResultadoValidacao<Dummy>();
        ValidacaoGuid.DeveSerValido(Guid.NewGuid(), "guid inválido", r);

        r.Invoking(x => x.DispararExcecaoDominioSeInvalido()).Should().NotThrow();
    }
}
