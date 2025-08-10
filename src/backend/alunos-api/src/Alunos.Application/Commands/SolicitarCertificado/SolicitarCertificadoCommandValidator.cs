using FluentValidation;

namespace Alunos.Application.Commands.SolicitarCertificado;
public class SolicitarCertificadoCommandValidator : AbstractValidator<SolicitarCertificadoCommand>
{
    public SolicitarCertificadoCommandValidator()
    {
        RuleFor(c => c.AlunoId).NotEqual(Guid.Empty).WithMessage("Id do aluno inválido.");
        RuleFor(c => c.MatriculaCursoId).NotEqual(Guid.Empty).WithMessage("Id da matrícula inválido.");
        //RuleFor(c => c.PathCertificado).NotEmpty().WithMessage("O caminho do certificado é obrigatório.");
        //RuleFor(c => c.NotaFinal).LessThanOrEqualTo((byte)10).WithMessage("Nota final possui valor inválido.");
        //RuleFor(c => c.NomeInstrutor).NotEmpty().WithMessage("Nome do instrutor é obrigatório.");
    }
}
