using Core.Messages;
using Core.Communication;
using FluentValidation;

namespace Conteudo.Application.Commands
{
    public class AtualizarCursoCommand : Command
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public int DuracaoHoras { get; set; }
        public string Nivel { get; set; } = string.Empty;
        public string Instrutor { get; set; } = string.Empty;
        public int VagasMaximas { get; set; }
        public string ImagemUrl { get; set; } = string.Empty;
        public DateTime? ValidoAte { get; set; }
        public Guid? CategoriaId { get; set; }
        public string Resumo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Objetivos { get; set; } = string.Empty;
        public string PreRequisitos { get; set; } = string.Empty;
        public string PublicoAlvo { get; set; } = string.Empty;
        public string Metodologia { get; set; } = string.Empty;
        public string Recursos { get; set; } = string.Empty;
        public string Avaliacao { get; set; } = string.Empty;
        public string Bibliografia { get; set; } = string.Empty;

        public override bool EhValido()
        {
            CommandResult = new CommandResult(new AtualizarCursoCommandValidator().Validate(this));
            return CommandResult.Success;
        }
    }
    public class AtualizarCursoCommandValidator : AbstractValidator<AtualizarCursoCommand>
    {
        public AtualizarCursoCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("ID é obrigatório");
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres");
            RuleFor(c => c.Valor)
                .GreaterThanOrEqualTo(0).WithMessage("Valor deve ser maior ou igual a zero");
            RuleFor(c => c.DuracaoHoras)
                .GreaterThan(0).WithMessage("Duração deve ser maior que zero");
            RuleFor(c => c.Nivel)
                .NotEmpty().WithMessage("Nível é obrigatório")
                .MaximumLength(50).WithMessage("Nível deve ter no máximo 50 caracteres");
            RuleFor(c => c.Instrutor)
                .NotEmpty().WithMessage("Instrutor é obrigatório")
                .MaximumLength(100).WithMessage("Instrutor deve ter no máximo 100 caracteres");
            RuleFor(c => c.VagasMaximas)
                .GreaterThan(0).WithMessage("Número de vagas deve ser maior que zero");
            RuleFor(c => c.ImagemUrl)
                .MaximumLength(500).WithMessage("URL da imagem deve ter no máximo 500 caracteres");
            RuleFor(c => c.Resumo)
                .NotEmpty().WithMessage("Resumo é obrigatório");
            RuleFor(c => c.Descricao)
                .NotEmpty().WithMessage("Descrição é obrigatória");
            RuleFor(c => c.Objetivos)
                .NotEmpty().WithMessage("Objetivos são obrigatórios");
        }
    }
}
