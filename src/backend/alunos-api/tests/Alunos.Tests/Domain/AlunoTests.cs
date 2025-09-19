using Alunos.Domain.Entities;
using FluentAssertions;
using Plataforma.Educacao.Core.Exceptions;

namespace Alunos.Tests.Domain;

public class AlunoTests
{
    private static readonly Guid _codigoUsuario = Guid.NewGuid();
    private const string _nomeValido = "Jairo Azevedo";
    private const string _emailValido = "jsouza.lp@gmail.com";
    private const string _cpfValido = "12345678909";
    private static readonly DateTime _dataNascimentoValida = new(1973, 06, 25);
    private const string _generoValido = "Masculino";
    private const string _cidadeValido = "Rio de Janeiro";
    private const string _estadoValido = "RJ";
    private const string _cepValido = "21210000";
    private const string _fotoValido = "var/mnt/fotos/jairo.jpeg";

    private Aluno CriarAlunoValido() => new(_codigoUsuario, _nomeValido, _emailValido, _cpfValido, _dataNascimentoValida, _generoValido, _cidadeValido, _estadoValido, _cepValido, _fotoValido);

    [Fact]
    public void Deve_criar_aluno_valido()
    {
        var aluno = CriarAlunoValido();

        aluno.Should().NotBeNull();
        aluno.CodigoUsuarioAutenticacao.Should().Be(_codigoUsuario);
        aluno.Nome.Should().Be(_nomeValido);
        aluno.Email.Should().Be(_emailValido);
        aluno.Cpf.Should().Be(_cpfValido);
        aluno.DataNascimento.Should().Be(_dataNascimentoValida);
        aluno.Genero.Should().Be(_generoValido);
        aluno.Cidade.Should().Be(_cidadeValido);
        aluno.Estado.Should().Be(_estadoValido);
        aluno.Cep.Should().Be(_cepValido);
        aluno.Foto.Should().Be(_fotoValido);
    }

    [Theory]
    [InlineData("", "*Nome não pode ser nulo ou vazio*")]
    [InlineData("Jo", "*Nome deve ter entre 3 e 100 caracteres*")]
    public void Nao_deve_criar_aluno_com_nome_invalido(string nome, string msgErro)
    {
        Action act = () => new Aluno(_codigoUsuario, nome, _emailValido, _cpfValido, _dataNascimentoValida, _generoValido, _cidadeValido, _estadoValido, _cepValido, _fotoValido);
        act.Should().Throw<DomainException>().WithMessage(msgErro);
    }

    [Theory]
    [InlineData("", "*Email não pode ser nulo ou vazio*")]
    [InlineData("a", "*Email deve ter entre 3 e 100 caracteres*")]
    [InlineData("emailinvalido", "*Email informado é inválido*")]
    public void Nao_deve_criar_aluno_com_email_invalido(string email, string msgErro)
    {
        Action act = () => new Aluno(_codigoUsuario, _nomeValido, email, _cpfValido, _dataNascimentoValida, _generoValido, _cidadeValido, _estadoValido, _cepValido, _fotoValido);
        act.Should().Throw<DomainException>().WithMessage(msgErro);
    }

    [Fact]
    public void Nao_deve_criar_aluno_com_data_nascimento_futura()
    {
        var dataFutura = DateTime.UtcNow.AddDays(1);
        Action act = () => new Aluno(_codigoUsuario, _nomeValido, _emailValido, _cpfValido, dataFutura, _generoValido, _cidadeValido, _estadoValido, _cepValido, _fotoValido);
        act.Should().Throw<DomainException>().WithMessage("*Data de nascimento não pode ser superior à data atual*");
    }

    [Fact]
    public void Deve_ativar_e_inativar_aluno()
    {
        var aluno = CriarAlunoValido();
        aluno.InativarAluno();
        aluno.Ativo.Should().BeFalse();

        aluno.AtivarAluno();
        aluno.Ativo.Should().BeTrue();
    }

    [Fact]
    public void Deve_atualizar_nome_email_contato()
    {
        var aluno = CriarAlunoValido();
        aluno.AtualizarNomeAluno("Novo Nome");
        aluno.AtualizarEmailAluno("novo@email.com");
        aluno.AtualizarContatoAluno("12345");

        aluno.Nome.Should().Be("Novo Nome");
        aluno.Email.Should().Be("novo@email.com");
        aluno.Telefone.Should().Be("12345");
    }

    [Fact]
    public void Deve_atualizar_data_nascimento()
    {
        var aluno = CriarAlunoValido();
        var novaData = new DateTime(1985, 5, 20);
        aluno.AtualizarDataNascimento(novaData);

        aluno.DataNascimento.Should().Be(novaData);
    }

    [Fact]
    public void Nao_deve_matricular_aluno_inativo()
    {
        var aluno = CriarAlunoValido();
        aluno.InativarAluno();

        Action act = () => aluno.MatricularAlunoEmCurso(Guid.NewGuid(), "Curso A", 1000, "obs");
        act.Should().Throw<DomainException>().WithMessage("*Aluno inativo não pode ser matriculado*");
    }

    [Fact]
    public void Nao_deve_matricular_aluno_em_curso_duplicado()
    {
        var aluno = CriarAlunoValido();
        aluno.AtivarAluno();
        var cursoId = Guid.NewGuid();
        aluno.MatricularAlunoEmCurso(cursoId, "Curso de Programação Avançada", 1000, "obs");

        Action act = () => aluno.MatricularAlunoEmCurso(cursoId, "Curso de Programação Avançada", 1000, "obs");
        act.Should().Throw<DomainException>().WithMessage("*Aluno já está matriculado neste curso*");
    }

    [Fact]
    public void ToString_deve_conter_nome_e_email()
    {
        var aluno = CriarAlunoValido();
        var texto = aluno.ToString();

        texto.Should().Contain(_nomeValido)
              .And.Contain(_emailValido);
    }
}
