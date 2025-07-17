namespace Conteudo.Domain.Events;

public class MatriculaRealizadaEvent
{
    public string EventId { get; set; } = Guid.NewGuid().ToString();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string EventType { get; set; } = "matricula.realizada";
    public MatriculaRealizadaData Data { get; set; }

    public MatriculaRealizadaEvent(MatriculaRealizadaData data)
    {
        Data = data;
    }
}

public class MatriculaRealizadaData
{
    public Guid AlunoId { get; set; }
    public Guid CursoId { get; set; }
    public string NomeAluno { get; set; } = string.Empty;
    public string NomeCurso { get; set; } = string.Empty;
    public string EmailAluno { get; set; } = string.Empty;
    public decimal ValorPago { get; set; }
    public DateTime DataMatricula { get; set; }
    public string StatusMatricula { get; set; } = "Ativa";
} 