namespace ProyectoFDI.v2.Models
{
    public class CompetenciaBloqueClasifica
    {
        public int IdCompeBloqueCla { get; set; }

        public int? ZonaCla { get; set; }

        public int? ZonaIntentosCla { get; set; }

        public int? TopCla { get; set; }

        public int? TopIntentosCla { get; set; }

        public int? Puesto { get; set; }

        public bool? ClasiBloque { get; set; }

        public int? IdDep { get; set; }

        public int? IdCom { get; set; }

        public virtual ICollection<CompetenciaBloqueFinal> CompetenciaBloqueFinals { get; } = new List<CompetenciaBloqueFinal>();

        public virtual Competencium? IdComNavigation { get; set; }

        public virtual Deportistum? IdDepNavigation { get; set; }
    }
}
