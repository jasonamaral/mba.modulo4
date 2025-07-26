namespace BFF.Domain.DTOs;

public class EstatisticasUsuariosDto
{
    public int TotalUsuarios { get; set; }
    public int UsuariosAtivos { get; set; }
    public int UsuariosOnline { get; set; }
    public int AdminsAtivos { get; set; }
    public int AlunosAtivos { get; set; }
}
