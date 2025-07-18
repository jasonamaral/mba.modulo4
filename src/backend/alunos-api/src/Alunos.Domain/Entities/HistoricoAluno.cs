using System;
using Alunos.Domain.Common;

namespace Alunos.Domain.Entities
{
    /// <summary>
    /// Entidade HistoricoAluno - Representa o histórico de ações do aluno
    /// </summary>
    public class HistoricoAluno : Entidade
    {
        /// <summary>
        /// ID do aluno
        /// </summary>
        public Guid AlunoId { get; private set; }

        /// <summary>
        /// Referência de navegação para o aluno
        /// </summary>
        public Aluno Aluno { get; private set; }

        /// <summary>
        /// Ação realizada
        /// </summary>
        public string Acao { get; private set; }

        /// <summary>
        /// Descrição da ação
        /// </summary>
        public string Descricao { get; private set; }

        /// <summary>
        /// Detalhes em formato JSON
        /// </summary>
        public string DetalhesJson { get; private set; }

        /// <summary>
        /// Tipo da ação
        /// </summary>
        public TipoAcaoHistorico TipoAcao { get; private set; }

        /// <summary>
        /// ID do usuário que realizou a ação (se aplicável)
        /// </summary>
        public Guid? UsuarioId { get; private set; }

        /// <summary>
        /// Endereço IP da ação
        /// </summary>
        public string EnderecoIP { get; private set; }

        /// <summary>
        /// User Agent da ação
        /// </summary>
        public string UserAgent { get; private set; }

        /// <summary>
        /// Construtor protegido para Entity Framework
        /// </summary>
        protected HistoricoAluno() : base()
        {
            Acao = string.Empty;
            Descricao = string.Empty;
            DetalhesJson = string.Empty;
            EnderecoIP = string.Empty;
            UserAgent = string.Empty;
            Aluno = null!;
        }

        /// <summary>
        /// Construtor principal para criação de novo histórico
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="acao">Ação realizada</param>
        /// <param name="descricao">Descrição da ação</param>
        /// <param name="tipoAcao">Tipo da ação</param>
        /// <param name="detalhesJson">Detalhes em JSON</param>
        /// <param name="usuarioId">ID do usuário que realizou a ação</param>
        /// <param name="enderecoIP">Endereço IP</param>
        /// <param name="userAgent">User Agent</param>
        public HistoricoAluno(
            Guid alunoId,
            string acao,
            string descricao,
            TipoAcaoHistorico tipoAcao,
            string detalhesJson = "",
            Guid? usuarioId = null,
            string enderecoIP = "",
            string userAgent = "") : base()
        {
            ValidarDadosObrigatorios(alunoId, acao, descricao);

            AlunoId = alunoId;
            Acao = acao.Trim();
            Descricao = descricao.Trim();
            TipoAcao = tipoAcao;
            DetalhesJson = detalhesJson?.Trim() ?? string.Empty;
            UsuarioId = usuarioId;
            EnderecoIP = enderecoIP?.Trim() ?? string.Empty;
            UserAgent = userAgent?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Cria histórico de cadastro
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="detalhes">Detalhes do cadastro</param>
        /// <param name="usuarioId">ID do usuário</param>
        /// <param name="enderecoIP">Endereço IP</param>
        /// <returns>Histórico criado</returns>
        public static HistoricoAluno CriarHistoricoCadastro(
            Guid alunoId,
            string detalhes = "",
            Guid? usuarioId = null,
            string enderecoIP = "")
        {
            return new HistoricoAluno(
                alunoId,
                "Cadastro",
                "Aluno cadastrado no sistema",
                TipoAcaoHistorico.Cadastro,
                detalhes,
                usuarioId,
                enderecoIP);
        }

        /// <summary>
        /// Cria histórico de atualização
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="detalhes">Detalhes da atualização</param>
        /// <param name="usuarioId">ID do usuário</param>
        /// <param name="enderecoIP">Endereço IP</param>
        /// <returns>Histórico criado</returns>
        public static HistoricoAluno CriarHistoricoAtualizacao(
            Guid alunoId,
            string detalhes = "",
            Guid? usuarioId = null,
            string enderecoIP = "")
        {
            return new HistoricoAluno(
                alunoId,
                "Atualização",
                "Dados do aluno atualizados",
                TipoAcaoHistorico.Atualizacao,
                detalhes,
                usuarioId,
                enderecoIP);
        }

        /// <summary>
        /// Cria histórico de matrícula
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="cursoId">ID do curso</param>
        /// <param name="nomeCurso">Nome do curso</param>
        /// <param name="detalhes">Detalhes da matrícula</param>
        /// <param name="usuarioId">ID do usuário</param>
        /// <param name="enderecoIP">Endereço IP</param>
        /// <returns>Histórico criado</returns>
        public static HistoricoAluno CriarHistoricoMatricula(
            Guid alunoId,
            Guid cursoId,
            string nomeCurso,
            string detalhes = "",
            Guid? usuarioId = null,
            string enderecoIP = "")
        {
            var detalhesJson = string.IsNullOrWhiteSpace(detalhes) 
                ? $"{{\"cursoId\":\"{cursoId}\",\"nomeCurso\":\"{nomeCurso}\"}}"
                : detalhes;

            return new HistoricoAluno(
                alunoId,
                "Matrícula",
                $"Matrícula realizada no curso: {nomeCurso}",
                TipoAcaoHistorico.Matricula,
                detalhesJson,
                usuarioId,
                enderecoIP);
        }

        /// <summary>
        /// Cria histórico de conclusão de curso
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="cursoId">ID do curso</param>
        /// <param name="nomeCurso">Nome do curso</param>
        /// <param name="notaFinal">Nota final</param>
        /// <param name="detalhes">Detalhes da conclusão</param>
        /// <param name="usuarioId">ID do usuário</param>
        /// <param name="enderecoIP">Endereço IP</param>
        /// <returns>Histórico criado</returns>
        public static HistoricoAluno CriarHistoricoConclusao(
            Guid alunoId,
            Guid cursoId,
            string nomeCurso,
            decimal? notaFinal = null,
            string detalhes = "",
            Guid? usuarioId = null,
            string enderecoIP = "")
        {
            var detalhesJson = string.IsNullOrWhiteSpace(detalhes) 
                ? $"{{\"cursoId\":\"{cursoId}\",\"nomeCurso\":\"{nomeCurso}\",\"notaFinal\":{notaFinal}}}"
                : detalhes;

            var descricao = notaFinal.HasValue 
                ? $"Curso concluído: {nomeCurso} - Nota: {notaFinal:F1}"
                : $"Curso concluído: {nomeCurso}";

            return new HistoricoAluno(
                alunoId,
                "Conclusão",
                descricao,
                TipoAcaoHistorico.Conclusao,
                detalhesJson,
                usuarioId,
                enderecoIP);
        }

        /// <summary>
        /// Cria histórico de certificação
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="certificadoId">ID do certificado</param>
        /// <param name="codigoCertificado">Código do certificado</param>
        /// <param name="nomeCurso">Nome do curso</param>
        /// <param name="detalhes">Detalhes da certificação</param>
        /// <param name="usuarioId">ID do usuário</param>
        /// <param name="enderecoIP">Endereço IP</param>
        /// <returns>Histórico criado</returns>
        public static HistoricoAluno CriarHistoricoCertificacao(
            Guid alunoId,
            Guid certificadoId,
            string codigoCertificado,
            string nomeCurso,
            string detalhes = "",
            Guid? usuarioId = null,
            string enderecoIP = "")
        {
            var detalhesJson = string.IsNullOrWhiteSpace(detalhes) 
                ? $"{{\"certificadoId\":\"{certificadoId}\",\"codigo\":\"{codigoCertificado}\",\"nomeCurso\":\"{nomeCurso}\"}}"
                : detalhes;

            return new HistoricoAluno(
                alunoId,
                "Certificação",
                $"Certificado emitido para o curso: {nomeCurso} - Código: {codigoCertificado}",
                TipoAcaoHistorico.Certificacao,
                detalhesJson,
                usuarioId,
                enderecoIP);
        }

        /// <summary>
        /// Cria histórico de login
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="enderecoIP">Endereço IP</param>
        /// <param name="userAgent">User Agent</param>
        /// <param name="detalhes">Detalhes do login</param>
        /// <returns>Histórico criado</returns>
        public static HistoricoAluno CriarHistoricoLogin(
            Guid alunoId,
            string enderecoIP = "",
            string userAgent = "",
            string detalhes = "")
        {
            return new HistoricoAluno(
                alunoId,
                "Login",
                "Aluno fez login no sistema",
                TipoAcaoHistorico.Login,
                detalhes,
                null,
                enderecoIP,
                userAgent);
        }

        /// <summary>
        /// Cria histórico de acesso a aula
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="aulaId">ID da aula</param>
        /// <param name="nomeAula">Nome da aula</param>
        /// <param name="detalhes">Detalhes do acesso</param>
        /// <param name="enderecoIP">Endereço IP</param>
        /// <returns>Histórico criado</returns>
        public static HistoricoAluno CriarHistoricoAcessoAula(
            Guid alunoId,
            Guid aulaId,
            string nomeAula,
            string detalhes = "",
            string enderecoIP = "")
        {
            var detalhesJson = string.IsNullOrWhiteSpace(detalhes) 
                ? $"{{\"aulaId\":\"{aulaId}\",\"nomeAula\":\"{nomeAula}\"}}"
                : detalhes;

            return new HistoricoAluno(
                alunoId,
                "Acesso Aula",
                $"Acesso à aula: {nomeAula}",
                TipoAcaoHistorico.AcessoAula,
                detalhesJson,
                null,
                enderecoIP);
        }

        /// <summary>
        /// Verifica se a ação é recente
        /// </summary>
        /// <param name="minutos">Minutos para considerar recente</param>
        /// <returns>True se recente</returns>
        public bool EhRecente(int minutos = 30)
        {
            return DateTime.UtcNow <= CreatedAt.AddMinutes(minutos);
        }

        /// <summary>
        /// Valida dados obrigatórios
        /// </summary>
        private static void ValidarDadosObrigatorios(Guid alunoId, string acao, string descricao)
        {
            if (alunoId == Guid.Empty)
                throw new ArgumentException("ID do aluno é obrigatório.", nameof(alunoId));

            if (string.IsNullOrWhiteSpace(acao))
                throw new ArgumentException("Ação é obrigatória.", nameof(acao));

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição é obrigatória.", nameof(descricao));

            if (acao.Length > 100)
                throw new ArgumentException("Ação deve ter no máximo 100 caracteres.", nameof(acao));

            if (descricao.Length > 500)
                throw new ArgumentException("Descrição deve ter no máximo 500 caracteres.", nameof(descricao));
        }
    }

    /// <summary>
    /// Enum para tipos de ação do histórico
    /// </summary>
    public enum TipoAcaoHistorico
    {
        /// <summary>
        /// Cadastro do aluno
        /// </summary>
        Cadastro = 1,

        /// <summary>
        /// Atualização de dados
        /// </summary>
        Atualizacao = 2,

        /// <summary>
        /// Matrícula em curso
        /// </summary>
        Matricula = 3,

        /// <summary>
        /// Conclusão de curso
        /// </summary>
        Conclusao = 4,

        /// <summary>
        /// Emissão de certificado
        /// </summary>
        Certificacao = 5,

        /// <summary>
        /// Login no sistema
        /// </summary>
        Login = 6,

        /// <summary>
        /// Acesso a aula
        /// </summary>
        AcessoAula = 7,

        /// <summary>
        /// Cancelamento de matrícula
        /// </summary>
        Cancelamento = 8,

        /// <summary>
        /// Suspensão de matrícula
        /// </summary>
        Suspensao = 9,

        /// <summary>
        /// Reativação de matrícula
        /// </summary>
        Reativacao = 10
    }
} 