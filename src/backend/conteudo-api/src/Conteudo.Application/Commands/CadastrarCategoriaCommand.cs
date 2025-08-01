using Core.Communication;
using Core.Messages;
using FluentValidation;

namespace Conteudo.Application.Commands
{
    public class CadastrarCategoriaCommand : Command
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Cor { get; set; } = string.Empty;
        public string IconeUrl { get; set; } = string.Empty;
        public int Ordem { get; set; }

        public override bool EhValido()
        {
            CommandResult = new CommandResult(new CadastrarCategoriaCommandValidator().Validate(this));
            return CommandResult.Success;
        }
    }
    public class CadastrarCategoriaCommandValidator : AbstractValidator<CadastrarCategoriaCommand>
    {
        public CadastrarCategoriaCommandValidator()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");
            RuleFor(c => c.Descricao)
                .NotEmpty().WithMessage("Descrição é obrigatória");
            RuleFor(c => c.Cor)
                .NotEmpty().WithMessage("Cor é obrigatória")
                .MaximumLength(100).WithMessage("Cor deve ter no máximo 100 caracteres");
            RuleFor(c => c.IconeUrl)
                .MaximumLength(500).WithMessage("URL do ícone deve ter no máximo 500 caracteres");
        }
    }
}
