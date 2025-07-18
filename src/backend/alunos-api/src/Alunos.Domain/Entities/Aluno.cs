using System;
using System.Collections.Generic;
using System.Linq;
using Alunos.Domain.Common;

namespace Alunos.Domain.Entities
{
    /// <summary>
    /// Entidade Aluno - Raiz de agregado para gestão de alunos
    /// </summary>
    public class Aluno : Entidade, IRaizAgregacao
    {
        /// <summary>
        /// Código do usuário no sistema de autenticação
        /// </summary>
        public Guid CodigoUsuarioAutenticacao { get; private set; }

        /// <summary>
        /// Nome completo do aluno
        /// </summary>
        public string Nome { get; private set; }

        /// <summary>
        /// Email do aluno
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Data de nascimento do aluno
        /// </summary>
        public DateTime DataNascimento { get; private set; }

        /// <summary>
        /// Telefone do aluno
        /// </summary>
        public string Telefone { get; private set; }

        /// <summary>
        /// Gênero do aluno
        /// </summary>
        public string Genero { get; private set; }

        /// <summary>
        /// Cidade do aluno
        /// </summary>
        public string Cidade { get; private set; }

        /// <summary>
        /// Estado do aluno
        /// </summary>
        public string Estado { get; private set; }

        /// <summary>
        /// CEP do aluno
        /// </summary>
        public string CEP { get; private set; }

        /// <summary>
        /// Indica se o aluno está ativo
        /// </summary>
        public bool IsAtivo { get; private set; }

        /// <summary>
        /// Lista privada de matrículas do aluno
        /// </summary>
        private readonly List<MatriculaCurso> _matriculasCursos = new();

        /// <summary>
        /// Coleção somente leitura das matrículas do aluno
        /// </summary>
        public IReadOnlyCollection<MatriculaCurso> MatriculasCursos => _matriculasCursos.AsReadOnly();

        /// <summary>
        /// Construtor protegido para Entity Framework
        /// </summary>
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

        /// <summary>
        /// Construtor principal para criação de novo aluno
        /// </summary>
        /// <param name="codigoUsuarioAutenticacao">Código do usuário no sistema de autenticação</param>
        /// <param name="nome">Nome completo do aluno</param>
        /// <param name="email">Email do aluno</param>
        /// <param name="dataNascimento">Data de nascimento</param>
        /// <param name="telefone">Telefone do aluno</param>
        /// <param name="genero">Gênero do aluno</param>
        /// <param name="cidade">Cidade do aluno</param>
        /// <param name="estado">Estado do aluno</param>
        /// <param name="cep">CEP do aluno</param>
        public Aluno(
            Guid codigoUsuarioAutenticacao,
            string nome,
            string email,
            DateTime dataNascimento,
            string telefone = "",
            string genero = "",
            string cidade = "",
            string estado = "",
            string cep = "") : base()
        {
            ValidarDadosObrigatorios(nome, email);
            ValidarIdade(dataNascimento);
            ValidarEmail(email);

            CodigoUsuarioAutenticacao = codigoUsuarioAutenticacao;
            Nome = nome.Trim();
            Email = email.Trim().ToLowerInvariant();
            DataNascimento = dataNascimento.Date;
            Telefone = telefone?.Trim() ?? string.Empty;
            Genero = genero?.Trim() ?? string.Empty;
            Cidade = cidade?.Trim() ?? string.Empty;
            Estado = estado?.Trim() ?? string.Empty;
            CEP = cep?.Trim() ?? string.Empty;
            IsAtivo = true;
        }

        /// <summary>
        /// Atualiza os dados do aluno
        /// </summary>
        /// <param name="nome">Nome completo</param>
        /// <param name="email">Email</param>
        /// <param name="dataNascimento">Data de nascimento</param>
        /// <param name="telefone">Telefone</param>
        /// <param name="genero">Gênero</param>
        /// <param name="cidade">Cidade</param>
        /// <param name="estado">Estado</param>
        /// <param name="cep">CEP</param>
        public void AtualizarDados(
            string nome,
            string email,
            DateTime dataNascimento,
            string telefone = "",
            string genero = "",
            string cidade = "",
            string estado = "",
            string cep = "")
        {
            ValidarDadosObrigatorios(nome, email);
            ValidarIdade(dataNascimento);
            ValidarEmail(email);

            Nome = nome.Trim();
            Email = email.Trim().ToLowerInvariant();
            DataNascimento = dataNascimento.Date;
            Telefone = telefone?.Trim() ?? string.Empty;
            Genero = genero?.Trim() ?? string.Empty;
            Cidade = cidade?.Trim() ?? string.Empty;
            Estado = estado?.Trim() ?? string.Empty;
            CEP = cep?.Trim() ?? string.Empty;

            SetUpdatedAt();
        }

        /// <summary>
        /// Ativa o aluno
        /// </summary>
        public void Ativar()
        {
            IsAtivo = true;
            SetUpdatedAt();
        }

        /// <summary>
        /// Desativa o aluno
        /// </summary>
        public void Desativar()
        {
            IsAtivo = false;
            SetUpdatedAt();
        }

        /// <summary>
        /// Adiciona uma matrícula ao aluno
        /// </summary>
        /// <param name="matricula">Matrícula a ser adicionada</param>
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

        /// <summary>
        /// Remove uma matrícula do aluno
        /// </summary>
        /// <param name="matriculaId">ID da matrícula a ser removida</param>
        public void RemoverMatricula(Guid matriculaId)
        {
            var matricula = _matriculasCursos.FirstOrDefault(m => m.Id == matriculaId);
            if (matricula == null)
                throw new ArgumentException("Matrícula não encontrada.", nameof(matriculaId));

            _matriculasCursos.Remove(matricula);
            SetUpdatedAt();
        }

        /// <summary>
        /// Obtém uma matrícula específica
        /// </summary>
        /// <param name="matriculaId">ID da matrícula</param>
        /// <returns>Matrícula encontrada</returns>
        public MatriculaCurso ObterMatricula(Guid matriculaId)
        {
            var matricula = _matriculasCursos.FirstOrDefault(m => m.Id == matriculaId);
            if (matricula == null)
                throw new ArgumentException("Matrícula não encontrada.", nameof(matriculaId));

            return matricula;
        }

        /// <summary>
        /// Obtém matrícula por curso
        /// </summary>
        /// <param name="cursoId">ID do curso</param>
        /// <returns>Matrícula no curso</returns>
        public MatriculaCurso ObterMatriculaPorCurso(Guid cursoId)
        {
            return _matriculasCursos.FirstOrDefault(m => m.CursoId == cursoId);
        }

        /// <summary>
        /// Verifica se o aluno está matriculado em um curso
        /// </summary>
        /// <param name="cursoId">ID do curso</param>
        /// <returns>True se matriculado</returns>
        public bool EstaMatriculadoNoCurso(Guid cursoId)
        {
            return _matriculasCursos.Any(m => m.CursoId == cursoId);
        }

        /// <summary>
        /// Calcula a idade do aluno
        /// </summary>
        /// <returns>Idade em anos</returns>
        public int CalcularIdade()
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - DataNascimento.Year;
            
            if (DataNascimento.Date > hoje.AddYears(-idade))
                idade--;

            return idade;
        }

        /// <summary>
        /// Valida dados obrigatórios
        /// </summary>
        private static void ValidarDadosObrigatorios(string nome, string email)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome é obrigatório.", nameof(nome));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email é obrigatório.", nameof(email));

            if (nome.Length < 2)
                throw new ArgumentException("Nome deve ter pelo menos 2 caracteres.", nameof(nome));

            if (nome.Length > 100)
                throw new ArgumentException("Nome deve ter no máximo 100 caracteres.", nameof(nome));
        }

        /// <summary>
        /// Valida idade do aluno
        /// </summary>
        private static void ValidarIdade(DateTime dataNascimento)
        {
            if (dataNascimento.Date > DateTime.Today)
                throw new ArgumentException("Data de nascimento não pode ser futura.", nameof(dataNascimento));

            var idade = DateTime.Today.Year - dataNascimento.Year;
            if (dataNascimento.Date > DateTime.Today.AddYears(-idade))
                idade--;

            if (idade < 16)
                throw new ArgumentException("Aluno deve ter pelo menos 16 anos.", nameof(dataNascimento));

            if (idade > 120)
                throw new ArgumentException("Data de nascimento inválida.", nameof(dataNascimento));
        }

        /// <summary>
        /// Valida formato do email
        /// </summary>
        private static void ValidarEmail(string email)
        {
            if (!email.Contains('@') || !email.Contains('.'))
                throw new ArgumentException("Email deve ter formato válido.", nameof(email));

            if (email.Length > 200)
                throw new ArgumentException("Email deve ter no máximo 200 caracteres.", nameof(email));
        }
    }
} 