namespace Alunos.Application.DTOs.Review;

public class CertificadoValidacaoResultadoDto
{
    public bool EhValido { get; set; }

    public string Mensagem { get; set; } = string.Empty;

    public CertificadoDto? Certificado { get; set; }

    public DateTime DataValidacao { get; set; }
}