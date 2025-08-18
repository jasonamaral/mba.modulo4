using Alunos.Domain.ValueObjects;
using Core.DomainObjects;
using Core.DomainValidations;
using Plataforma.Educacao.Core.Exceptions;

namespace Alunos.Domain.Entities;

public class Aluno : Entidade, IRaizAgregacao
{
    #region Atributos
    public Guid CodigoUsuarioAutenticacao { get; }
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string Cpf { get; }
    public DateTime DataNascimento { get; private set; }
    public string Telefone { get; private set; }
    public bool Ativo { get; private set; }
    public string Genero { get; private set; }
    public string Cidade { get; private set; }
    public string Estado { get; private set; }
    public string Cep { get; private set; }
    public string? Foto { get; private set; }

    private readonly List<MatriculaCurso> _matriculasCursos = [];
    public IReadOnlyCollection<MatriculaCurso> MatriculasCursos => _matriculasCursos.AsReadOnly();
    #endregion

    #region CTOR
    // EF Compatibility
    protected Aluno() { }

    public Aluno(Guid codigoUsuarioAutenticacao,
        string nome,
        string email,
        string cpf,
        DateTime dataNascimento,
        string genero,
        string cidade,
        string estado,
        string cep,
        string foto)
    {
        CodigoUsuarioAutenticacao = codigoUsuarioAutenticacao;
        Nome = nome?.Trim() ?? string.Empty;
        Email = email?.Trim().ToLowerInvariant() ?? string.Empty;
        Cpf = cpf?.Trim() ?? string.Empty;
        DataNascimento = dataNascimento.Date;
        Genero = genero?.Trim() ?? string.Empty;
        Cidade = cidade?.Trim() ?? string.Empty;
        Estado = estado?.Trim() ?? string.Empty;
        Cep = cep?.Trim().Replace("-", string.Empty).Replace(".", string.Empty) ?? string.Empty;
        Foto = foto?.Trim() ?? string.Empty;

        ValidarIntegridadeAluno();
    }
    #endregion

    #region Métodos
    #region Manipuladores de Aluno
    public void AtivarAluno() => Ativo = true;
    public void InativarAluno() => Ativo = false;

    internal void AtualizarNomeAluno(string nome)
    {
        ValidarIntegridadeAluno(novoNome: nome ?? string.Empty);
        Nome = nome.Trim();
    }

    internal void AtualizarEmailAluno(string email)
    {
        ValidarIntegridadeAluno(novoEmail: email ?? string.Empty);
        Email = email.Trim().ToLowerInvariant();
    }

    internal void AtualizarContatoAluno(string contato)
    {
        ValidarIntegridadeAluno(novoContato: contato ?? string.Empty);
        Telefone = contato.Trim();
    }

    public void AtualizarDataNascimento(DateTime dataNascimento)
    {
        ValidarIntegridadeAluno(novaDataNascimento: dataNascimento);
        DataNascimento = dataNascimento;
    }
    #endregion

    #region Manipuladores de MatriculaCurso
    public int ObterQuantidadeAulasPendenteMatriculaCurso(Guid cursoId)
    {
        return _matriculasCursos.Count(m => m.CursoId == cursoId && m.PodeConcluirCurso() == false);
    }

    public MatriculaCurso ObterMatriculaPorCursoId(Guid cursoId)
    {
        var matriculaCurso = _matriculasCursos.FirstOrDefault(m => m.CursoId == cursoId);
        if (matriculaCurso == null) { throw new DomainException("Matrícula pelo Curso não foi localizada"); }

        return matriculaCurso;
    }

    public MatriculaCurso ObterMatriculaCursoPeloId(Guid matriculaCursoId)
    {
        var matriculaCurso = _matriculasCursos.FirstOrDefault(m => m.Id == matriculaCursoId);
        if (matriculaCurso == null) { throw new DomainException("Matrícula não foi localizada"); }

        return matriculaCurso;
    }

    public void MatricularAlunoEmCurso(Guid cursoId, string nomeCurso, decimal valor, string observacao)
    {
        if (_matriculasCursos.Any(m => m.CursoId == cursoId)) { throw new DomainException("Aluno já está matriculado neste curso"); }
        if (!Ativo) { throw new DomainException("Aluno inativo não pode ser matriculado em cursos"); }

        var novaMatricula = new MatriculaCurso(Id, cursoId, nomeCurso, valor, observacao);
        _matriculasCursos.Add(novaMatricula);
    }

    internal void AtualizarNotaFinalCurso(Guid matriculaCursoId, byte notaFinal)
    {
        MatriculaCurso matriculaCurso = ObterMatriculaCursoPeloId(matriculaCursoId);
        matriculaCurso.AtualizarNotaFinalCurso(notaFinal);
    }

    public void AtualizarPagamentoMatricula(Guid matriculaCursoId)
    {
        MatriculaCurso matriculaCurso = ObterMatriculaCursoPeloId(matriculaCursoId);
        matriculaCurso.RegistrarPagamentoMatricula();
    }

    public void AtualizarAbandonoMatricula(Guid matriculaCursoId)
    {
        MatriculaCurso matriculaCurso = ObterMatriculaCursoPeloId(matriculaCursoId);
        matriculaCurso.RegistrarAbandonoMatricula();
    }

    public void ConcluirCurso(Guid matriculaCursoId)
    {
        MatriculaCurso matriculaCurso = ObterMatriculaCursoPeloId(matriculaCursoId);
        matriculaCurso.ConcluirCurso();
    }

    internal void AtualizarObservacoes(Guid matriculaCursoId, string observacao)
    {
        MatriculaCurso matriculaCurso = ObterMatriculaCursoPeloId(matriculaCursoId);
        matriculaCurso.AtualizarObservacoes(observacao);
    }
    #endregion

    #region Manipuladores de HistoricoAprendizado
    public void RegistrarHistoricoAprendizado(Guid matriculaCursoId, Guid aulaId, string nomeAula, int cargaHoraria, DateTime? dataTermino = null)
    {
        MatriculaCurso matriculaCurso = ObterMatriculaCursoPeloId(matriculaCursoId);
        matriculaCurso.RegistrarHistoricoAprendizado(aulaId, nomeAula, cargaHoraria, dataTermino);
    }

    public HistoricoAprendizado ObterHistoricoAprendizado(Guid matriculaCursoId, Guid aulaId)
    {
        var matriculaCurso = ObterMatriculaCursoPeloId(matriculaCursoId);
        var historico = matriculaCurso.HistoricoAprendizado.FirstOrDefault(h => h.AulaId == aulaId);
        //if (historico == null) { throw new DomainException("Histórico de aprendizado não foi localizado"); }
        return historico;
    }

    public int ObterQuantidadeAulasMatriculaCurso(Guid matriculaCursoId)
    {
        var matriculaCurso = ObterMatriculaCursoPeloId(matriculaCursoId);
        return matriculaCurso.ObterQuantidadeAulasRegistradas();
    }
    #endregion

    #region Manipuladores de Certificado
    public void RequisitarCertificadoConclusao(Guid matriculaCursoId, decimal notaFinal, string pathCertificado, string nomeInstrutor)
    {
        MatriculaCurso matriculaCurso = ObterMatriculaCursoPeloId(matriculaCursoId);
        matriculaCurso.RequisitarCertificadoConclusao(notaFinal, pathCertificado, nomeInstrutor);
    }

    public void ComunicarDataEmissaoCertificado(Guid matriculaCursoId, DateTime dataEmissao)
    {
        MatriculaCurso matriculaCurso = ObterMatriculaCursoPeloId(matriculaCursoId);
        matriculaCurso.ComunicarDataEmissaoCertificado(dataEmissao);
    }

    public void AtualizarCargaHoraria(Guid matriculaCursoId, short cargaHoraria)
    {
        MatriculaCurso matriculaCurso = ObterMatriculaCursoPeloId(matriculaCursoId);
        matriculaCurso.AtualizarCargaHoraria(cargaHoraria);
    }

    public void AtualizarPathCertificado(Guid matriculaCursoId, string path)
    {
        MatriculaCurso matriculaCurso = ObterMatriculaCursoPeloId(matriculaCursoId);
        matriculaCurso.AtualizarPathCertificado(path);
    }

    public void AtualizarNomeInstrutor(Guid matriculaCursoId, string nomeInstrutor)
    {
        MatriculaCurso matriculaCurso = ObterMatriculaCursoPeloId(matriculaCursoId);
        matriculaCurso.AtualizarNomeInstrutor(nomeInstrutor);
    }
    #endregion

    private void ValidarIntegridadeAluno(string novoNome = null,
        string novoEmail = null,
        string novoContato = null,
        DateTime? novaDataNascimento = null)
    {
        novoNome ??= Nome;
        novoEmail ??= Email;
        novoContato ??= Telefone;
        novaDataNascimento ??= DataNascimento;

        var validacao = new ResultadoValidacao<Aluno>();

        ValidacaoGuid.DeveSerValido(CodigoUsuarioAutenticacao, "Código de identificação deve ser informado", validacao);
        ValidacaoTexto.DevePossuirConteudo(novoNome, "Nome não pode ser nulo ou vazio", validacao);
        ValidacaoTexto.DevePossuirTamanho(novoNome, 3, 100, "Nome deve ter entre 3 e 100 caracteres", validacao);
        ValidacaoTexto.DevePossuirConteudo(novoEmail, "Email não pode ser nulo ou vazio", validacao);
        ValidacaoTexto.DevePossuirTamanho(novoEmail, 3, 100, "Email deve ter entre 3 e 100 caracteres", validacao);
        ValidacaoTexto.DeveAtenderRegex(novoEmail, @"^[\w\.\-]+@([\w\-]+\.)+[\w\-]{2,}$", "Email informado é inválido", validacao);
        ValidacaoTexto.DevePossuirConteudo(Cpf, "CPF não pode ser nulo ou vazio", validacao);
        ValidacaoTexto.DeveSerCpfValido(Cpf, "CPF informado é inválido", validacao);
        ValidacaoData.DeveSerValido(novaDataNascimento.Value, "Data de nascimento deve ser válida", validacao);
        ValidacaoData.DeveSerMenorQue(novaDataNascimento.Value, DateTime.Now, "Data de nascimento não pode ser superior à data atual", validacao);
        ValidacaoTexto.DevePossuirConteudo(Genero, "Genero não pode ser nulo ou vazio", validacao);
        ValidacaoTexto.DevePossuirTamanho(Genero, 1, 20, "Genero deve ter entre 1 e 20 caracteres", validacao);
        ValidacaoTexto.DevePossuirConteudo(Cidade, "Cidade não pode ser nulo ou vazio", validacao);
        ValidacaoTexto.DevePossuirTamanho(Cidade, 1, 50, "Cidade deve ter entre 1 e 50 caracteres", validacao);
        ValidacaoTexto.DevePossuirConteudo(Cidade, "Cidade não pode ser nulo ou vazio", validacao);
        ValidacaoTexto.DevePossuirTamanho(Estado, 2, 2, "Estado deve ter 2 caracteres", validacao);
        ValidacaoTexto.DevePossuirConteudo(Cep, "Cep não pode ser nulo ou vazio", validacao);
        ValidacaoTexto.DevePossuirTamanho(Cep, 1, 8, "Cep deve ter até 8 caracteres", validacao);

        if (!string.IsNullOrWhiteSpace(Foto))
        {
            ValidacaoTexto.DevePossuirTamanho(Foto, 1, 1024, "Foto deve ter entre 1 e 1024 caracteres", validacao);
        }

        if (!string.IsNullOrWhiteSpace(novoContato))
        {
            ValidacaoTexto.DevePossuirTamanho(novoContato, 1, 25, "Contato deve ter entre 1 e 25 caracteres", validacao);
        }

        validacao.DispararExcecaoDominioSeInvalido();
    }
    #endregion

    public override string ToString() => $"{Nome} (Email: {Email})";
}