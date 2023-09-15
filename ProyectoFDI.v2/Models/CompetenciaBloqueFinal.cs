using System;
using System.Collections.Generic;

namespace ProyectoFDI.v2.Models;

public partial class CompetenciaBloqueFinal
{
    public int IdCompeBloqueFinal { get; set; }

    public int? ZonaCla { get; set; }

    public int? ZonaIntentosCla { get; set; }

    public int? TopCla { get; set; }

    public int? TopIntentosCla { get; set; }

    public int? Puesto { get; set; }

    public int? IdCompeBloqueCla { get; set; }

    public virtual CompetenciaBloqueClasifica IdCompeBloqueClaNavigation { get; set; }
}
