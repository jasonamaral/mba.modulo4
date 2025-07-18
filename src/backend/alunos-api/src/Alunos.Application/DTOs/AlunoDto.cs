using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alunos.Application.DTOs
{
    /// <summary>
    /// DTO para cadastro de aluno
    /// </summary>
    public class AlunoCadastroDto
    {
        /// <summary>
        /// Código do usuário no sistema de autenticação
        /// </summary>
        [Required(ErrorMessage = "Código do usuário de autenticação é obrigatório")]
        public Guid CodigoUsuarioAutenticacao { get; set; }

        /// <summary>
        /// Nome completo do aluno
        /// </summary>
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Email do aluno
        /// </summary>
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        [StringLength(200, ErrorMessage = "Email deve ter no máximo 200 caracteres")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Data de nascimento do aluno
        /// </summary>
        [Required(ErrorMessage = "Data de nascimento é obrigatória")]
        public DateTime DataNascimento { get; set; }

        /// <summary>
        /// Telefone do aluno
        /// </summary>
        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string Telefone { get; set; } = string.Empty;

        /// <summary>
        /// Gênero do aluno
        /// </summary>
        [StringLength(20, ErrorMessage = "Gênero deve ter no máximo 20 caracteres")]
        public string Genero { get; set; } = string.Empty;

        /// <summary>
        /// Cidade do aluno
        /// </summary>
        [StringLength(100, ErrorMessage = "Cidade deve ter no máximo 100 caracteres")]
        public string Cidade { get; set; } = string.Empty;

        /// <summary>
        /// Estado do aluno
        /// </summary>
        [StringLength(50, ErrorMessage = "Estado deve ter no máximo 50 caracteres")]
        public string Estado { get; set; } = string.Empty;

        /// <summary>
        /// CEP do aluno
        /// </summary>
        [StringLength(10, ErrorMessage = "CEP deve ter no máximo 10 caracteres")]
        public string CEP { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para atualização de aluno
    /// </summary>
    public class AlunoAtualizarDto
    {
        /// <summary>
        /// Nome completo do aluno
        /// </summary>
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Email do aluno
        /// </summary>
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        [StringLength(200, ErrorMessage = "Email deve ter no máximo 200 caracteres")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Data de nascimento do aluno
        /// </summary>
        [Required(ErrorMessage = "Data de nascimento é obrigatória")]
        public DateTime DataNascimento { get; set; }

        /// <summary>
        /// Telefone do aluno
        /// </summary>
        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string Telefone { get; set; } = string.Empty;

        /// <summary>
        /// Gênero do aluno
        /// </summary>
        [StringLength(20, ErrorMessage = "Gênero deve ter no máximo 20 caracteres")]
        public string Genero { get; set; } = string.Empty;

        /// <summary>
        /// Cidade do aluno
        /// </summary>
        [StringLength(100, ErrorMessage = "Cidade deve ter no máximo 100 caracteres")]
        public string Cidade { get; set; } = string.Empty;

        /// <summary>
        /// Estado do aluno
        /// </summary>
        [StringLength(50, ErrorMessage = "Estado deve ter no máximo 50 caracteres")]
        public string Estado { get; set; } = string.Empty;

        /// <summary>
        /// CEP do aluno
        /// </summary>
        [StringLength(10, ErrorMessage = "CEP deve ter no máximo 10 caracteres")]
        public string CEP { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO de resposta para aluno
    /// </summary>
    public class AlunoDto
    {
        /// <summary>
        /// ID do aluno
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Código do usuário no sistema de autenticação
        /// </summary>
        public Guid CodigoUsuarioAutenticacao { get; set; }

        /// <summary>
        /// Nome completo do aluno
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Email do aluno
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Data de nascimento do aluno
        /// </summary>
        public DateTime DataNascimento { get; set; }

        /// <summary>
        /// Idade calculada do aluno
        /// </summary>
        public int Idade { get; set; }

        /// <summary>
        /// Telefone do aluno
        /// </summary>
        public string Telefone { get; set; } = string.Empty;

        /// <summary>
        /// Gênero do aluno
        /// </summary>
        public string Genero { get; set; } = string.Empty;

        /// <summary>
        /// Cidade do aluno
        /// </summary>
        public string Cidade { get; set; } = string.Empty;

        /// <summary>
        /// Estado do aluno
        /// </summary>
        public string Estado { get; set; } = string.Empty;

        /// <summary>
        /// CEP do aluno
        /// </summary>
        public string CEP { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o aluno está ativo
        /// </summary>
        public bool IsAtivo { get; set; }

        /// <summary>
        /// Data de criação
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Data de atualização
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Lista de matrículas do aluno
        /// </summary>
        public List<MatriculaDto> Matriculas { get; set; } = new();
    }

    /// <summary>
    /// DTO resumido para listagem de alunos
    /// </summary>
    public class AlunoResumoDto
    {
        /// <summary>
        /// ID do aluno
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nome completo do aluno
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Email do aluno
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Idade calculada do aluno
        /// </summary>
        public int Idade { get; set; }

        /// <summary>
        /// Cidade do aluno
        /// </summary>
        public string Cidade { get; set; } = string.Empty;

        /// <summary>
        /// Estado do aluno
        /// </summary>
        public string Estado { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o aluno está ativo
        /// </summary>
        public bool IsAtivo { get; set; }

        /// <summary>
        /// Quantidade de matrículas ativas
        /// </summary>
        public int QuantidadeMatriculasAtivas { get; set; }

        /// <summary>
        /// Quantidade de cursos concluídos
        /// </summary>
        public int QuantidadeCursosConcluidos { get; set; }

        /// <summary>
        /// Data de criação
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// DTO para perfil completo do aluno
    /// </summary>
    public class AlunoPerfilDto
    {
        /// <summary>
        /// Dados básicos do aluno
        /// </summary>
        public AlunoDto Aluno { get; set; } = new();

        /// <summary>
        /// Estatísticas do aluno
        /// </summary>
        public AlunoEstatisticasDto Estatisticas { get; set; } = new();

        /// <summary>
        /// Histórico recente do aluno
        /// </summary>
        public List<HistoricoAlunoDto> HistoricoRecente { get; set; } = new();
    }

    /// <summary>
    /// DTO para estatísticas do aluno
    /// </summary>
    public class AlunoEstatisticasDto
    {
        /// <summary>
        /// Total de matrículas
        /// </summary>
        public int TotalMatriculas { get; set; }

        /// <summary>
        /// Matrículas ativas
        /// </summary>
        public int MatriculasAtivas { get; set; }

        /// <summary>
        /// Cursos concluídos
        /// </summary>
        public int CursosConcluidos { get; set; }

        /// <summary>
        /// Certificados emitidos
        /// </summary>
        public int CertificadosEmitidos { get; set; }

        /// <summary>
        /// Horas de estudo total
        /// </summary>
        public decimal HorasEstudoTotal { get; set; }

        /// <summary>
        /// Média de notas
        /// </summary>
        public decimal MediaNotas { get; set; }

        /// <summary>
        /// Percentual de conclusão médio
        /// </summary>
        public decimal PercentualConclusaoMedio { get; set; }

        /// <summary>
        /// Último acesso
        /// </summary>
        public DateTime? UltimoAcesso { get; set; }
    }

    /// <summary>
    /// DTO para dashboard do aluno
    /// </summary>
    public class AlunoDashboardDto
    {
        /// <summary>
        /// Informações básicas do aluno
        /// </summary>
        public AlunoResumoDto Aluno { get; set; } = new();

        /// <summary>
        /// Estatísticas do aluno
        /// </summary>
        public AlunoEstatisticasDto Estatisticas { get; set; } = new();

        /// <summary>
        /// Matrículas em andamento
        /// </summary>
        public List<MatriculaDto> MatriculasEmAndamento { get; set; } = new();

        /// <summary>
        /// Próximas aulas
        /// </summary>
        public List<ProximaAulaDto> ProximasAulas { get; set; } = new();

        /// <summary>
        /// Certificados recentes
        /// </summary>
        public List<CertificadoDto> CertificadosRecentes { get; set; } = new();

        /// <summary>
        /// Atividades recentes
        /// </summary>
        public List<HistoricoAlunoDto> AtividadesRecentes { get; set; } = new();
    }

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