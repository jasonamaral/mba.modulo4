using System;
using System.Collections.Generic;
using System.Linq;
using Alunos.Domain.Common;

namespace Alunos.Domain.Entities
{
    /// <summary>
    /// Entidade MatriculaCurso - Representa a matrícula de um aluno em um curso
    /// </summary>
    public class MatriculaCurso : Entidade
    {
        /// <summary>
        /// ID do aluno matriculado
        /// </summary>
        public Guid AlunoId { get; private set; }

        /// <summary>
        /// Referência de navegação para o aluno
        /// </summary>
        public Aluno Aluno { get; private set; }

        /// <summary>
        /// ID do curso
        /// </summary>
        public Guid CursoId { get; private set; }

        /// <summary>
        /// Data da matrícula
        /// </summary>
        public DateTime DataMatricula { get; private set; }

        /// <summary>
        /// Data de início do curso
        /// </summary>
        public DateTime DataInicio { get; private set; }

        /// <summary>
        /// Data de término do curso
        /// </summary>
        public DateTime? DataTermino { get; private set; }

        /// <summary>
        /// Status da matrícula
        /// </summary>
        public StatusMatricula Status { get; private set; }

        /// <summary>
        /// Valor pago pela matrícula
        /// </summary>
        public decimal ValorPago { get; private set; }

        /// <summary>
        /// Forma de pagamento
        /// </summary>
        public string FormaPagamento { get; private set; }

        /// <summary>
        /// Percentual de conclusão do curso
        /// </summary>
        public decimal PercentualConclusao { get; private set; }

        /// <summary>
        /// Nota final do aluno
        /// </summary>
        public decimal? NotaFinal { get; private set; }

        /// <summary>
        /// Observações sobre a matrícula
        /// </summary>
        public string Observacoes { get; private set; }

        /// <summary>
        /// Indica se a matrícula está ativa
        /// </summary>
        public bool IsAtiva { get; private set; }

        /// <summary>
        /// Lista privada de progresso nas aulas
        /// </summary>
        private readonly List<Progresso> _progresso = new();

        /// <summary>
        /// Coleção somente leitura do progresso
        /// </summary>
        public IReadOnlyCollection<Progresso> Progresso => _progresso.AsReadOnly();

        /// <summary>
        /// Lista privada de certificados
        /// </summary>
        private readonly List<Certificado> _certificados = new();

        /// <summary>
        /// Coleção somente leitura dos certificados
        /// </summary>
        public IReadOnlyCollection<Certificado> Certificados => _certificados.AsReadOnly();

        /// <summary>
        /// Construtor protegido para Entity Framework
        /// </summary>
        protected MatriculaCurso() : base()
        {
            FormaPagamento = string.Empty;
            Observacoes = string.Empty;
            Aluno = null!;
        }

        /// <summary>
        /// Construtor principal para criação de nova matrícula
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="cursoId">ID do curso</param>
        /// <param name="dataInicio">Data de início do curso</param>
        /// <param name="valorPago">Valor pago pela matrícula</param>
        /// <param name="formaPagamento">Forma de pagamento</param>
        /// <param name="observacoes">Observações</param>
        public MatriculaCurso(
            Guid alunoId,
            Guid cursoId,
            DateTime dataInicio,
            decimal valorPago,
            string formaPagamento = "",
            string observacoes = "") : base()
        {
            ValidarDadosObrigatorios(alunoId, cursoId, dataInicio, valorPago);

            AlunoId = alunoId;
            CursoId = cursoId;
            DataMatricula = DateTime.UtcNow;
            DataInicio = dataInicio.Date;
            ValorPago = valorPago;
            FormaPagamento = formaPagamento?.Trim() ?? string.Empty;
            Observacoes = observacoes?.Trim() ?? string.Empty;
            Status = StatusMatricula.Ativa;
            PercentualConclusao = 0;
            IsAtiva = true;
        }

        /// <summary>
        /// Atualiza os dados da matrícula
        /// </summary>
        /// <param name="dataInicio">Nova data de início</param>
        /// <param name="valorPago">Novo valor pago</param>
        /// <param name="formaPagamento">Nova forma de pagamento</param>
        /// <param name="observacoes">Novas observações</param>
        public void AtualizarDados(
            DateTime dataInicio,
            decimal valorPago,
            string formaPagamento = "",
            string observacoes = "")
        {
            if (Status == StatusMatricula.Concluida)
                throw new InvalidOperationException("Não é possível atualizar uma matrícula concluída.");

            if (Status == StatusMatricula.Cancelada)
                throw new InvalidOperationException("Não é possível atualizar uma matrícula cancelada.");

            ValidarValorPago(valorPago);

            DataInicio = dataInicio.Date;
            ValorPago = valorPago;
            FormaPagamento = formaPagamento?.Trim() ?? string.Empty;
            Observacoes = observacoes?.Trim() ?? string.Empty;

            SetUpdatedAt();
        }

        /// <summary>
        /// Inicia o curso
        /// </summary>
        public void IniciarCurso()
        {
            if (Status != StatusMatricula.Ativa)
                throw new InvalidOperationException("Apenas matrículas ativas podem ser iniciadas.");

            Status = StatusMatricula.EmAndamento;
            SetUpdatedAt();
        }

        /// <summary>
        /// Conclui o curso
        /// </summary>
        /// <param name="notaFinal">Nota final do aluno</param>
        public void ConcluirCurso(decimal? notaFinal = null)
        {
            if (Status != StatusMatricula.EmAndamento)
                throw new InvalidOperationException("Apenas matrículas em andamento podem ser concluídas.");

            if (notaFinal.HasValue)
            {
                ValidarNota(notaFinal.Value);
                NotaFinal = notaFinal.Value;
            }

            Status = StatusMatricula.Concluida;
            DataTermino = DateTime.UtcNow;
            PercentualConclusao = 100;
            SetUpdatedAt();
        }

        /// <summary>
        /// Cancela a matrícula
        /// </summary>
        /// <param name="motivo">Motivo do cancelamento</param>
        public void CancelarMatricula(string motivo = "")
        {
            if (Status == StatusMatricula.Concluida)
                throw new InvalidOperationException("Não é possível cancelar uma matrícula concluída.");

            Status = StatusMatricula.Cancelada;
            IsAtiva = false;
            
            if (!string.IsNullOrWhiteSpace(motivo))
            {
                Observacoes = string.IsNullOrWhiteSpace(Observacoes) 
                    ? $"Cancelamento: {motivo.Trim()}" 
                    : $"{Observacoes} | Cancelamento: {motivo.Trim()}";
            }

            SetUpdatedAt();
        }

        /// <summary>
        /// Suspende a matrícula
        /// </summary>
        /// <param name="motivo">Motivo da suspensão</param>
        public void SuspenderMatricula(string motivo = "")
        {
            if (Status == StatusMatricula.Concluida)
                throw new InvalidOperationException("Não é possível suspender uma matrícula concluída.");

            if (Status == StatusMatricula.Cancelada)
                throw new InvalidOperationException("Não é possível suspender uma matrícula cancelada.");

            Status = StatusMatricula.Suspensa;
            
            if (!string.IsNullOrWhiteSpace(motivo))
            {
                Observacoes = string.IsNullOrWhiteSpace(Observacoes) 
                    ? $"Suspensão: {motivo.Trim()}" 
                    : $"{Observacoes} | Suspensão: {motivo.Trim()}";
            }

            SetUpdatedAt();
        }

        /// <summary>
        /// Reativa a matrícula
        /// </summary>
        public void ReativarMatricula()
        {
            if (Status != StatusMatricula.Suspensa)
                throw new InvalidOperationException("Apenas matrículas suspensas podem ser reativadas.");

            Status = StatusMatricula.Ativa;
            IsAtiva = true;
            SetUpdatedAt();
        }

        /// <summary>
        /// Atualiza o percentual de conclusão
        /// </summary>
        /// <param name="percentual">Novo percentual (0-100)</param>
        public void AtualizarPercentualConclusao(decimal percentual)
        {
            if (percentual < 0 || percentual > 100)
                throw new ArgumentException("Percentual deve estar entre 0 e 100.", nameof(percentual));

            PercentualConclusao = percentual;
            SetUpdatedAt();
        }

        /// <summary>
        /// Adiciona progresso de uma aula
        /// </summary>
        /// <param name="progresso">Progresso a ser adicionado</param>
        public void AdicionarProgresso(Progresso progresso)
        {
            if (progresso == null)
                throw new ArgumentNullException(nameof(progresso));

            if (Status != StatusMatricula.EmAndamento)
                throw new InvalidOperationException("Apenas matrículas em andamento podem ter progresso adicionado.");

            var progressoExistente = _progresso.FirstOrDefault(p => p.AulaId == progresso.AulaId);
            if (progressoExistente != null)
            {
                _progresso.Remove(progressoExistente);
            }

            _progresso.Add(progresso);
            SetUpdatedAt();
        }

        /// <summary>
        /// Adiciona certificado
        /// </summary>
        /// <param name="certificado">Certificado a ser adicionado</param>
        public void AdicionarCertificado(Certificado certificado)
        {
            if (certificado == null)
                throw new ArgumentNullException(nameof(certificado));

            if (Status != StatusMatricula.Concluida)
                throw new InvalidOperationException("Apenas matrículas concluídas podem ter certificados.");

            _certificados.Add(certificado);
            SetUpdatedAt();
        }

        /// <summary>
        /// Calcula a duração do curso em dias
        /// </summary>
        /// <returns>Duração em dias</returns>
        public int CalcularDuracaoCurso()
        {
            var dataFim = DataTermino ?? DateTime.UtcNow;
            return (dataFim.Date - DataInicio.Date).Days;
        }

        /// <summary>
        /// Verifica se a matrícula está vencida
        /// </summary>
        /// <param name="diasVencimento">Dias para considerar vencida</param>
        /// <returns>True se vencida</returns>
        public bool EstaVencida(int diasVencimento = 365)
        {
            if (Status == StatusMatricula.Concluida || Status == StatusMatricula.Cancelada)
                return false;

            return DateTime.UtcNow.Date > DataInicio.AddDays(diasVencimento).Date;
        }

        /// <summary>
        /// Valida dados obrigatórios
        /// </summary>
        private static void ValidarDadosObrigatorios(Guid alunoId, Guid cursoId, DateTime dataInicio, decimal valorPago)
        {
            if (alunoId == Guid.Empty)
                throw new ArgumentException("ID do aluno é obrigatório.", nameof(alunoId));

            if (cursoId == Guid.Empty)
                throw new ArgumentException("ID do curso é obrigatório.", nameof(cursoId));

            if (dataInicio.Date < DateTime.Today.AddDays(-30))
                throw new ArgumentException("Data de início não pode ser muito antiga.", nameof(dataInicio));

            ValidarValorPago(valorPago);
        }

        /// <summary>
        /// Valida valor pago
        /// </summary>
        private static void ValidarValorPago(decimal valorPago)
        {
            if (valorPago < 0)
                throw new ArgumentException("Valor pago não pode ser negativo.", nameof(valorPago));

            if (valorPago > 999999.99m)
                throw new ArgumentException("Valor pago é muito alto.", nameof(valorPago));
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
    /// Enum para status da matrícula
    /// </summary>
    public enum StatusMatricula
    {
        /// <summary>
        /// Matrícula ativa, mas curso não iniciado
        /// </summary>
        Ativa = 1,

        /// <summary>
        /// Curso em andamento
        /// </summary>
        EmAndamento = 2,

        /// <summary>
        /// Curso concluído
        /// </summary>
        Concluida = 3,

        /// <summary>
        /// Matrícula cancelada
        /// </summary>
        Cancelada = 4,

        /// <summary>
        /// Matrícula suspensa
        /// </summary>
        Suspensa = 5
    }
} 