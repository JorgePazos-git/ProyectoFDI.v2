using System;
using System.Collections.Generic;

namespace ProyectoFDI.API.v2.Models;
using System.ComponentModel.DataAnnotations;

public partial class VistaCompetencium
{
    [Key]
    public int IdCom { get; set; }

    public string? NombreCom { get; set; }

    public DateTime? FechaInicioCom { get; set; }

    public DateTime? FechaFinCom { get; set; }

    public bool? ActivoCom { get; set; }

    public string? Genero { get; set; }

    public string? NombreDelJuez { get; set; }

    public string? NombreCategoria { get; set; }

    public string? DescripcionModalidad { get; set; }

    public string? NombreDeSede { get; set; }
}
