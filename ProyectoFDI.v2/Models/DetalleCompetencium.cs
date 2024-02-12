using System;
using System.Collections.Generic;

namespace ProyectoFDI.v2.Models;

public partial class DetalleCompetencium
{
    public int IdDetalle { get; set; }

    public int? Puesto { get; set; }

    public string? ClasRes { get; set; }

    public string? OctavosRes { get; set; }

    public string? CuartosRes { get; set; }

    public string? SemiRes { get; set; }

    public string? FinalRes { get; set; }

    public int? IdDep { get; set; }

    public int? IdCom { get; set; }

    public virtual Competencium? IdComNavigation { get; set; }

    public virtual Deportistum? IdDepNavigation { get; set; }
}
