using System;
using Alunos.Domain.Common;

namespace Alunos.Domain.Entities
{
    /// <summary>
    /// Entidade Certificado - Representa um certificado emitido para um aluno
    /// </summary>
    public class Certificado : Entidade
    {
        /// <summary>
        /// ID da matrícula
        /// </summary>
        public Guid MatriculaId { get; private set; }

        /// <summary>
        /// Referência de navegação para a matrícula
        /// </summary>
        public MatriculaCurso Matricula { get; private set; }

        /// <summary>
        /// Código único do certificado
        /// </summary>
        public string Codigo { get; private set; }

        /// <summary>
        /// Nome do curso certificado
        /// </summary>
        public string NomeCurso { get; private set; }

        /// <summary>
        /// Nome do aluno certificado
        /// </summary>
        public string NomeAluno { get; private set; }

        /// <summary>
        /// Data de emissão do certificado
        /// </summary>
        public DateTime DataEmissao { get; private set; }

        /// <summary>
        /// Data de validade do certificado
        /// </summary>
        public DateTime? DataValidade { get; private set; }

        /// <summary>
        /// Carga horária do curso
        /// </summary>
        public int CargaHoraria { get; private set; }

        /// <summary>
        /// Nota final obtida
        /// </summary>
        public decimal? NotaFinal { get; private set; }

        /// <summary>
        /// Status do certificado
        /// </summary>
        public StatusCertificado Status { get; private set; }

        /// <summary>
        /// URL do arquivo do certificado
        /// </summary>
        public string UrlArquivo { get; private set; }

        /// <summary>
        /// Hash do certificado para validação
        /// </summary>
        public string HashValidacao { get; private set; }

        /// <summary>
        /// Observações sobre o certificado
        /// </summary>
        public string Observacoes { get; private set; }

        /// <summary>
        /// Nome do instrutor
        /// </summary>
        public string NomeInstrutor { get; private set; }

        /// <summary>
        /// Construtor protegido para Entity Framework
        /// </summary>
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

        /// <summary>
        /// Construtor principal para criação de novo certificado
        /// </summary>
        /// <param name="matriculaId">ID da matrícula</param>
        /// <param name="nomeCurso">Nome do curso</param>
        /// <param name="nomeAluno">Nome do aluno</param>
        /// <param name="cargaHoraria">Carga horária do curso</param>
        /// <param name="notaFinal">Nota final obtida</param>
        /// <param name="nomeInstrutor">Nome do instrutor</param>
        /// <param name="validadeDias">Dias de validade do certificado</param>
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
            Status = StatusCertificado.Ativo;
            Codigo = GerarCodigo();
            HashValidacao = GerarHashValidacao();
            UrlArquivo = string.Empty;
            Observacoes = string.Empty;
        }

        /// <summary>
        /// Atualiza a URL do arquivo do certificado
        /// </summary>
        /// <param name="urlArquivo">URL do arquivo</param>
        public void AtualizarUrlArquivo(string urlArquivo)
        {
            if (string.IsNullOrWhiteSpace(urlArquivo))
                throw new ArgumentException("URL do arquivo é obrigatória.", nameof(urlArquivo));

            UrlArquivo = urlArquivo.Trim();
            SetUpdatedAt();
        }

        /// <summary>
        /// Atualiza observações do certificado
        /// </summary>
        /// <param name="observacoes">Novas observações</param>
        public void AtualizarObservacoes(string observacoes)
        {
            Observacoes = observacoes?.Trim() ?? string.Empty;
            SetUpdatedAt();
        }

        /// <summary>
        /// Revoga o certificado
        /// </summary>
        /// <param name="motivo">Motivo da revogação</param>
        public void Revogar(string motivo = "")
        {
            if (Status == StatusCertificado.Revogado)
                throw new InvalidOperationException("Certificado já está revogado.");

            Status = StatusCertificado.Revogado;
            
            if (!string.IsNullOrWhiteSpace(motivo))
            {
                Observacoes = string.IsNullOrWhiteSpace(Observacoes) 
                    ? $"Revogado: {motivo.Trim()}" 
                    : $"{Observacoes} | Revogado: {motivo.Trim()}";
            }

            SetUpdatedAt();
        }

        /// <summary>
        /// Suspende o certificado
        /// </summary>
        /// <param name="motivo">Motivo da suspensão</param>
        public void Suspender(string motivo = "")
        {
            if (Status == StatusCertificado.Revogado)
                throw new InvalidOperationException("Não é possível suspender um certificado revogado.");

            if (Status == StatusCertificado.Suspenso)
                throw new InvalidOperationException("Certificado já está suspenso.");

            Status = StatusCertificado.Suspenso;
            
            if (!string.IsNullOrWhiteSpace(motivo))
            {
                Observacoes = string.IsNullOrWhiteSpace(Observacoes) 
                    ? $"Suspenso: {motivo.Trim()}" 
                    : $"{Observacoes} | Suspenso: {motivo.Trim()}";
            }

            SetUpdatedAt();
        }

        /// <summary>
        /// Reativa o certificado
        /// </summary>
        public void Reativar()
        {
            if (Status == StatusCertificado.Revogado)
                throw new InvalidOperationException("Não é possível reativar um certificado revogado.");

            if (Status == StatusCertificado.Ativo)
                throw new InvalidOperationException("Certificado já está ativo.");

            Status = StatusCertificado.Ativo;
            SetUpdatedAt();
        }

        /// <summary>
        /// Renova o certificado
        /// </summary>
        /// <param name="novosDias">Novos dias de validade</param>
        public void Renovar(int novosDias)
        {
            if (Status == StatusCertificado.Revogado)
                throw new InvalidOperationException("Não é possível renovar um certificado revogado.");

            if (novosDias <= 0)
                throw new ArgumentException("Dias de validade deve ser maior que zero.", nameof(novosDias));

            DataValidade = DateTime.UtcNow.AddDays(novosDias);
            
            if (Status == StatusCertificado.Expirado)
            {
                Status = StatusCertificado.Ativo;
            }

            SetUpdatedAt();
        }

        /// <summary>
        /// Verifica se o certificado está válido
        /// </summary>
        /// <returns>True se válido</returns>
        public bool EstaValido()
        {
            if (Status != StatusCertificado.Ativo)
                return false;

            if (!DataValidade.HasValue)
                return true;

            return DateTime.UtcNow <= DataValidade.Value;
        }

        /// <summary>
        /// Verifica se o certificado está expirado
        /// </summary>
        /// <returns>True se expirado</returns>
        public bool EstaExpirado()
        {
            if (!DataValidade.HasValue)
                return false;

            return DateTime.UtcNow > DataValidade.Value;
        }

        /// <summary>
        /// Calcula dias restantes até expiração
        /// </summary>
        /// <returns>Dias restantes (negativo se expirado)</returns>
        public int DiasRestantesValidade()
        {
            if (!DataValidade.HasValue)
                return int.MaxValue;

            return (DataValidade.Value.Date - DateTime.UtcNow.Date).Days;
        }

        /// <summary>
        /// Valida o hash do certificado
        /// </summary>
        /// <param name="hash">Hash a ser validado</param>
        /// <returns>True se hash válido</returns>
        public bool ValidarHash(string hash)
        {
            return !string.IsNullOrWhiteSpace(hash) && hash.Equals(HashValidacao, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gera código único do certificado
        /// </summary>
        /// <returns>Código gerado</returns>
        private string GerarCodigo()
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var random = new Random().Next(1000, 9999);
            return $"CERT-{timestamp}-{random}";
        }

        /// <summary>
        /// Gera hash de validação do certificado
        /// </summary>
        /// <returns>Hash gerado</returns>
        private string GerarHashValidacao()
        {
            var content = $"{Id}|{MatriculaId}|{NomeCurso}|{NomeAluno}|{DataEmissao:yyyy-MM-dd}";
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(content));
        }

        /// <summary>
        /// Valida dados obrigatórios
        /// </summary>
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

        /// <summary>
        /// Valida nota
        /// </summary>
        private static void ValidarNota(decimal nota)
        {
            if (nota < 0 || nota > 10)
                throw new ArgumentException("Nota deve estar entre 0 e 10.", nameof(nota));
        }
    }

    /// <summary>
    /// Enum para status do certificado
    /// </summary>
    public enum StatusCertificado
    {
        /// <summary>
        /// Certificado ativo e válido
        /// </summary>
        Ativo = 1,

        /// <summary>
        /// Certificado suspenso temporariamente
        /// </summary>
        Suspenso = 2,

        /// <summary>
        /// Certificado revogado permanentemente
        /// </summary>
        Revogado = 3,

        /// <summary>
        /// Certificado expirado
        /// </summary>
        Expirado = 4
    }
} 