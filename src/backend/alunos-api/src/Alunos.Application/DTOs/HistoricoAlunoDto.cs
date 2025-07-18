using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alunos.Application.DTOs
{
    /// <summary>
    /// DTO para cadastro de histórico do aluno
    /// </summary>
    public class HistoricoAlunoCadastroDto
    {
        /// <summary>
        /// ID do aluno
        /// </summary>
        [Required(ErrorMessage = "ID do aluno é obrigatório")]
        public Guid AlunoId { get; set; }

        /// <summary>
        /// Ação realizada
        /// </summary>
        [Required(ErrorMessage = "Ação é obrigatória")]
        [StringLength(100, ErrorMessage = "Ação deve ter no máximo 100 caracteres")]
        public string Acao { get; set; } = string.Empty;

        /// <summary>
        /// Descrição da ação
        /// </summary>
        [Required(ErrorMessage = "Descrição é obrigatória")]
        [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Tipo da ação
        /// </summary>
        [Required(ErrorMessage = "Tipo da ação é obrigatório")]
        public string TipoAcao { get; set; } = string.Empty;

        /// <summary>
        /// Detalhes em formato JSON
        /// </summary>
        public string DetalhesJson { get; set; } = string.Empty;

        /// <summary>
        /// ID do usuário que realizou a ação (se aplicável)
        /// </summary>
        public Guid? UsuarioId { get; set; }

        /// <summary>
        /// Endereço IP da ação
        /// </summary>
        [StringLength(50, ErrorMessage = "Endereço IP deve ter no máximo 50 caracteres")]
        public string EnderecoIP { get; set; } = string.Empty;

        /// <summary>
        /// User Agent da ação
        /// </summary>
        [StringLength(500, ErrorMessage = "User Agent deve ter no máximo 500 caracteres")]
        public string UserAgent { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO de resposta para histórico do aluno
    /// </summary>
    public class HistoricoAlunoDto
    {
        /// <summary>
        /// ID do histórico
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
        /// Ação realizada
        /// </summary>
        public string Acao { get; set; } = string.Empty;

        /// <summary>
        /// Descrição da ação
        /// </summary>
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Detalhes em formato JSON
        /// </summary>
        public string DetalhesJson { get; set; } = string.Empty;

        /// <summary>
        /// Tipo da ação
        /// </summary>
        public string TipoAcao { get; set; } = string.Empty;

        /// <summary>
        /// ID do usuário que realizou a ação (se aplicável)
        /// </summary>
        public Guid? UsuarioId { get; set; }

        /// <summary>
        /// Nome do usuário que realizou a ação
        /// </summary>
        public string NomeUsuario { get; set; } = string.Empty;

        /// <summary>
        /// Endereço IP da ação
        /// </summary>
        public string EnderecoIP { get; set; } = string.Empty;

        /// <summary>
        /// User Agent da ação
        /// </summary>
        public string UserAgent { get; set; } = string.Empty;

        /// <summary>
        /// Indica se a ação é recente
        /// </summary>
        public bool EhRecente { get; set; }

        /// <summary>
        /// Data de criação
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// DTO resumido para listagem de histórico
    /// </summary>
    public class HistoricoAlunoResumoDto
    {
        /// <summary>
        /// ID do histórico
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Ação realizada
        /// </summary>
        public string Acao { get; set; } = string.Empty;

        /// <summary>
        /// Descrição da ação
        /// </summary>
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Tipo da ação
        /// </summary>
        public string TipoAcao { get; set; } = string.Empty;

        /// <summary>
        /// Indica se a ação é recente
        /// </summary>
        public bool EhRecente { get; set; }

        /// <summary>
        /// Data de criação
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// DTO para filtros de histórico
    /// </summary>
    public class HistoricoAlunoFiltroDto
    {
        /// <summary>
        /// ID do aluno para filtrar
        /// </summary>
        public Guid? AlunoId { get; set; }

        /// <summary>
        /// Tipo de ação para filtrar
        /// </summary>
        public string TipoAcao { get; set; } = string.Empty;

        /// <summary>
        /// Data inicial para filtrar
        /// </summary>
        public DateTime? DataInicial { get; set; }

        /// <summary>
        /// Data final para filtrar
        /// </summary>
        public DateTime? DataFinal { get; set; }

        /// <summary>
        /// Endereço IP para filtrar
        /// </summary>
        public string EnderecoIP { get; set; } = string.Empty;

        /// <summary>
        /// Apenas ações recentes
        /// </summary>
        public bool? ApenasRecentes { get; set; }

        /// <summary>
        /// Página para paginação
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Página deve ser maior que 0")]
        public int Pagina { get; set; } = 1;

        /// <summary>
        /// Tamanho da página
        /// </summary>
        [Range(1, 100, ErrorMessage = "Tamanho da página deve estar entre 1 e 100")]
        public int TamanhoPagina { get; set; } = 20;

        /// <summary>
        /// Campo para ordenação
        /// </summary>
        public string CampoOrdenacao { get; set; } = "CreatedAt";

        /// <summary>
        /// Direção da ordenação (asc/desc)
        /// </summary>
        public string DirecaoOrdenacao { get; set; } = "desc";
    }

    /// <summary>
    /// DTO para relatório de histórico
    /// </summary>
    public class HistoricoAlunoRelatorioDto
    {
        /// <summary>
        /// ID do aluno
        /// </summary>
        public Guid AlunoId { get; set; }

        /// <summary>
        /// Nome do aluno
        /// </summary>
        public string NomeAluno { get; set; } = string.Empty;

        /// <summary>
        /// Período do relatório
        /// </summary>
        public string Periodo { get; set; } = string.Empty;

        /// <summary>
        /// Data inicial do relatório
        /// </summary>
        public DateTime DataInicial { get; set; }

        /// <summary>
        /// Data final do relatório
        /// </summary>
        public DateTime DataFinal { get; set; }

        /// <summary>
        /// Total de ações no período
        /// </summary>
        public int TotalAcoes { get; set; }

        /// <summary>
        /// Resumo por tipo de ação
        /// </summary>
        public List<HistoricoAlunoResumoTipoDto> ResumoPorTipo { get; set; } = new();

        /// <summary>
        /// Ações mais recentes
        /// </summary>
        public List<HistoricoAlunoDto> AcoesRecentes { get; set; } = new();

        /// <summary>
        /// Estatísticas de acesso
        /// </summary>
        public HistoricoAlunoEstatisticasDto Estatisticas { get; set; } = new();
    }

    /// <summary>
    /// DTO para resumo por tipo de ação
    /// </summary>
    public class HistoricoAlunoResumoTipoDto
    {
        /// <summary>
        /// Tipo da ação
        /// </summary>
        public string TipoAcao { get; set; } = string.Empty;

        /// <summary>
        /// Nome do tipo de ação
        /// </summary>
        public string NomeTipoAcao { get; set; } = string.Empty;

        /// <summary>
        /// Quantidade de ações
        /// </summary>
        public int Quantidade { get; set; }

        /// <summary>
        /// Percentual do total
        /// </summary>
        public decimal Percentual { get; set; }

        /// <summary>
        /// Primeira ocorrência
        /// </summary>
        public DateTime? PrimeiraOcorrencia { get; set; }

        /// <summary>
        /// Última ocorrência
        /// </summary>
        public DateTime? UltimaOcorrencia { get; set; }
    }

    /// <summary>
    /// DTO para estatísticas de histórico
    /// </summary>
    public class HistoricoAlunoEstatisticasDto
    {
        /// <summary>
        /// Total de logins
        /// </summary>
        public int TotalLogins { get; set; }

        /// <summary>
        /// Total de acessos a aulas
        /// </summary>
        public int TotalAcessosAulas { get; set; }

        /// <summary>
        /// Total de matrículas
        /// </summary>
        public int TotalMatriculas { get; set; }

        /// <summary>
        /// Total de conclusões
        /// </summary>
        public int TotalConclusoes { get; set; }

        /// <summary>
        /// Total de certificações
        /// </summary>
        public int TotalCertificacoes { get; set; }

        /// <summary>
        /// Média de ações por dia
        /// </summary>
        public decimal MediaAcoesPorDia { get; set; }

        /// <summary>
        /// Dia da semana com mais atividade
        /// </summary>
        public string DiaSemanaComMaisAtividade { get; set; } = string.Empty;

        /// <summary>
        /// Horário com mais atividade
        /// </summary>
        public string HorarioComMaisAtividade { get; set; } = string.Empty;

        /// <summary>
        /// Primeiro acesso
        /// </summary>
        public DateTime? PrimeiroAcesso { get; set; }

        /// <summary>
        /// Último acesso
        /// </summary>
        public DateTime? UltimoAcesso { get; set; }

        /// <summary>
        /// Dias desde último acesso
        /// </summary>
        public int DiasDesdeUltimoAcesso { get; set; }
    }

    /// <summary>
    /// DTO para resultado paginado de histórico
    /// </summary>
    public class HistoricoAlunoPaginadoDto
    {
        /// <summary>
        /// Lista de históricos
        /// </summary>
        public List<HistoricoAlunoDto> Historicos { get; set; } = new();

        /// <summary>
        /// Página atual
        /// </summary>
        public int PaginaAtual { get; set; }

        /// <summary>
        /// Total de páginas
        /// </summary>
        public int TotalPaginas { get; set; }

        /// <summary>
        /// Total de registros
        /// </summary>
        public int TotalRegistros { get; set; }

        /// <summary>
        /// Tamanho da página
        /// </summary>
        public int TamanhoPagina { get; set; }

        /// <summary>
        /// Indica se tem página anterior
        /// </summary>
        public bool TemPaginaAnterior { get; set; }

        /// <summary>
        /// Indica se tem próxima página
        /// </summary>
        public bool TemProximaPagina { get; set; }
    }
} 