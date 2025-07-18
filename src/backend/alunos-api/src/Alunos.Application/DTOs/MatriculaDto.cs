using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alunos.Application.DTOs
{
    /// <summary>
    /// DTO para cadastro de matrícula
    /// </summary>
    public class MatriculaCadastroDto
    {
        /// <summary>
        /// ID do curso
        /// </summary>
        [Required(ErrorMessage = "ID do curso é obrigatório")]
        public Guid CursoId { get; set; }

        /// <summary>
        /// Data de início do curso
        /// </summary>
        [Required(ErrorMessage = "Data de início é obrigatória")]
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Valor pago pela matrícula
        /// </summary>
        [Required(ErrorMessage = "Valor pago é obrigatório")]
        [Range(0, 999999.99, ErrorMessage = "Valor deve estar entre 0 e 999.999,99")]
        public decimal ValorPago { get; set; }

        /// <summary>
        /// Forma de pagamento
        /// </summary>
        [StringLength(50, ErrorMessage = "Forma de pagamento deve ter no máximo 50 caracteres")]
        public string FormaPagamento { get; set; } = string.Empty;

        /// <summary>
        /// Observações sobre a matrícula
        /// </summary>
        [StringLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres")]
        public string Observacoes { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para atualização de matrícula
    /// </summary>
    public class MatriculaAtualizarDto
    {
        /// <summary>
        /// Data de início do curso
        /// </summary>
        [Required(ErrorMessage = "Data de início é obrigatória")]
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Valor pago pela matrícula
        /// </summary>
        [Required(ErrorMessage = "Valor pago é obrigatório")]
        [Range(0, 999999.99, ErrorMessage = "Valor deve estar entre 0 e 999.999,99")]
        public decimal ValorPago { get; set; }

        /// <summary>
        /// Forma de pagamento
        /// </summary>
        [StringLength(50, ErrorMessage = "Forma de pagamento deve ter no máximo 50 caracteres")]
        public string FormaPagamento { get; set; } = string.Empty;

        /// <summary>
        /// Observações sobre a matrícula
        /// </summary>
        [StringLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres")]
        public string Observacoes { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO de resposta para matrícula
    /// </summary>
    public class MatriculaDto
    {
        /// <summary>
        /// ID da matrícula
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ID do aluno
        /// </summary>
        public Guid AlunoId { get; set; }

        /// <summary>
        /// Nome do aluno
        /// </summary>
        public string NomeAluno { get; set; } = string.Empty;

        /// <summary>
        /// ID do curso
        /// </summary>
        public Guid CursoId { get; set; }

        /// <summary>
        /// Nome do curso
        /// </summary>
        public string NomeCurso { get; set; } = string.Empty;

        /// <summary>
        /// Data da matrícula
        /// </summary>
        public DateTime DataMatricula { get; set; }

        /// <summary>
        /// Data de início do curso
        /// </summary>
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data de término do curso
        /// </summary>
        public DateTime? DataTermino { get; set; }

        /// <summary>
        /// Status da matrícula
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Valor pago pela matrícula
        /// </summary>
        public decimal ValorPago { get; set; }

        /// <summary>
        /// Forma de pagamento
        /// </summary>
        public string FormaPagamento { get; set; } = string.Empty;

        /// <summary>
        /// Percentual de conclusão do curso
        /// </summary>
        public decimal PercentualConclusao { get; set; }

        /// <summary>
        /// Nota final do aluno
        /// </summary>
        public decimal? NotaFinal { get; set; }

        /// <summary>
        /// Observações sobre a matrícula
        /// </summary>
        public string Observacoes { get; set; } = string.Empty;

        /// <summary>
        /// Indica se a matrícula está ativa
        /// </summary>
        public bool IsAtiva { get; set; }

        /// <summary>
        /// Duração do curso em dias
        /// </summary>
        public int DuracaoCursoDias { get; set; }

        /// <summary>
        /// Indica se a matrícula está vencida
        /// </summary>
        public bool EstaVencida { get; set; }

        /// <summary>
        /// Data de criação
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Data de atualização
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Lista de progresso nas aulas
        /// </summary>
        public List<ProgressoDto> Progresso { get; set; } = new();

        /// <summary>
        /// Lista de certificados
        /// </summary>
        public List<CertificadoDto> Certificados { get; set; } = new();
    }

    /// <summary>
    /// DTO resumido para listagem de matrículas
    /// </summary>
    public class MatriculaResumoDto
    {
        /// <summary>
        /// ID da matrícula
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nome do aluno
        /// </summary>
        public string NomeAluno { get; set; } = string.Empty;

        /// <summary>
        /// Nome do curso
        /// </summary>
        public string NomeCurso { get; set; } = string.Empty;

        /// <summary>
        /// Data da matrícula
        /// </summary>
        public DateTime DataMatricula { get; set; }

        /// <summary>
        /// Status da matrícula
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Percentual de conclusão do curso
        /// </summary>
        public decimal PercentualConclusao { get; set; }

        /// <summary>
        /// Valor pago pela matrícula
        /// </summary>
        public decimal ValorPago { get; set; }

        /// <summary>
        /// Indica se a matrícula está ativa
        /// </summary>
        public bool IsAtiva { get; set; }

        /// <summary>
        /// Indica se a matrícula está vencida
        /// </summary>
        public bool EstaVencida { get; set; }
    }

    /// <summary>
    /// DTO para conclusão de curso
    /// </summary>
    public class MatriculaConclusaoDto
    {
        /// <summary>
        /// Nota final do aluno
        /// </summary>
        [Range(0, 10, ErrorMessage = "Nota deve estar entre 0 e 10")]
        public decimal? NotaFinal { get; set; }

        /// <summary>
        /// Observações sobre a conclusão
        /// </summary>
        [StringLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres")]
        public string Observacoes { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para cancelamento de matrícula
    /// </summary>
    public class MatriculaCancelamentoDto
    {
        /// <summary>
        /// Motivo do cancelamento
        /// </summary>
        [Required(ErrorMessage = "Motivo do cancelamento é obrigatório")]
        [StringLength(200, ErrorMessage = "Motivo deve ter no máximo 200 caracteres")]
        public string Motivo { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para suspensão de matrícula
    /// </summary>
    public class MatriculaSuspensaoDto
    {
        /// <summary>
        /// Motivo da suspensão
        /// </summary>
        [Required(ErrorMessage = "Motivo da suspensão é obrigatório")]
        [StringLength(200, ErrorMessage = "Motivo deve ter no máximo 200 caracteres")]
        public string Motivo { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para relatório de matrícula
    /// </summary>
    public class MatriculaRelatorioDto
    {
        /// <summary>
        /// Dados da matrícula
        /// </summary>
        public MatriculaDto Matricula { get; set; } = new();

        /// <summary>
        /// Informações do curso
        /// </summary>
        public CursoInfoDto CursoInfo { get; set; } = new();

        /// <summary>
        /// Estatísticas de progresso
        /// </summary>
        public MatriculaEstatisticasDto Estatisticas { get; set; } = new();

        /// <summary>
        /// Histórico de atividades
        /// </summary>
        public List<HistoricoAlunoDto> HistoricoAtividades { get; set; } = new();
    }

    /// <summary>
    /// DTO para estatísticas de matrícula
    /// </summary>
    public class MatriculaEstatisticasDto
    {
        /// <summary>
        /// Total de aulas
        /// </summary>
        public int TotalAulas { get; set; }

        /// <summary>
        /// Aulas assistidas
        /// </summary>
        public int AulasAssistidas { get; set; }

        /// <summary>
        /// Aulas concluídas
        /// </summary>
        public int AulasConcluidas { get; set; }

        /// <summary>
        /// Tempo total de estudo em horas
        /// </summary>
        public decimal TempoEstudoHoras { get; set; }

        /// <summary>
        /// Média de tempo por aula em minutos
        /// </summary>
        public decimal MediaTempoPorAula { get; set; }

        /// <summary>
        /// Percentual de conclusão
        /// </summary>
        public decimal PercentualConclusao { get; set; }

        /// <summary>
        /// Primeira aula assistida
        /// </summary>
        public DateTime? PrimeiraAulaAssistida { get; set; }

        /// <summary>
        /// Última atividade
        /// </summary>
        public DateTime? UltimaAtividade { get; set; }
    }

    /// <summary>
    /// DTO para informações do curso
    /// </summary>
    public class CursoInfoDto
    {
        /// <summary>
        /// ID do curso
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nome do curso
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Descrição do curso
        /// </summary>
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Carga horária total
        /// </summary>
        public int CargaHoraria { get; set; }

        /// <summary>
        /// Nome do instrutor
        /// </summary>
        public string Instrutor { get; set; } = string.Empty;

        /// <summary>
        /// Categoria do curso
        /// </summary>
        public string Categoria { get; set; } = string.Empty;

        /// <summary>
        /// Nível do curso
        /// </summary>
        public string Nivel { get; set; } = string.Empty;

        /// <summary>
        /// URL da imagem do curso
        /// </summary>
        public string ImagemUrl { get; set; } = string.Empty;
    }
} 