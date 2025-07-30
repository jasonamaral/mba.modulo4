namespace Alunos.Application.DTOs;

public class CertificadoArquivoDto
{
    public byte[] Conteudo { get; set; } = Array.Empty<byte>();

    public string ContentType { get; set; } = string.Empty;

    public string NomeArquivo { get; set; } = string.Empty;
}