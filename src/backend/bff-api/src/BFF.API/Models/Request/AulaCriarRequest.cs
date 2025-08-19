using System.ComponentModel.DataAnnotations;

namespace BFF.API.Models.Request
{
    public class AulaCriarRequest
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Descrição é obrigatória")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ordem é obrigatória")]
        public int Ordem { get; set; }

        [Required(ErrorMessage = "Duração é obrigatória")]
        public int DuracaoMinutos { get; set; }

        [Required(ErrorMessage = "URL do vídeo é obrigatória")]
        public string VideoUrl { get; set; } = string.Empty;
    }
}
