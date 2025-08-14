using Core.DomainObjects;

namespace Conteudo.Domain.Entities;

public class Aula : Entidade, IRaizAgregacao
{
    public Guid CursoId { get; private set; }
    public Curso Curso { get; private set; }
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public int Numero { get; private set; }
    public int DuracaoMinutos { get; private set; }
    public string VideoUrl { get; private set; }
    public string TipoAula { get; private set; }
    public bool IsObrigatoria { get; private set; }
    public bool IsPublicada { get; private set; }
    public DateTime? DataPublicacao { get; private set; }
    public string Observacoes { get; private set; }
    
    private readonly List<Material> _materiais = [];
    public IReadOnlyCollection<Material> Materiais => _materiais.AsReadOnly();

    protected Aula() { }

    public Aula(
        Guid cursoId,
        string nome,
        string descricao,
        int numero,
        int duracaoMinutos,
        string videoUrl,
        string tipoAula,
        bool isObrigatoria = true,
        string observacoes = "")
    {
        ValidarDados(nome, descricao, numero, duracaoMinutos, videoUrl, tipoAula);
        
        CursoId = cursoId;
        Nome = nome;
        Descricao = descricao;
        Numero = numero;
        DuracaoMinutos = duracaoMinutos;
        VideoUrl = videoUrl;
        TipoAula = tipoAula;
        IsObrigatoria = isObrigatoria;
        Observacoes = observacoes;
        IsPublicada = false;
    }

    private static void ValidarDados(string nome, string descricao, int numero, int duracaoMinutos, string videoUrl, string tipoAula)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome da aula é obrigatório", nameof(nome));
            
        if (nome.Length > 200)
            throw new ArgumentException("Nome da aula não pode ter mais de 200 caracteres", nameof(nome));
            
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição da aula é obrigatória", nameof(descricao));
            
        if (numero <= 0)
            throw new ArgumentException("Número da aula deve ser maior que zero", nameof(numero));
            
        if (duracaoMinutos <= 0)
            throw new ArgumentException("Duração da aula deve ser maior que zero", nameof(duracaoMinutos));
            
        if (string.IsNullOrWhiteSpace(videoUrl))
            throw new ArgumentException("URL do vídeo é obrigatória", nameof(videoUrl));
            
        if (string.IsNullOrWhiteSpace(tipoAula))
            throw new ArgumentException("Tipo da aula é obrigatório", nameof(tipoAula));
    }

    public void AtualizarInformacoes(
        string nome,
        string descricao,
        int numero,
        int duracaoMinutos,
        string videoUrl,
        string tipoAula,
        bool isObrigatoria,
        string observacoes = "")
    {
        ValidarDados(nome, descricao, numero, duracaoMinutos, videoUrl, tipoAula);
        
        Nome = nome;
        Descricao = descricao;
        Numero = numero;
        DuracaoMinutos = duracaoMinutos;
        VideoUrl = videoUrl;
        TipoAula = tipoAula;
        IsObrigatoria = isObrigatoria;
        Observacoes = observacoes;
        
        AtualizarDataModificacao();
    }

    public void Publicar()
    {
        if (IsPublicada)
            throw new InvalidOperationException("Aula já está publicada");
            
        IsPublicada = true;
        DataPublicacao = DateTime.UtcNow;
        AtualizarDataModificacao();
    }

    public void Despublicar()
    {
        if (!IsPublicada)
            throw new InvalidOperationException("Aula não está publicada");
            
        IsPublicada = false;
        DataPublicacao = null;
        AtualizarDataModificacao();
    }

    public void AdicionarMaterial(Material material)
    {
        if (material == null)
            throw new ArgumentException("Material não pode ser nulo", nameof(material));
            
        if (_materiais.Any(m => m.Nome == material.Nome))
            throw new InvalidOperationException($"Já existe um material com o nome {material.Nome}");
            
        _materiais.Add(material);
        AtualizarDataModificacao();
    }

    public void RemoverMaterial(Guid materialId)
    {
        var material = _materiais.FirstOrDefault(m => m.Id == materialId);
        if (material != null)
        {
            _materiais.Remove(material);
            AtualizarDataModificacao();
        }
    }

    public bool PodeSerVisualizada => IsPublicada;
    public string DuracaoFormatada => $"{DuracaoMinutos / 60:D2}:{DuracaoMinutos % 60:D2}";
} 