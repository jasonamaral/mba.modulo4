namespace Alunos.Application.DTOs
{
    /// <summary>
    /// DTO para próxima aula
    /// </summary>
    public class ProximaAulaDto
    {
        /// <summary>
        /// ID da aula
        /// </summary>
        public Guid AulaId { get; set; }

        /// <summary>
        /// Nome da aula
        /// </summary>
        public string NomeAula { get; set; } = string.Empty;

        /// <summary>
        /// Nome do curso
        /// </summary>
        public string NomeCurso { get; set; } = string.Empty;

        /// <summary>
        /// Duração em minutos
        /// </summary>
        public int DuracaoMinutos { get; set; }

        /// <summary>
        /// Percentual assistido
        /// </summary>
        public decimal PercentualAssistido { get; set; }

        /// <summary>
        /// Último acesso
        /// </summary>
        public DateTime? UltimoAcesso { get; set; }
    }
} 