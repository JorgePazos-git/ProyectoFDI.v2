using System;
using System.Collections.Generic;

namespace ProyectoFDI.v2.Models;

public partial class Categorium
{
    public int IdCat { get; set; }

    public string? NombreCat { get; set; }

    public virtual ICollection<Competencium> Competencia { get; } = new List<Competencium>();

    public virtual ICollection<Deportistum> Deportista { get; } = new List<Deportistum>();
}
