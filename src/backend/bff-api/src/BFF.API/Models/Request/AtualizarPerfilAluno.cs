namespace BFF.API.Models.Request;

public class AtualizarPerfilAluno
{
    public string Telefone { get; set; } = string.Empty;
    public string Genero { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string CEP { get; set; } = string.Empty;
}
