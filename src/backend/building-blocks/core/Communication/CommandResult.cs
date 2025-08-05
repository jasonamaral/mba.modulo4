namespace Core.Communication
{
    public class CommandResult
    {
        public bool Success => Data != null;

        public object Data { get; set; }
        //public bool Success => ValidationResult.IsValid;

        //public CommandResult(ValidationResult validationResult, object data = null)
        //{
        //    ValidationResult = validationResult;
        //    Data = data;
        //}

        //public void AdicionarErro(string propName, string mensagem)
        //{
        //    ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));
        //}

        //public List<ValidationFailure> ObterErros()
        //{
        //    return ValidationResult.Errors;
        //}

        //public ValidationResult ObterValidationResult()
        //{
        //    return ValidationResult;
        //}
    }
}
