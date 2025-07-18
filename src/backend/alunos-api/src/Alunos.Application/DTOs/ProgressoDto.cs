using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic; // Added for List

namespace Alunos.Application.DTOs
{
    /// <summary>
    /// DTO para atualização de progresso
    /// </summary>
    public class ProgressoAtualizarDto
    {
        /// <summary>
        /// Percentual assistido da aula (0-100)
        /// </summary>
        [Required(ErrorMessage = "Percentual assistido é obrigatório")]
        [Range(0, 100, ErrorMessage = "Percentual deve estar entre 0 e 100")]
        public decimal PercentualAssistido { get; set; }

        /// <summary>
        /// Tempo assistido em segundos
        /// </summary>
        [Required(ErrorMessage = "Tempo assistido é obrigatório")]
        [Range(0, 86400, ErrorMessage = "Tempo assistido deve estar entre 0 e 86400 segundos")]
        public int TempoAssistido { get; set; }

        /// <summary>
        /// Observações sobre o progresso
        /// </summary>
        [StringLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres")]
        public string Observacoes { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para conclusão de aula
    /// </summary>
    public class ProgressoConclusaoDto
    {
        /// <summary>
        /// Nota obtida na aula (opcional)
        /// </summary>
        [Range(0, 10, ErrorMessage = "Nota deve estar entre 0 e 10")]
        public decimal? Nota { get; set; }

        /// <summary>
        /// Observações sobre a conclusão
        /// </summary>
        [StringLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres")]
        public string Observacoes { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO de resposta para progresso
    /// </summary>
    public class ProgressoDto
    {
        /// <summary>
        /// ID do progresso
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ID da matrícula
        /// </summary>
        public Guid MatriculaId { get; set; }

        /// <summary>
        /// ID da aula
        /// </summary>
        public Guid AulaId { get; set; }

        /// <summary>
        /// Nome da aula
        /// </summary>
        public string NomeAula { get; set; } = string.Empty;

        /// <summary>
        /// Status do progresso na aula
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Percentual assistido da aula (0-100)
        /// </summary>
        public decimal PercentualAssistido { get; set; }

        /// <summary>
        /// Tempo assistido em segundos
        /// </summary>
        public int TempoAssistido { get; set; }

        /// <summary>
        /// Tempo assistido em minutos
        /// </summary>
        public int TempoAssistidoMinutos { get; set; }

        /// <summary>
        /// Tempo assistido em horas
        /// </summary>
        public decimal TempoAssistidoHoras { get; set; }

        /// <summary>
        /// Data de início da aula
        /// </summary>
        public DateTime? DataInicio { get; set; }

        /// <summary>
        /// Data de conclusão da aula
        /// </summary>
        public DateTime? DataConclusao { get; set; }

        /// <summary>
        /// Data do último acesso à aula
        /// </summary>
        public DateTime? UltimoAcesso { get; set; }

        /// <summary>
        /// Nota obtida na aula (se aplicável)
        /// </summary>
        public decimal? Nota { get; set; }

        /// <summary>
        /// Observações sobre o progresso
        /// </summary>
        public string Observacoes { get; set; } = string.Empty;

        /// <summary>
        /// Indica se a aula está abandonada
        /// </summary>
        public bool EstaAbandonada { get; set; }

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
    /// DTO resumido para listagem de progresso
    /// </summary>
    public class ProgressoResumoDto
    {
        /// <summary>
        /// ID do progresso
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ID da aula
        /// </summary>
        public Guid AulaId { get; set; }

        /// <summary>
        /// Nome da aula
        /// </summary>
        public string NomeAula { get; set; } = string.Empty;

        /// <summary>
        /// Status do progresso na aula
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Percentual assistido da aula (0-100)
        /// </summary>
        public decimal PercentualAssistido { get; set; }

        /// <summary>
        /// Tempo assistido em minutos
        /// </summary>
        public int TempoAssistidoMinutos { get; set; }

        /// <summary>
        /// Data do último acesso à aula
        /// </summary>
        public DateTime? UltimoAcesso { get; set; }

        /// <summary>
        /// Nota obtida na aula (se aplicável)
        /// </summary>
        public decimal? Nota { get; set; }

        /// <summary>
        /// Indica se a aula está abandonada
        /// </summary>
        public bool EstaAbandonada { get; set; }
    }

    /// <summary>
    /// DTO para relatório de progresso
    /// </summary>
    public class ProgressoRelatorioDto
    {
        /// <summary>
        /// ID da matrícula
        /// </summary>
        public Guid MatriculaId { get; set; }

        /// <summary>
        /// Nome do curso
        /// </summary>
        public string NomeCurso { get; set; } = string.Empty;

        /// <summary>
        /// Nome do aluno
        /// </summary>
        public string NomeAluno { get; set; } = string.Empty;

        /// <summary>
        /// Total de aulas
        /// </summary>
        public int TotalAulas { get; set; }

        /// <summary>
        /// Aulas iniciadas
        /// </summary>
        public int AulasIniciadas { get; set; }

        /// <summary>
        /// Aulas concluídas
        /// </summary>
        public int AulasConcluidas { get; set; }

        /// <summary>
        /// Percentual de conclusão geral
        /// </summary>
        public decimal PercentualConclusaoGeral { get; set; }

        /// <summary>
        /// Tempo total de estudo em horas
        /// </summary>
        public decimal TempoTotalEstudoHoras { get; set; }

        /// <summary>
        /// Média de tempo por aula em minutos
        /// </summary>
        public decimal MediaTempoPorAula { get; set; }

        /// <summary>
        /// Nota média
        /// </summary>
        public decimal? NotaMedia { get; set; }

        /// <summary>
        /// Data da primeira aula assistida
        /// </summary>
        public DateTime? PrimeiraAulaAssistida { get; set; }

        /// <summary>
        /// Data da última atividade
        /// </summary>
        public DateTime? UltimaAtividade { get; set; }

        /// <summary>
        /// Lista detalhada de progresso por aula
        /// </summary>
        public List<ProgressoDto> ProgressoPorAula { get; set; } = new();
    }
} 