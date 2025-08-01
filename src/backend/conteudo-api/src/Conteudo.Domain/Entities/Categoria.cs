using Core.DomainObjects;

namespace Conteudo.Domain.Entities;

public class Categoria : Entidade, IRaizAgregacao
{
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public string Cor { get; private set; }
    public string IconeUrl { get; private set; }
    public bool IsAtiva { get; private set; }
    public int Ordem { get; private set; }
    
    private readonly List<Curso> _cursos = [];
    public IReadOnlyCollection<Curso> Cursos => _cursos.AsReadOnly();

    protected Categoria() { }

    public Categoria(
        string nome,
        string descricao,
        string cor,
        string iconeUrl = "",
        int ordem = 0)
    {
        ValidarDados(nome, descricao, cor);
        
        Nome = nome;
        Descricao = descricao;
        Cor = cor;
        IconeUrl = iconeUrl;
        Ordem = ordem;
        IsAtiva = true;
    }

    private static void ValidarDados(string nome, string descricao, string cor)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome da categoria é obrigatório", nameof(nome));
            
        if (nome.Length > 100)
            throw new ArgumentException("Nome da categoria não pode ter mais de 100 caracteres", nameof(nome));
            
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição da categoria é obrigatória", nameof(descricao));
            
        if (string.IsNullOrWhiteSpace(cor))
            throw new ArgumentException("Cor da categoria é obrigatória", nameof(cor));
    }

    public void AtualizarInformacoes(
        string nome,
        string descricao,
        string cor,
        string iconeUrl = "",
        int ordem = 0)
    {
        ValidarDados(nome, descricao, cor);
        
        Nome = nome;
        Descricao = descricao;
        Cor = cor;
        IconeUrl = iconeUrl;
        Ordem = ordem;
        
        AtualizarDataModificacao();
    }

    public void Ativar()
    {
        IsAtiva = true;
        AtualizarDataModificacao();
    }

    public void Desativar()
    {
        if (_cursos.Any(c => c.Ativo))
            throw new InvalidOperationException("Não é possível desativar categoria com cursos ativos");
            
        IsAtiva = false;
        AtualizarDataModificacao();
    }

    public void AlterarOrdem(int novaOrdem)
    {
        if (novaOrdem < 0)
            throw new ArgumentException("Ordem não pode ser negativa", nameof(novaOrdem));
            
        Ordem = novaOrdem;
        AtualizarDataModificacao();
    }

    public int TotalCursos => _cursos.Count;
    public int CursosAtivos => _cursos.Count(c => c.Ativo);
} 