using System;
using System.Collections.Generic;

namespace ProyectoFDI.v2.Models;

public partial class VistaVeloClasificacion
{
    public int IdCompe { get; set; }

    public int? Puesto { get; set; }

    public string ResultadoClasificacion { get; set; }

    public string Deportista { get; set; }
}
