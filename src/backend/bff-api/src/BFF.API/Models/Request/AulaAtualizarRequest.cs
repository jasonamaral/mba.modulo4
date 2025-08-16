using System.ComponentModel.DataAnnotations;

namespace BFF.API.Models.Request
{
    public class AulaAtualizarRequest : AulaCriarRequest
    {
        [Required(ErrorMessage = "ID da aula é obrigatório")]
        public Guid Id { get; set; }
    }
}
