using System.ComponentModel.DataAnnotations;

namespace Auth.API.Models.Requests;

public class RegistroRequest : IValidatableObject
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Senha deve ter entre 6 e 100 caracteres")]
    public string Senha { get; set; } = string.Empty;

    [Required(ErrorMessage = "Data de nascimento é obrigatória")]
    public DateTime DataNascimento { get; set; }

    public bool EhAdministrador { get; set; } = false;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var idade = CalcularIdade(DataNascimento);
        if (idade < 16 || idade > 100)
        {
            yield return new ValidationResult("Idade deve estar entre 16 e 100 anos.", new[] { nameof(DataNascimento) });
        }
    }

    private int CalcularIdade(DateTime nascimento)
    {
        var hoje = DateTime.Today;
        var idade = hoje.Year - nascimento.Year;
        if (nascimento.Date > hoje.AddYears(-idade)) idade--;
        return idade;
    }
}