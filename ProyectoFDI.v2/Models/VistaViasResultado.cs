using System;
using System.Collections.Generic;

namespace ProyectoFDI.v2.Models;

public partial class VistaViasResultado
{
    public int IdCompe { get; set; }

    public int? PuestoFinal { get; set; }

    public int? PuestoClasificacion { get; set; }

    public string Deportista { get; set; }

    public string Clasificacion1 { get; set; }

    public string Clasificacion2 { get; set; }

    public string Final { get; set; }
}
