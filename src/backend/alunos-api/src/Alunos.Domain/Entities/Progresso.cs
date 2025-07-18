using System;
using Alunos.Domain.Common;

namespace Alunos.Domain.Entities
{
    /// <summary>
    /// Entidade Progresso - Representa o progresso de um aluno em uma aula
    /// </summary>
    public class Progresso : Entidade
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
        /// ID da aula
        /// </summary>
        public Guid AulaId { get; private set; }

        /// <summary>
        /// Status do progresso na aula
        /// </summary>
        public StatusProgresso Status { get; private set; }

        /// <summary>
        /// Percentual assistido da aula (0-100)
        /// </summary>
        public decimal PercentualAssistido { get; private set; }

        /// <summary>
        /// Tempo assistido em segundos
        /// </summary>
        public int TempoAssistido { get; private set; }

        /// <summary>
        /// Data de início da aula
        /// </summary>
        public DateTime? DataInicio { get; private set; }

        /// <summary>
        /// Data de conclusão da aula
        /// </summary>
        public DateTime? DataConclusao { get; private set; }

        /// <summary>
        /// Data do último acesso à aula
        /// </summary>
        public DateTime? UltimoAcesso { get; private set; }

        /// <summary>
        /// Nota obtida na aula (se aplicável)
        /// </summary>
        public decimal? Nota { get; private set; }

        /// <summary>
        /// Observações sobre o progresso
        /// </summary>
        public string Observacoes { get; private set; }

        /// <summary>
        /// Construtor protegido para Entity Framework
        /// </summary>
        protected Progresso() : base()
        {
            Observacoes = string.Empty;
            Matricula = null!;
        }

        /// <summary>
        /// Construtor principal para criação de novo progresso
        /// </summary>
        /// <param name="matriculaId">ID da matrícula</param>
        /// <param name="aulaId">ID da aula</param>
        public Progresso(Guid matriculaId, Guid aulaId) : base()
        {
            ValidarDadosObrigatorios(matriculaId, aulaId);

            MatriculaId = matriculaId;
            AulaId = aulaId;
            Status = StatusProgresso.NaoIniciado;
            PercentualAssistido = 0;
            TempoAssistido = 0;
            Observacoes = string.Empty;
        }

        /// <summary>
        /// Inicia o progresso na aula
        /// </summary>
        public void IniciarAula()
        {
            if (Status == StatusProgresso.Concluido)
                throw new InvalidOperationException("Aula já foi concluída.");

            Status = StatusProgresso.EmAndamento;
            DataInicio = DateTime.UtcNow;
            UltimoAcesso = DateTime.UtcNow;
            SetUpdatedAt();
        }

        /// <summary>
        /// Atualiza o progresso da aula
        /// </summary>
        /// <param name="percentualAssistido">Percentual assistido (0-100)</param>
        /// <param name="tempoAssistido">Tempo assistido em segundos</param>
        public void AtualizarProgresso(decimal percentualAssistido, int tempoAssistido)
        {
            ValidarPercentual(percentualAssistido);
            ValidarTempo(tempoAssistido);

            if (Status == StatusProgresso.NaoIniciado)
            {
                IniciarAula();
            }

            PercentualAssistido = percentualAssistido;
            TempoAssistido = tempoAssistido;
            UltimoAcesso = DateTime.UtcNow;

            // Se assistiu 100%, marca como concluído
            if (percentualAssistido >= 100 && Status != StatusProgresso.Concluido)
            {
                ConcluirAula();
            }

            SetUpdatedAt();
        }

        /// <summary>
        /// Conclui a aula
        /// </summary>
        /// <param name="nota">Nota obtida (opcional)</param>
        public void ConcluirAula(decimal? nota = null)
        {
            if (Status == StatusProgresso.NaoIniciado)
                throw new InvalidOperationException("Não é possível concluir uma aula não iniciada.");

            if (nota.HasValue)
            {
                ValidarNota(nota.Value);
                Nota = nota.Value;
            }

            Status = StatusProgresso.Concluido;
            DataConclusao = DateTime.UtcNow;
            UltimoAcesso = DateTime.UtcNow;
            PercentualAssistido = 100;
            SetUpdatedAt();
        }

        /// <summary>
        /// Reinicia o progresso da aula
        /// </summary>
        public void ReiniciarAula()
        {
            Status = StatusProgresso.NaoIniciado;
            PercentualAssistido = 0;
            TempoAssistido = 0;
            DataInicio = null;
            DataConclusao = null;
            UltimoAcesso = null;
            Nota = null;
            SetUpdatedAt();
        }

        /// <summary>
        /// Atualiza observações
        /// </summary>
        /// <param name="observacoes">Novas observações</param>
        public void AtualizarObservacoes(string observacoes)
        {
            Observacoes = observacoes?.Trim() ?? string.Empty;
            SetUpdatedAt();
        }

        /// <summary>
        /// Atualiza último acesso
        /// </summary>
        public void AtualizarUltimoAcesso()
        {
            UltimoAcesso = DateTime.UtcNow;
            SetUpdatedAt();
        }

        /// <summary>
        /// Calcula tempo total em minutos
        /// </summary>
        /// <returns>Tempo em minutos</returns>
        public int CalcularTempoMinutos()
        {
            return TempoAssistido / 60;
        }

        /// <summary>
        /// Calcula tempo total em horas
        /// </summary>
        /// <returns>Tempo em horas</returns>
        public decimal CalcularTempoHoras()
        {
            return Math.Round((decimal)TempoAssistido / 3600, 2);
        }

        /// <summary>
        /// Verifica se a aula está em progresso há muito tempo
        /// </summary>
        /// <param name="horasLimite">Horas limite para considerar abandonada</param>
        /// <returns>True se abandonada</returns>
        public bool EstaAbandonada(int horasLimite = 24)
        {
            if (Status != StatusProgresso.EmAndamento || !UltimoAcesso.HasValue)
                return false;

            return DateTime.UtcNow > UltimoAcesso.Value.AddHours(horasLimite);
        }

        /// <summary>
        /// Valida dados obrigatórios
        /// </summary>
        private static void ValidarDadosObrigatorios(Guid matriculaId, Guid aulaId)
        {
            if (matriculaId == Guid.Empty)
                throw new ArgumentException("ID da matrícula é obrigatório.", nameof(matriculaId));

            if (aulaId == Guid.Empty)
                throw new ArgumentException("ID da aula é obrigatório.", nameof(aulaId));
        }

        /// <summary>
        /// Valida percentual
        /// </summary>
        private static void ValidarPercentual(decimal percentual)
        {
            if (percentual < 0 || percentual > 100)
                throw new ArgumentException("Percentual deve estar entre 0 e 100.", nameof(percentual));
        }

        /// <summary>
        /// Valida tempo
        /// </summary>
        private static void ValidarTempo(int tempo)
        {
            if (tempo < 0)
                throw new ArgumentException("Tempo não pode ser negativo.", nameof(tempo));

            if (tempo > 86400) // 24 horas em segundos
                throw new ArgumentException("Tempo assistido não pode exceder 24 horas.", nameof(tempo));
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
    /// Enum para status do progresso
    /// </summary>
    public enum StatusProgresso
    {
        /// <summary>
        /// Aula não iniciada
        /// </summary>
        NaoIniciado = 1,

        /// <summary>
        /// Aula em andamento
        /// </summary>
        EmAndamento = 2,

        /// <summary>
        /// Aula concluída
        /// </summary>
        Concluido = 3
    }
} 