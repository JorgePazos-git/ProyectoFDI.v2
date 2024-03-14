using System;
using System.Collections.Generic;

namespace ProyectoFDI.v2.Models;

public partial class VistaVeloFinal
{
    public int IdCompe { get; set; }

    public int? Puesto { get; set; }

    public string Deportista { get; set; }

    public string ResultadoClasificacion { get; set; }

    public string ResultadoOctavos { get; set; }

    public string ResultadoCuartos { get; set; }

    public string ResultadoSemifinal { get; set; }

    public string ResultadoFinal { get; set; }
}
