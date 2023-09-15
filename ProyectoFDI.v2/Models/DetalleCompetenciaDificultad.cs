using System;
using System.Collections.Generic;

namespace ProyectoFDI.v2.Models;

public partial class DetalleCompetenciaDificultad
{
    public int IdDetalleDificultad { get; set; }

    public int? Puesto { get; set; }

    public string Clas1Res { get; set; }

    public string Clas2Res { get; set; }

    public int? PuestoInicialRes { get; set; }

    public string FinalRes { get; set; }

    public int? IdDep { get; set; }

    public int? IdCom { get; set; }

    public virtual Competencium IdComNavigation { get; set; }

    public virtual Deportistum IdDepNavigation { get; set; }
}
