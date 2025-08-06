namespace Alunos.Infrastructure.Services;
public class RegistrarUsuarioEvent
{
    public RegistrarUsuarioEvent(Guid codigoUsuarioAutenticacao, string nome, string email, string cpf, DateTime dataNascimento, string contato, bool ehAdministrador)
    {
        CodigoUsuarioAutenticacao = codigoUsuarioAutenticacao;
        Nome = nome;
        Email = email;
        Cpf = cpf;
        DataNascimento = dataNascimento;
        Contato = contato;
        Ativo = true;
        DataCriacao = DateTime.UtcNow;
        EhAdministrador = ehAdministrador;
    }

    public Guid CodigoUsuarioAutenticacao { get; }
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string Cpf { get; }
    public DateTime DataNascimento { get; private set; }
    public string Contato { get; private set; }
    public bool Ativo { get; private set; }
    public DateTime DataCriacao { get; set; }
    public bool EhAdministrador { get; private set; }
}