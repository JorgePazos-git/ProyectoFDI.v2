using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoFDI.v2.Models;

public partial class VistaPuntajesDeportista
{
    [Key]
    public long  IdVw { get; set; } 

    public int? IdCom { get; set; }

    public int? IdDep { get; set; }

    public string NombreDep { get; set; } = null!;

    public string? Etapa { get; set; }

    public int? IntentosTops { get; set; }

    public int? TopsRealizados { get; set; }

    public int? IntentosZonas { get; set; }

    public int? ZonasRealizadas { get; set; }
}
