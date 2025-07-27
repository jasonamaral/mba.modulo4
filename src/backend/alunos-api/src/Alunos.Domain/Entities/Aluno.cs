using Alunos.Domain.Common;

namespace Alunos.Domain.Entities;

public class Aluno : Entidade, IRaizAgregacao
{
    public Guid CodigoUsuarioAutenticacao { get; private set; }

    public string Nome { get; private set; }

    public string Email { get; private set; }

    public string CPF { get; private set; }

    public DateTime DataNascimento { get; private set; }

    public string Telefone { get; private set; }

    public string Genero { get; private set; }

    public string Cidade { get; private set; }

    public string Estado { get; private set; }

    public string CEP { get; private set; }

    public bool IsAtivo { get; private set; }

    private readonly List<MatriculaCurso> _matriculasCursos = new();

    public IReadOnlyCollection<MatriculaCurso> MatriculasCursos => _matriculasCursos.AsReadOnly();

    protected Aluno() : base()
    {
        Nome = string.Empty;
        Email = string.Empty;
        Telefone = string.Empty;
        Genero = string.Empty;
        Cidade = string.Empty;
        Estado = string.Empty;
        CEP = string.Empty;
    }

    public Aluno(
        Guid codigoUsuarioAutenticacao,
        string nome,
        string email,
        string cpf,
        DateTime dataNascimento,
        string telefone = "",
        string genero = "",
        string cidade = "",
        string estado = "",
        string cep = "") : base()
    {
        ValidarDadosObrigatorios(nome, email, cpf);
        ValidarIdade(dataNascimento);
        ValidarEmail(email);
        ValidarCPF(cpf);

        CodigoUsuarioAutenticacao = codigoUsuarioAutenticacao;
        Nome = nome.Trim();
        Email = email.Trim().ToLowerInvariant();
        CPF = cpf.Trim();
        DataNascimento = dataNascimento.Date;
        Telefone = telefone?.Trim() ?? string.Empty;
        Genero = genero?.Trim() ?? string.Empty;
        Cidade = cidade?.Trim() ?? string.Empty;
        Estado = estado?.Trim() ?? string.Empty;
        CEP = cep?.Trim() ?? string.Empty;
        IsAtivo = true;
    }

    public void AtualizarDados(
        string nome,
        string email,
        string cpf,
        DateTime dataNascimento,
        string telefone = "",
        string genero = "",
        string cidade = "",
        string estado = "",
        string cep = "")
    {
        ValidarDadosObrigatorios(nome, email, cpf);
        ValidarIdade(dataNascimento);
        ValidarEmail(email);
        ValidarCPF(cpf);

        Nome = nome.Trim();
        Email = email.Trim().ToLowerInvariant();
        CPF = cpf.Trim();
        DataNascimento = dataNascimento.Date;
        Telefone = telefone?.Trim() ?? string.Empty;
        Genero = genero?.Trim() ?? string.Empty;
        Cidade = cidade?.Trim() ?? string.Empty;
        Estado = estado?.Trim() ?? string.Empty;
        CEP = cep?.Trim() ?? string.Empty;

        SetUpdatedAt();
    }

    public void Ativar()
    {
        IsAtivo = true;
        SetUpdatedAt();
    }

    public void Desativar()
    {
        IsAtivo = false;
        SetUpdatedAt();
    }

    public void AdicionarMatricula(MatriculaCurso matricula)
    {
        if (matricula == null)
            throw new ArgumentNullException(nameof(matricula));

        if (!IsAtivo)
            throw new InvalidOperationException("Não é possível matricular um aluno inativo.");

        var matriculaExistente = _matriculasCursos.FirstOrDefault(m => m.CursoId == matricula.CursoId);
        if (matriculaExistente != null)
            throw new InvalidOperationException("Aluno já possui matrícula neste curso.");

        _matriculasCursos.Add(matricula);
        SetUpdatedAt();
    }

    public void RemoverMatricula(Guid matriculaId)
    {
        var matricula = _matriculasCursos.FirstOrDefault(m => m.Id == matriculaId);
        if (matricula == null)
            throw new ArgumentException("Matrícula não encontrada.", nameof(matriculaId));

        _matriculasCursos.Remove(matricula);
        SetUpdatedAt();
    }

    public MatriculaCurso ObterMatricula(Guid matriculaId)
    {
        var matricula = _matriculasCursos.FirstOrDefault(m => m.Id == matriculaId);
        if (matricula == null)
            throw new ArgumentException("Matrícula não encontrada.", nameof(matriculaId));

        return matricula;
    }

    public MatriculaCurso ObterMatriculaPorCurso(Guid cursoId)
    {
        return _matriculasCursos.FirstOrDefault(m => m.CursoId == cursoId);
    }

    public bool EstaMatriculadoNoCurso(Guid cursoId)
    {
        return _matriculasCursos.Any(m => m.CursoId == cursoId);
    }

    public int CalcularIdade()
    {
        var hoje = DateTime.Today;
        var idade = hoje.Year - DataNascimento.Year;

        if (DataNascimento.Date > hoje.AddYears(-idade))
            idade--;

        return idade;
    }

    private static void ValidarDadosObrigatorios(string nome, string email, string cpf)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório.", nameof(nome));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email é obrigatório.", nameof(email));

        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("CPF é obrigatório.", nameof(cpf));

        if (nome.Length < 2)
            throw new ArgumentException("Nome deve ter pelo menos 2 caracteres.", nameof(nome));

        if (nome.Length > 100)
            throw new ArgumentException("Nome deve ter no máximo 100 caracteres.", nameof(nome));
    }

    private static void ValidarCPF(string cpf)
    {
        if (cpf.Length < 11 || cpf.Length > 14)
            throw new ArgumentException("CPF deve ter entre 11 e 14 caracteres.", nameof(cpf));

        // Remove caracteres não numéricos
        var cpfNumerico = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpfNumerico.Length != 11)
            throw new ArgumentException("CPF deve conter 11 dígitos numéricos.", nameof(cpf));

        // Validação básica de CPF (verificar se não são todos os mesmos dígitos)
        if (cpfNumerico.All(c => c == cpfNumerico[0]))
            throw new ArgumentException("CPF inválido.", nameof(cpf));
    }

    private static void ValidarIdade(DateTime dataNascimento)
    {
        if (dataNascimento.Date > DateTime.Today)
            throw new ArgumentException("Data de nascimento não pode ser futura.", nameof(dataNascimento));

        var idade = DateTime.Today.Year - dataNascimento.Year;
        if (dataNascimento.Date > DateTime.Today.AddYears(-idade))
            idade--;

        if (idade < 16)
            throw new ArgumentException("Aluno deve ter pelo menos 16 anos.", nameof(dataNascimento));

        if (idade > 100)
            throw new ArgumentException("Data de nascimento inválida.", nameof(dataNascimento));
    }

    private static void ValidarEmail(string email)
    {
        if (!email.Contains('@') || !email.Contains('.'))
            throw new ArgumentException("Email deve ter formato válido.", nameof(email));

        if (email.Length > 200)
            throw new ArgumentException("Email deve ter no máximo 200 caracteres.", nameof(email));
    }
}