using System.ComponentModel.DataAnnotations;

namespace ProyectoFDI.v2.Models;

public partial class Deportistum
{
    public int IdDep { get; set; }

    [Required(ErrorMessage = "Debe ingresar el Nombre del Deportista")]
    [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = "El Nombre es muy corto")]
    public string? NombresDep { get; set; }

    [Required(ErrorMessage = "Debe ingresar el Apellido del Deportista")]
    [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = "El Apellido es muy corto")]
    public string? ApellidosDep { get; set; }

    [Required(ErrorMessage = "Ingrese la Cédula")]
    [StringLength(maximumLength: 10, MinimumLength = 10, ErrorMessage = "Cédula Inválida")]
    public string? CedulaDep { get; set; }
    public bool? ActivoDep { get; set; }

    public int? IdPro { get; set; }

    public int? IdUsu { get; set; }

    public int? IdCat { get; set; }

    public int? IdGen { get; set; }

    public int? IdClub { get; set; }

    public int? IdEnt { get; set; }

    public virtual ICollection<DeportistaModalidad> DeportistaModalidads { get; } = new List<DeportistaModalidad>();

    public virtual ICollection<DetalleCompetencium> DetalleCompetencia { get; } = new List<DetalleCompetencium>();

    public virtual Categorium? IdCatNavigation { get; set; }

    public virtual Club? IdClubNavigation { get; set; }

    public virtual Entrenador? IdEntNavigation { get; set; }

    public virtual Genero? IdGenNavigation { get; set; }

    public virtual Provincium? IdProNavigation { get; set; }

    public virtual Usuario? IdUsuNavigation { get; set; }
}
