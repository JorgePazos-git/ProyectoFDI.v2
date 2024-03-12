using System;
using System.Collections.Generic;

namespace ProyectoFDI.v2.Models;

public partial class ResultadoBloque
{
    public int IdResBloque { get; set; }

    public int? IdCom { get; set; }

    public int? IdDep { get; set; }

    public int? Puesto { get; set; }

    public string Etapa { get; set; }

    public virtual Competencium IdComNavigation { get; set; }

    public virtual Deportistum IdDepNavigation { get; set; }
}
