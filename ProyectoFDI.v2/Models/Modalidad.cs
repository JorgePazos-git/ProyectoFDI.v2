using System;
using System.Collections.Generic;

namespace ProyectoFDI.v2.Models;

public partial class Modalidad
{
    public int IdMod { get; set; }

    public string DescripcionMod { get; set; }

    public virtual ICollection<Competencium> Competencia { get; } = new List<Competencium>();

    public virtual ICollection<DeportistaModalidad> DeportistaModalidads { get; } = new List<DeportistaModalidad>();
}
