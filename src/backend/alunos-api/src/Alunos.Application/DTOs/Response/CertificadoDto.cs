namespace Alunos.Application.DTOs.Response;
public class CertificadoDto
{
    public Guid Id { get; set; }
    public Guid MatriculaCursoId { get; set; }
    public string NomeCurso { get; set; }
    public DateTime DataSolicitacao { get; set; }
    public DateTime? DataEmissao { get; set; }
    public short CargaHoraria { get; set; }
    public decimal? NotaFinal { get; set; }
    public string PathCertificado { get; set; }
    public string NomeInstrutor { get; set; }
}