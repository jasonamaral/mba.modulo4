using Conteudo.Domain.Common;
using Conteudo.Domain.ValueObjects;

namespace Conteudo.Domain.Entities;

public class Curso : Entidade, IRaizAgregacao
{
    public string Nome { get; private set; }
    public decimal Valor { get; private set; }
    public bool Ativo { get; private set; }
    public DateTime? ValidoAte { get; private set; }
    public ConteudoProgramatico ConteudoProgramatico { get; private set; }
    public Guid? CategoriaId { get; private set; }
    public Categoria? Categoria { get; private set; }
    public int DuracaoHoras { get; private set; }
    public string Nivel { get; private set; }
    public string ImagemUrl { get; private set; }
    public string Instrutor { get; private set; }
    public int VagasMaximas { get; private set; }
    public int VagasOcupadas { get; private set; }
    
    private readonly List<Aula> _aulas = [];
    public IReadOnlyCollection<Aula> Aulas => _aulas.AsReadOnly();

    protected Curso() { }

    public Curso(
        string nome,
        decimal valor,
        ConteudoProgramatico conteudoProgramatico,
        int duracaoHoras,
        string nivel,
        string instrutor,
        int vagasMaximas,
        string imagemUrl = "",
        DateTime? validoAte = null,
        Guid? categoriaId = null)
    {
        ValidarDados(nome, valor, conteudoProgramatico, duracaoHoras, nivel, instrutor, vagasMaximas);
        
        Nome = nome;
        Valor = valor;
        ConteudoProgramatico = conteudoProgramatico;
        DuracaoHoras = duracaoHoras;
        Nivel = nivel;
        Instrutor = instrutor;
        VagasMaximas = vagasMaximas;
        VagasOcupadas = 0;
        ImagemUrl = imagemUrl;
        ValidoAte = validoAte;
        CategoriaId = categoriaId;
        Ativo = true;
    }

    private static void ValidarDados(string nome, decimal valor, ConteudoProgramatico conteudoProgramatico, 
        int duracaoHoras, string nivel, string instrutor, int vagasMaximas)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do curso é obrigatório", nameof(nome));
            
        if (nome.Length > 200)
            throw new ArgumentException("Nome do curso não pode ter mais de 200 caracteres", nameof(nome));
            
        if (valor < 0)
            throw new ArgumentException("Valor do curso não pode ser negativo", nameof(valor));
            
        if (conteudoProgramatico == null)
            throw new ArgumentException("Conteúdo programático é obrigatório", nameof(conteudoProgramatico));
            
        if (duracaoHoras <= 0)
            throw new ArgumentException("Duração do curso deve ser maior que zero", nameof(duracaoHoras));
            
        if (string.IsNullOrWhiteSpace(nivel))
            throw new ArgumentException("Nível do curso é obrigatório", nameof(nivel));
            
        if (string.IsNullOrWhiteSpace(instrutor))
            throw new ArgumentException("Instrutor é obrigatório", nameof(instrutor));
            
        if (vagasMaximas <= 0)
            throw new ArgumentException("Número de vagas deve ser maior que zero", nameof(vagasMaximas));
    }

    public void AtualizarInformacoes(
        string nome,
        decimal valor,
        ConteudoProgramatico conteudoProgramatico,
        int duracaoHoras,
        string nivel,
        string instrutor,
        int vagasMaximas,
        string imagemUrl = "",
        DateTime? validoAte = null,
        Guid? categoriaId = null)
    {
        ValidarDados(nome, valor, conteudoProgramatico, duracaoHoras, nivel, instrutor, vagasMaximas);
        
        Nome = nome;
        Valor = valor;
        ConteudoProgramatico = conteudoProgramatico;
        DuracaoHoras = duracaoHoras;
        Nivel = nivel;
        Instrutor = instrutor;
        VagasMaximas = vagasMaximas;
        ImagemUrl = imagemUrl;
        ValidoAte = validoAte;
        CategoriaId = categoriaId;
        
        AtualizarDataModificacao();
    }

    public void Ativar()
    {
        Ativo = true;
        AtualizarDataModificacao();
    }

    public void Desativar()
    {
        Ativo = false;
        AtualizarDataModificacao();
    }

    public void AdicionarAula(Aula aula)
    {
        if (aula == null)
            throw new ArgumentException("Aula não pode ser nula", nameof(aula));
            
        if (_aulas.Any(a => a.Numero == aula.Numero))
            throw new InvalidOperationException($"Já existe uma aula com o número {aula.Numero}");
            
        _aulas.Add(aula);
        AtualizarDataModificacao();
    }

    public void RemoverAula(Guid aulaId)
    {
        var aula = _aulas.FirstOrDefault(a => a.Id == aulaId);
        if (aula != null)
        {
            _aulas.Remove(aula);
            AtualizarDataModificacao();
        }
    }

    public void AdicionarMatricula()
    {
        if (VagasOcupadas >= VagasMaximas)
            throw new InvalidOperationException("Não há vagas disponíveis para este curso");
            
        VagasOcupadas++;
        AtualizarDataModificacao();
    }

    public void RemoverMatricula()
    {
        if (VagasOcupadas > 0)
        {
            VagasOcupadas--;
            AtualizarDataModificacao();
        }
    }

    public bool TemVagasDisponiveis => VagasOcupadas < VagasMaximas;
    public int VagasDisponiveis => VagasMaximas - VagasOcupadas;
    public bool EstaExpirado => ValidoAte.HasValue && ValidoAte.Value < DateTime.UtcNow;
    public bool PodeSerMatriculado => Ativo && !EstaExpirado && TemVagasDisponiveis;
} 