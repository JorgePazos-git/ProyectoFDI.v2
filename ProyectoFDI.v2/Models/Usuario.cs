using System;
using System.Collections.Generic;

namespace ProyectoFDI.v2.Models;

public partial class Usuario
{
    public int IdUsu { get; set; }

    public string NombreUsu { get; set; }

    public string ClaveUsu { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public string RolesUsu { get; set; }

    public bool? ActivoUsu { get; set; }

    public virtual ICollection<Deportistum> Deportista { get; } = new List<Deportistum>();

    public virtual ICollection<Entrenador> Entrenadors { get; } = new List<Entrenador>();

    public virtual ICollection<Juez> Juezs { get; } = new List<Juez>();
}
