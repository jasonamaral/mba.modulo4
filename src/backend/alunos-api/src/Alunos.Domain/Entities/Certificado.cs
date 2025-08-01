using Alunos.Domain.Common;

namespace Alunos.Domain.Entities;

public class Certificado : Entidade
{
    public Guid MatriculaId { get; private set; }

    public MatriculaCurso Matricula { get; private set; }

    public string Codigo { get; private set; }

    public string NomeCurso { get; private set; }

    public string NomeAluno { get; private set; }

    public DateTime DataEmissao { get; private set; }

    public DateTime? DataValidade { get; private set; }

    public int CargaHoraria { get; private set; }

    public decimal? NotaFinal { get; private set; }

    public StatusCertificadoEnum Status { get; private set; }

    public string UrlArquivo { get; private set; }

    public string HashValidacao { get; private set; }

    public string Observacoes { get; private set; }

    public string NomeInstrutor { get; private set; }

    protected Certificado() : base()
    {
        Codigo = string.Empty;
        NomeCurso = string.Empty;
        NomeAluno = string.Empty;
        UrlArquivo = string.Empty;
        HashValidacao = string.Empty;
        Observacoes = string.Empty;
        NomeInstrutor = string.Empty;
        Matricula = null!;
    }

    public Certificado(
        Guid matriculaId,
        string nomeCurso,
        string nomeAluno,
        int cargaHoraria,
        decimal? notaFinal = null,
        string nomeInstrutor = "",
        int validadeDias = 3650) : base()
    {
        ValidarDadosObrigatorios(matriculaId, nomeCurso, nomeAluno, cargaHoraria);

        if (notaFinal.HasValue)
        {
            ValidarNota(notaFinal.Value);
        }

        MatriculaId = matriculaId;
        NomeCurso = nomeCurso.Trim();
        NomeAluno = nomeAluno.Trim();
        CargaHoraria = cargaHoraria;
        NotaFinal = notaFinal;
        NomeInstrutor = nomeInstrutor?.Trim() ?? string.Empty;
        DataEmissao = DateTime.UtcNow;
        DataValidade = validadeDias > 0 ? DateTime.UtcNow.AddDays(validadeDias) : null;
        Status = StatusCertificadoEnum.Ativo;
        Codigo = GerarCodigo();
        HashValidacao = GerarHashValidacao();
        UrlArquivo = string.Empty;
        Observacoes = string.Empty;
    }

    public void AtualizarUrlArquivo(string urlArquivo)
    {
        if (string.IsNullOrWhiteSpace(urlArquivo))
            throw new ArgumentException("URL do arquivo é obrigatória.", nameof(urlArquivo));

        UrlArquivo = urlArquivo.Trim();
        SetUpdatedAt();
    }

    public void AtualizarObservacoes(string observacoes)
    {
        Observacoes = observacoes?.Trim() ?? string.Empty;
        SetUpdatedAt();
    }

    public void Revogar(string motivo = "")
    {
        if (Status == StatusCertificadoEnum.Revogado)
            throw new InvalidOperationException("Certificado já está revogado.");

        Status = StatusCertificadoEnum.Revogado;

        if (!string.IsNullOrWhiteSpace(motivo))
        {
            Observacoes = string.IsNullOrWhiteSpace(Observacoes)
                ? $"Revogado: {motivo.Trim()}"
                : $"{Observacoes} | Revogado: {motivo.Trim()}";
        }

        SetUpdatedAt();
    }

    public void Suspender(string motivo = "")
    {
        if (Status == StatusCertificadoEnum.Revogado)
            throw new InvalidOperationException("Não é possível suspender um certificado revogado.");

        if (Status == StatusCertificadoEnum.Suspenso)
            throw new InvalidOperationException("Certificado já está suspenso.");

        Status = StatusCertificadoEnum.Suspenso;

        if (!string.IsNullOrWhiteSpace(motivo))
        {
            Observacoes = string.IsNullOrWhiteSpace(Observacoes)
                ? $"Suspenso: {motivo.Trim()}"
                : $"{Observacoes} | Suspenso: {motivo.Trim()}";
        }

        SetUpdatedAt();
    }

    public void Reativar()
    {
        if (Status == StatusCertificadoEnum.Revogado)
            throw new InvalidOperationException("Não é possível reativar um certificado revogado.");

        if (Status == StatusCertificadoEnum.Ativo)
            throw new InvalidOperationException("Certificado já está ativo.");

        Status = StatusCertificadoEnum.Ativo;
        SetUpdatedAt();
    }

    public void Renovar(int novosDias)
    {
        if (Status == StatusCertificadoEnum.Revogado)
            throw new InvalidOperationException("Não é possível renovar um certificado revogado.");

        if (novosDias <= 0)
            throw new ArgumentException("Dias de validade deve ser maior que zero.", nameof(novosDias));

        DataValidade = DateTime.UtcNow.AddDays(novosDias);

        if (Status == StatusCertificadoEnum.Expirado)
        {
            Status = StatusCertificadoEnum.Ativo;
        }

        SetUpdatedAt();
    }

    public bool EstaValido()
    {
        if (Status != StatusCertificadoEnum.Ativo)
            return false;

        if (!DataValidade.HasValue)
            return true;

        return DateTime.UtcNow <= DataValidade.Value;
    }

    public bool EstaExpirado()
    {
        if (!DataValidade.HasValue)
            return false;

        return DateTime.UtcNow > DataValidade.Value;
    }

    public int DiasRestantesValidade()
    {
        if (!DataValidade.HasValue)
            return int.MaxValue;

        return (DataValidade.Value.Date - DateTime.UtcNow.Date).Days;
    }

    public bool ValidarHash(string hash)
    {
        return !string.IsNullOrWhiteSpace(hash) && hash.Equals(HashValidacao, StringComparison.OrdinalIgnoreCase);
    }

    private string GerarCodigo()
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var random = new Random().Next(1000, 9999);
        return $"CERT-{timestamp}-{random}";
    }

    private string GerarHashValidacao()
    {
        var content = $"{Id}|{MatriculaId}|{NomeCurso}|{NomeAluno}|{DataEmissao:yyyy-MM-dd}";
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(content));
    }

    private static void ValidarDadosObrigatorios(Guid matriculaId, string nomeCurso, string nomeAluno, int cargaHoraria)
    {
        if (matriculaId == Guid.Empty)
            throw new ArgumentException("ID da matrícula é obrigatório.", nameof(matriculaId));

        if (string.IsNullOrWhiteSpace(nomeCurso))
            throw new ArgumentException("Nome do curso é obrigatório.", nameof(nomeCurso));

        if (string.IsNullOrWhiteSpace(nomeAluno))
            throw new ArgumentException("Nome do aluno é obrigatório.", nameof(nomeAluno));

        if (nomeCurso.Length < 2)
            throw new ArgumentException("Nome do curso deve ter pelo menos 2 caracteres.", nameof(nomeCurso));

        if (nomeAluno.Length < 2)
            throw new ArgumentException("Nome do aluno deve ter pelo menos 2 caracteres.", nameof(nomeAluno));

        if (cargaHoraria <= 0)
            throw new ArgumentException("Carga horária deve ser maior que zero.", nameof(cargaHoraria));

        if (cargaHoraria > 10000)
            throw new ArgumentException("Carga horária muito alta.", nameof(cargaHoraria));
    }

    private static void ValidarNota(decimal nota)
    {
        if (nota < 0 || nota > 10)
            throw new ArgumentException("Nota deve estar entre 0 e 10.", nameof(nota));
    }
}
