using System.ComponentModel.DataAnnotations;

namespace Conteudo.Application.DTOs;

public class CadastroMaterialDto
{
    [Required(ErrorMessage = "ID da aula é obrigatório")]
    public Guid AulaId { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Descrição é obrigatória")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tipo do material é obrigatório")]
    [StringLength(50, ErrorMessage = "Tipo do material deve ter no máximo 50 caracteres")]
    public string TipoMaterial { get; set; } = string.Empty;

    [Required(ErrorMessage = "URL é obrigatória")]
    [StringLength(500, ErrorMessage = "URL deve ter no máximo 500 caracteres")]
    public string Url { get; set; } = string.Empty;

    public bool IsObrigatorio { get; set; } = false;
    public long TamanhoBytes { get; set; } = 0;
    public string Extensao { get; set; } = string.Empty;
    public int Ordem { get; set; } = 0;
}

public class AtualizarMaterialDto
{
    [Required(ErrorMessage = "ID é obrigatório")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Descrição é obrigatória")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tipo do material é obrigatório")]
    [StringLength(50, ErrorMessage = "Tipo do material deve ter no máximo 50 caracteres")]
    public string TipoMaterial { get; set; } = string.Empty;

    [Required(ErrorMessage = "URL é obrigatória")]
    [StringLength(500, ErrorMessage = "URL deve ter no máximo 500 caracteres")]
    public string Url { get; set; } = string.Empty;

    public bool IsObrigatorio { get; set; } = false;
    public long TamanhoBytes { get; set; } = 0;
    public string Extensao { get; set; } = string.Empty;
    public int Ordem { get; set; } = 0;
}

public class MaterialDto
{
    public Guid Id { get; set; }
    public Guid AulaId { get; set; }
    public string NomeAula { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string TipoMaterial { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public bool IsObrigatorio { get; set; }
    public long TamanhoBytes { get; set; }
    public string TamanhoFormatado { get; set; } = string.Empty;
    public string Extensao { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public bool IsAtivo { get; set; }
    public bool EhArquivo { get; set; }
    public bool EhLink { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
} 