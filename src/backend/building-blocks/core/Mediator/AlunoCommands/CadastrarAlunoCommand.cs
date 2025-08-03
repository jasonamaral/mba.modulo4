using Core.Messages;

namespace Core.Mediator.AlunoCommands;
public class CadastrarAlunoCommand : CommandRaiz
{
    public Guid AlunoId { get; private set; }
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string Cpf { get; private set; }
    public DateTime DataNascimento { get; private set; }

    public CadastrarAlunoCommand(Guid alunoId, string nome, string email, string cpf, DateTime dataNascimento)
    {
        DefinirRaizAgregacao(alunoId);
        AlunoId = alunoId;
        Nome = nome;
        Email = email;
        Cpf = cpf;
        DataNascimento = dataNascimento;
    }
}
