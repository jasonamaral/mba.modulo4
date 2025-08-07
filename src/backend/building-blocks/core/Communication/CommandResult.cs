using FluentValidation.Results;

namespace Core.Communication;

public class CommandResult
{
    public bool IsValid => ValidationResult.IsValid;
    public object? Data { get; set; }
    private ValidationResult ValidationResult { get; }

    public CommandResult(ValidationResult validationResult, object? data = null)
    {
        ValidationResult = validationResult;
        Data = data;
    }

    public void AdicionarErro(string mensagem)
    {
        ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));
    }

    public IEnumerable<string> ObterErros() =>
        ValidationResult.Errors.Select(e => e.ErrorMessage);

    public ValidationResult ObterValidationResult() => ValidationResult;
}
