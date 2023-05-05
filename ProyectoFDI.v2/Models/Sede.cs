namespace ProyectoFDI.v2.Models;

public partial class Sede
{
    public int IdSede { get; set; }

    public string? NombreSede { get; set; }

    public virtual ICollection<Competencium> Competencia { get; } = new List<Competencium>();
}
