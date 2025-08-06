using Core.Communication;
using Core.Messages;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Alunos.Application.Commands;

public class RegistrarClienteCommand : CommandRaiz, IRequest<CommandResult>
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string Cpf { get; private set; }
    public DateTime DataNascimento { get; private set; }
    public string Telefone { get; private set; }
    public string Genero { get; private set; }
    public string Cidade { get; private set; }
    public string Estado { get; private set; }
    public string Cep { get; private set; }
    public string? Foto { get; private set; }
    public bool EhAdministrador { get; private set; }
    public DateTime DataCadastro { get; private set; }

    public ValidationResult ValidationResult { get; set; } = new ValidationResult();

    public RegistrarClienteCommand(
        Guid id,
        string nome,
        string email,
        string cpf,
        DateTime dataNascimento,
        string telefone,
        string genero,
        string cidade,
        string estado,
        string cep,
        string? foto,
        bool ehAdministrador,
        DateTime dataCadastro)
    {
        Id = id;
        Nome = nome;
        Email = email;
        Cpf = cpf;
        DataNascimento = dataNascimento;
        Telefone = telefone;
        Genero = genero;
        Cidade = cidade;
        Estado = estado;
        Cep = cep;
        Foto = foto;
        EhAdministrador = ehAdministrador;
        DataCadastro = dataCadastro;
    }

    public override bool EhValido()
    {
        ValidationResult = new RegistrarClienteCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}

public class RegistrarClienteCommandValidation : AbstractValidator<RegistrarClienteCommand>
{
    public RegistrarClienteCommandValidation()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Id do usuário é obrigatório");

        RuleFor(c => c.Nome)
            .NotEmpty()
            .WithMessage("Nome é obrigatório")
            .Length(2, 100)
            .WithMessage("Nome deve ter entre 2 e 100 caracteres");

        RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("Email é obrigatório")
            .EmailAddress()
            .WithMessage("Email deve ter formato válido")
            .MaximumLength(200)
            .WithMessage("Email deve ter no máximo 200 caracteres");

        RuleFor(c => c.Cpf)
            .NotEmpty()
            .WithMessage("CPF é obrigatório")
            .Length(11, 14)
            .WithMessage("CPF deve ter entre 11 e 14 caracteres");

        RuleFor(c => c.DataNascimento)
            .NotEmpty()
            .WithMessage("Data de nascimento é obrigatória")
            .LessThan(DateTime.Today)
            .WithMessage("Data de nascimento deve ser anterior a hoje");

        RuleFor(c => c.Telefone)
            .MaximumLength(20)
            .WithMessage("Telefone deve ter no máximo 20 caracteres");

        RuleFor(c => c.Genero)
            .MaximumLength(20)
            .WithMessage("Gênero deve ter no máximo 20 caracteres");

        RuleFor(c => c.Cidade)
            .MaximumLength(100)
            .WithMessage("Cidade deve ter no máximo 100 caracteres");

        RuleFor(c => c.Estado)
            .MaximumLength(50)
            .WithMessage("Estado deve ter no máximo 50 caracteres");

        RuleFor(c => c.Cep)
            .MaximumLength(10)
            .WithMessage("CEP deve ter no máximo 10 caracteres");

        RuleFor(c => c.Foto)
            .MaximumLength(500)
            .WithMessage("URL da foto deve ter no máximo 500 caracteres");

        RuleFor(c => c.DataCadastro)
            .NotEmpty()
            .WithMessage("Data de cadastro é obrigatória");
    }
}