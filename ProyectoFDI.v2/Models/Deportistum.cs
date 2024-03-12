using System;
using System.Collections.Generic;

namespace ProyectoFDI.v2.Models;

public partial class Deportistum
{
    public int IdDep { get; set; }

    public string NombresDep { get; set; }

    public string ApellidosDep { get; set; }

    public string CedulaDep { get; set; }

    public bool? ActivoDep { get; set; }

    public int? IdPro { get; set; }

    public int? IdUsu { get; set; }

    public int? IdCat { get; set; }

    public int? IdGen { get; set; }

    public int? IdClub { get; set; }

    public int? IdEnt { get; set; }

    public virtual ICollection<DeportistaModalidad> DeportistaModalidads { get; } = new List<DeportistaModalidad>();

    public virtual ICollection<DetalleCompetencium> DetalleCompetencia { get; } = new List<DetalleCompetencium>();

    public virtual ICollection<DetalleCompetenciaDificultad> DetalleCompetenciaDificultads { get; } = new List<DetalleCompetenciaDificultad>();

    public virtual Categorium IdCatNavigation { get; set; }

    public virtual Club IdClubNavigation { get; set; }

    public virtual Entrenador IdEntNavigation { get; set; }

    public virtual Genero IdGenNavigation { get; set; }

    public virtual Provincium IdProNavigation { get; set; }

    public virtual Usuario IdUsuNavigation { get; set; }

    public virtual ICollection<PuntajeBloque> PuntajeBloques { get; } = new List<PuntajeBloque>();

    public virtual ICollection<ResultadoBloque> ResultadoBloques { get; } = new List<ResultadoBloque>();
}
