using System;
using System.ComponentModel.DataAnnotations;

namespace Alunos.Application.DTOs
{
    /// <summary>
    /// DTO para geração de certificado
    /// </summary>
    public class CertificadoGerarDto
    {
        /// <summary>
        /// ID da matrícula
        /// </summary>
        [Required(ErrorMessage = "ID da matrícula é obrigatório")]
        public Guid MatriculaId { get; set; }

        /// <summary>
        /// Nome do curso
        /// </summary>
        [Required(ErrorMessage = "Nome do curso é obrigatório")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Nome do curso deve ter entre 2 e 200 caracteres")]
        public string NomeCurso { get; set; } = string.Empty;

        /// <summary>
        /// Nome do aluno
        /// </summary>
        [Required(ErrorMessage = "Nome do aluno é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome do aluno deve ter entre 2 e 100 caracteres")]
        public string NomeAluno { get; set; } = string.Empty;

        /// <summary>
        /// Carga horária do curso
        /// </summary>
        [Required(ErrorMessage = "Carga horária é obrigatória")]
        [Range(1, 10000, ErrorMessage = "Carga horária deve estar entre 1 e 10000 horas")]
        public int CargaHoraria { get; set; }

        /// <summary>
        /// Nota final obtida
        /// </summary>
        [Range(0, 10, ErrorMessage = "Nota deve estar entre 0 e 10")]
        public decimal? NotaFinal { get; set; }

        /// <summary>
        /// Nome do instrutor
        /// </summary>
        [StringLength(100, ErrorMessage = "Nome do instrutor deve ter no máximo 100 caracteres")]
        public string NomeInstrutor { get; set; } = string.Empty;

        /// <summary>
        /// Dias de validade do certificado
        /// </summary>
        [Range(1, 36500, ErrorMessage = "Dias de validade deve estar entre 1 e 36500")]
        public int ValidadeDias { get; set; } = 3650; // 10 anos por padrão
    }

    /// <summary>
    /// DTO de resposta para certificado
    /// </summary>
    public class CertificadoDto
    {
        /// <summary>
        /// ID do certificado
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ID da matrícula
        /// </summary>
        public Guid MatriculaId { get; set; }

        /// <summary>
        /// Código único do certificado
        /// </summary>
        public string Codigo { get; set; } = string.Empty;

        /// <summary>
        /// Nome do curso certificado
        /// </summary>
        public string NomeCurso { get; set; } = string.Empty;

        /// <summary>
        /// Nome do aluno certificado
        /// </summary>
        public string NomeAluno { get; set; } = string.Empty;

        /// <summary>
        /// Data de emissão do certificado
        /// </summary>
        public DateTime DataEmissao { get; set; }

        /// <summary>
        /// Data de validade do certificado
        /// </summary>
        public DateTime? DataValidade { get; set; }

        /// <summary>
        /// Carga horária do curso
        /// </summary>
        public int CargaHoraria { get; set; }

        /// <summary>
        /// Nota final obtida
        /// </summary>
        public decimal? NotaFinal { get; set; }

        /// <summary>
        /// Status do certificado
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// URL do arquivo do certificado
        /// </summary>
        public string UrlArquivo { get; set; } = string.Empty;

        /// <summary>
        /// Hash do certificado para validação
        /// </summary>
        public string HashValidacao { get; set; } = string.Empty;

        /// <summary>
        /// Observações sobre o certificado
        /// </summary>
        public string Observacoes { get; set; } = string.Empty;

        /// <summary>
        /// Nome do instrutor
        /// </summary>
        public string NomeInstrutor { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o certificado está válido
        /// </summary>
        public bool EstaValido { get; set; }

        /// <summary>
        /// Indica se o certificado está expirado
        /// </summary>
        public bool EstaExpirado { get; set; }

        /// <summary>
        /// Dias restantes até expiração
        /// </summary>
        public int DiasRestantesValidade { get; set; }

        /// <summary>
        /// Data de criação
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Data de atualização
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO resumido para listagem de certificados
    /// </summary>
    public class CertificadoResumoDto
    {
        /// <summary>
        /// ID do certificado
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Código único do certificado
        /// </summary>
        public string Codigo { get; set; } = string.Empty;

        /// <summary>
        /// Nome do curso certificado
        /// </summary>
        public string NomeCurso { get; set; } = string.Empty;

        /// <summary>
        /// Nome do aluno certificado
        /// </summary>
        public string NomeAluno { get; set; } = string.Empty;

        /// <summary>
        /// Data de emissão do certificado
        /// </summary>
        public DateTime DataEmissao { get; set; }

        /// <summary>
        /// Data de validade do certificado
        /// </summary>
        public DateTime? DataValidade { get; set; }

        /// <summary>
        /// Status do certificado
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o certificado está válido
        /// </summary>
        public bool EstaValido { get; set; }

        /// <summary>
        /// Dias restantes até expiração
        /// </summary>
        public int DiasRestantesValidade { get; set; }
    }

    /// <summary>
    /// DTO para validação de certificado
    /// </summary>
    public class CertificadoValidacaoDto
    {
        /// <summary>
        /// Código do certificado para validação
        /// </summary>
        [Required(ErrorMessage = "Código do certificado é obrigatório")]
        [StringLength(100, ErrorMessage = "Código deve ter no máximo 100 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        /// <summary>
        /// Hash para validação (opcional)
        /// </summary>
        [StringLength(500, ErrorMessage = "Hash deve ter no máximo 500 caracteres")]
        public string Hash { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para resultado da validação
    /// </summary>
    public class CertificadoValidacaoResultadoDto
    {
        /// <summary>
        /// Indica se o certificado é válido
        /// </summary>
        public bool EhValido { get; set; }

        /// <summary>
        /// Mensagem do resultado da validação
        /// </summary>
        public string Mensagem { get; set; } = string.Empty;

        /// <summary>
        /// Dados do certificado (se válido)
        /// </summary>
        public CertificadoDto? Certificado { get; set; }

        /// <summary>
        /// Data da validação
        /// </summary>
        public DateTime DataValidacao { get; set; }
    }

    /// <summary>
    /// DTO para atualização de certificado
    /// </summary>
    public class CertificadoAtualizarDto
    {
        /// <summary>
        /// URL do arquivo do certificado
        /// </summary>
        [StringLength(500, ErrorMessage = "URL do arquivo deve ter no máximo 500 caracteres")]
        public string UrlArquivo { get; set; } = string.Empty;

        /// <summary>
        /// Observações sobre o certificado
        /// </summary>
        [StringLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres")]
        public string Observacoes { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para revogação de certificado
    /// </summary>
    public class CertificadoRevogacaoDto
    {
        /// <summary>
        /// Motivo da revogação
        /// </summary>
        [Required(ErrorMessage = "Motivo da revogação é obrigatório")]
        [StringLength(200, ErrorMessage = "Motivo deve ter no máximo 200 caracteres")]
        public string Motivo { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para suspensão de certificado
    /// </summary>
    public class CertificadoSuspensaoDto
    {
        /// <summary>
        /// Motivo da suspensão
        /// </summary>
        [Required(ErrorMessage = "Motivo da suspensão é obrigatório")]
        [StringLength(200, ErrorMessage = "Motivo deve ter no máximo 200 caracteres")]
        public string Motivo { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para renovação de certificado
    /// </summary>
    public class CertificadoRenovacaoDto
    {
        /// <summary>
        /// Novos dias de validade
        /// </summary>
        [Required(ErrorMessage = "Dias de validade são obrigatórios")]
        [Range(1, 36500, ErrorMessage = "Dias de validade deve estar entre 1 e 36500")]
        public int NovosDias { get; set; }
    }
} 