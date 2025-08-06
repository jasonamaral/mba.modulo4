using Alunos.Domain.Enumerators;
using Alunos.Domain.ValueObjects;
using Core.DomainValidations;
using Core.Utils;
using Plataforma.Educacao.Core.Exceptions;
using System.Text.Json.Serialization;

namespace Alunos.Domain.Entities;
public class MatriculaCurso : Common.Entidade
{
    #region Atributos
    public Guid AlunoId { get; }
    public Guid CursoId { get; }
    public string NomeCurso { get; }
    public decimal Valor { get; }
    public DateTime DataMatricula { get; }
    public DateTime? DataConclusao { get; private set; }
    public EstadoMatriculaCursoEnum EstadoMatricula { get; private set; }
    public byte? NotaFinal { get; private set; }
    public string Observacao { get; private set; }

    private readonly List<HistoricoAprendizado> _historicoAprendizado = [];
    public IReadOnlyCollection<HistoricoAprendizado> HistoricoAprendizado => _historicoAprendizado.AsReadOnly();
    public Certificado Certificado { get; private set; }

    [JsonIgnore]
    public Aluno Aluno { get; private set; }
    #endregion

    #region CTOR
    // EF Compatibility
    protected MatriculaCurso() { }

    public MatriculaCurso(Guid alunoId,
        Guid cursoId,
        string nomeCurso,
        decimal valor,
        string observacao)
    {
        AlunoId = alunoId;
        CursoId = cursoId;
        NomeCurso = nomeCurso?.Trim() ?? string.Empty;
        Valor = valor;
        DataMatricula = DateTime.UtcNow;
        EstadoMatricula = EstadoMatriculaCursoEnum.PendentePagamento;
        Observacao = observacao;

        ValidarIntegridadeMatriculaCurso();
    }
    #endregion

    #region Métodos
    internal short QuantidadeTotalCargaHoraria() => (short)_historicoAprendizado.Sum(x => x.CargaHoraria);
    //public int QuantidadeAulasFinalizadas => _historicoAprendizado.Count(h => h.DataTermino.HasValue);
    //public int QuantidadeAulasEmAndamento => _historicoAprendizado.Count(h => !h.DataTermino.HasValue);
    public bool MatriculaCursoConcluido() => DataConclusao.HasValue;
    internal bool MatriculaCursoDisponivel() => !DataConclusao.HasValue && EstadoMatricula == EstadoMatriculaCursoEnum.PagamentoRealizado;
    internal bool PodeConcluirCurso() => EstadoMatricula == EstadoMatriculaCursoEnum.PagamentoRealizado && _historicoAprendizado.Count(h => !h.DataTermino.HasValue) == 0;

    //public bool PagamentoPodeSerRealizado => EstadoMatricula == EstadoMatriculaCursoEnum.PendentePagamento || EstadoMatricula == EstadoMatriculaCursoEnum.Abandonado;

    //internal HistoricoAprendizado ObterHistoricoAulaPeloId(Guid aulaId)
    //{
    //    var historico = _historicoAprendizado.FirstOrDefault(h => h.CursoId == CursoId && h.AulaId == aulaId);
    //    if (historico == null) { throw new DomainException("Historico não foi localizado"); }

    //    return historico;
    //}

    #region Manipuladores de MatriculaCurso
    internal void AtualizarNotaFinalCurso(byte notaFinal)
    {
        ValidarIntegridadeMatriculaCurso(novaNotaFinal: notaFinal);
        VerificarSeCertificadoExiste();
        Certificado.AtualizarNotaFinal(notaFinal);
        NotaFinal = notaFinal;
    }

    internal void RegistrarPagamentoMatricula()
    {
        ValidarIntegridadeMatriculaCurso(novoEstadoMatriculaCurso: EstadoMatriculaCursoEnum.PagamentoRealizado);
        EstadoMatricula = EstadoMatriculaCursoEnum.PagamentoRealizado;
    }

    internal void RegistrarAbandonoMatricula()
    {
        ValidarIntegridadeMatriculaCurso(novoEstadoMatriculaCurso: EstadoMatriculaCursoEnum.Abandonado);
        EstadoMatricula = EstadoMatriculaCursoEnum.Abandonado;
    }

    internal void ConcluirCurso()
    {
        if (EstadoMatricula == EstadoMatriculaCursoEnum.Abandonado) { throw new DomainException("Não é possível concluir um curso com estado de pagamento abandonado"); }
        if (!PodeConcluirCurso()) { throw new DomainException("Não é possível concluir o curso, existem aulas não finalizadas"); }
        if (MatriculaCursoConcluido()) { throw new DomainException("Curso já foi concluído"); }

        var dataAtual = DateTime.Now;
        ValidarIntegridadeMatriculaCurso(novaDataConclusao: dataAtual);
        DataConclusao = dataAtual;
        EstadoMatricula = EstadoMatriculaCursoEnum.Concluido;
    }

    internal void AtualizarObservacoes(string observacao)
    {
        ValidarIntegridadeMatriculaCurso(novaObservacao: observacao ?? string.Empty);
        Observacao = observacao;
    }
    #endregion

    #region Manipuladores de HistoricoAprendizado
    internal void RegistrarHistoricoAprendizado(Guid aulaId, string nomeAula, byte cargaHoraria, DateTime? dataTermino = null)
    {
        if (!MatriculaCursoDisponivel()) { throw new DomainException("Matrícula não está disponível para registrar histórico de aprendizado"); }

        var historicoExistente = _historicoAprendizado.FirstOrDefault(h => h.CursoId == CursoId && h.AulaId == aulaId);
        DateTime? dataInicio = historicoExistente?.DataInicio ?? DateTime.UtcNow;
        if (historicoExistente != null)
        {
            if (historicoExistente.DataTermino.HasValue) { throw new DomainException("Esta aula já foi concluída"); }

            _historicoAprendizado.Remove(historicoExistente);
        }

        _historicoAprendizado.Add(new HistoricoAprendizado(Id, CursoId, aulaId, nomeAula, cargaHoraria, dataInicio, dataTermino));
    }
    #endregion

    #region Manipuladores de Certificado
    internal void RequisitarCertificadoConclusao(byte notaFinal, string pathCertificado, string nomeInstrutor)
    {
        if (Certificado != null) { throw new DomainException("Certificado já foi solicitado para esta matrícula"); }
        if (!MatriculaCursoConcluido()) { throw new DomainException("Certificado só pode ser solicitado após a conclusão do curso"); }

        Certificado = new Certificado(Id, NomeCurso, DateTime.UtcNow, null, QuantidadeTotalCargaHoraria(), notaFinal, pathCertificado, nomeInstrutor);
    }

    internal void ComunicarDataEmissaoCertificado(DateTime dataEmissao)
    {
        VerificarSeCertificadoExiste();
        if (Certificado.DataEmissao.HasValue) { throw new DomainException("Data de emissão do certificado já foi informada"); }

        Certificado.AtualizarDataEmissao(dataEmissao);
    }

    internal void AtualizarCargaHoraria(short cargaHoraria)
    {
        VerificarSeCertificadoExiste();
        Certificado.AtualizarCargaHoraria(cargaHoraria);
    }

    internal void AtualizarPathCertificado(string path)
    {
        VerificarSeCertificadoExiste();
        Certificado.AtualizarPathCertificado(path ?? string.Empty);
    }

    internal void AtualizarNomeInstrutor(string nomeInstrutor)
    {
        VerificarSeCertificadoExiste();
        Certificado.AtualizarNomeInstrutor(nomeInstrutor ?? string.Empty);
    }
    private void VerificarSeCertificadoExiste()
    {
        if (Certificado == null) { throw new DomainException("Certificado não foi solicitado para esta matrícula"); }
    }
    #endregion

    private void ValidarIntegridadeMatriculaCurso(int? novaNotaFinal = null,
        DateTime? novaDataConclusao = null,
        EstadoMatriculaCursoEnum? novoEstadoMatriculaCurso = null, 
        string novaObservacao = null)
    {
        novaDataConclusao ??= DataConclusao;
        novaObservacao ??= Observacao;

        var validacao = new ResultadoValidacao<MatriculaCurso>();
        ValidacaoGuid.DeveSerValido(AlunoId, "Aluno deve ser informado", validacao);
        ValidacaoGuid.DeveSerValido(CursoId, "Curso deve ser informado", validacao);
        ValidacaoTexto.DevePossuirConteudo(NomeCurso, "Nome do curso deve ser informado", validacao);
        ValidacaoTexto.DevePossuirTamanho(NomeCurso, 10, 200, "Nome do curso deve ter entre 10 e 200 caracteres", validacao);
        ValidacaoNumerica.DeveSerMaiorQueZero(Valor, "Valor da matrícula deve ser maior que zero", validacao);
        ValidacaoData.DeveSerValido(DataMatricula, "Data da matrícula é inválida", validacao);

        if (novaNotaFinal.HasValue)
        {
            ValidacaoNumerica.DeveEstarEntre(novaNotaFinal.Value, 0, 10, "Nota final deve estar entre 0 e 10", validacao);
        }

        ValidarConclusaoCurso(novaDataConclusao, validacao);
        ValidarEstadoParaAbandono(novoEstadoMatriculaCurso, novaDataConclusao, validacao);

        if (!string.IsNullOrWhiteSpace(novaObservacao))
        {
            ValidacaoTexto.DevePossuirTamanho(novaObservacao, 0, 2000, "Observações devem ter no máximo 2000 caracteres", validacao);
        }

        validacao.DispararExcecaoDominioSeInvalido();
    }

    private void ValidarConclusaoCurso(DateTime? dataConclusao, ResultadoValidacao<MatriculaCurso> validacao)
    {
        if (dataConclusao.HasValue)
        {
            switch (EstadoMatricula)
            {
                case EstadoMatriculaCursoEnum.PagamentoRealizado:
                    ValidacaoData.DeveSerValido(dataConclusao.Value, "Data de conclusão deve ser informada", validacao);
                    ValidacaoData.DeveTerRangeValido(DataMatricula, dataConclusao.Value, "Data de conclusão não pode ser anterior a data de matrícula", validacao);
                    break;
                case EstadoMatriculaCursoEnum.PendentePagamento:
                case EstadoMatriculaCursoEnum.Abandonado:
                    validacao.AdicionarErro($"Não é possível concluir um curso com estado de pagamento {EstadoMatricula.GetDescription()}");
                    break;
            }
        }
    }

    private void ValidarEstadoParaAbandono(EstadoMatriculaCursoEnum? novoEstado, DateTime? dataConclusao, ResultadoValidacao<MatriculaCurso> validacao)
    {
        if (novoEstado.HasValue && novoEstado == EstadoMatriculaCursoEnum.Abandonado && dataConclusao.HasValue)
        {
            validacao.AdicionarErro("Não é possível alterar o estado da matrícula para pagamento abandonado com o curso concluído");
        }
    }
    #endregion

    public override string ToString()
    {
        string concluido = MatriculaCursoConcluido() ? "Sim" : "Não";
        return $"Matrícula no curso {CursoId} do aluno {AlunoId} (Concluído? {concluido})";
    }
}