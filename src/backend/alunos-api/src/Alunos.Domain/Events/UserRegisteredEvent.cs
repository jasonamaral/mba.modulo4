namespace Alunos.Domain.Events;

/// <summary>
/// Evento recebido quando um usuário é registrado na Auth API
/// </summary>
public class UserRegisteredEvent
{
    public UserRegisteredEvent(Guid userId, string email, string nome, DateTime dataNascimento, bool ehAdministrador, DateTime dataCadastro)
    {
        UserId = userId;
        Email = email;
        Nome = nome;
        DataNascimento = dataNascimento;
        EhAdministrador = ehAdministrador;
        DataCadastro = dataCadastro;
    }

    /// <summary>
    /// ID do usuário no sistema de autenticação
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Email do usuário
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Nome completo do usuário
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Data de nascimento
    /// </summary>
    public DateTime DataNascimento { get; set; }

    /// <summary>
    /// Se é administrador (não cria perfil de aluno)
    /// </summary>
    public bool EhAdministrador { get; set; }

    /// <summary>
    /// Data do cadastro
    /// </summary>
    public DateTime DataCadastro { get; set; }
} 