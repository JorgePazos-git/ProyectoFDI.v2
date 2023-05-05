using Microsoft.AspNetCore.Mvc;
using ProyectoFDI.v2.Code;
using ProyectoFDI.v2.Models;

namespace ProyectoFDI.v2.Controllers
{
    public class BloqueController : Controller
    {
        private string apiUrl;
        private int idCompetencia;
        public BloqueController(IConfiguration configuration)
        {
            apiUrl = configuration["urlBase"].ToString() + "CompetenciaBloqueClasificas/";
        }
        //public IActionResult Index()
        //{
        //    ViewBag.ListadoDeportistas = listadoDeportista();
        //    ViewBag.ListadoDeportistaCompetencia = listadoDeportistaCompetencia();
        //    return View();
        //}
        public IActionResult Index(int competencia)
        {
            idCompetencia = competencia;
            ViewBag.idcompetencia = idCompetencia;
            ViewBag.ListadoDeportistas = listadoDeportista();
            ViewBag.ListadoDeportistaCompetencia = listadoDeportistaCompetencia();
            return View();
        }
        private List<Deportistum> listadoDeportista()
        {
            var deportista = APIConsumer<Deportistum>.Select(apiUrl.Replace("CompetenciaBloqueClasificas", "Deportista"));
            var lista = deportista.Select(f => new Deportistum
            {
                IdDep = f.IdDep,
                NombresDep = f.NombresDep
            }).ToList();

            return lista;
        }
        [HttpPost]
        public IActionResult SeleccionarDeportista(int deportistaSeleccionado)
        {
            // Hacer algo con el deportista seleccionado
            return RedirectToAction("Index");
        }

        private List<CompetenciaBloqueClasifica> listadoDeportistaCompetencia()
        {
            var competenciaBloque = APIConsumer<CompetenciaBloqueClasifica>.Select(apiUrl.Replace("CompetenciaBloqueClasificas", "CompetenciaBloqueClasificas"));
            var lista = competenciaBloque.Select(f => new CompetenciaBloqueClasifica
            {
                ZonaCla = f.ZonaCla,
                ZonaIntentosCla = f.ZonaIntentosCla,
                TopCla = f.TopCla,
                TopIntentosCla = f.TopIntentosCla,
                Puesto = f.Puesto,
                ClasiBloque = f.ClasiBloque,
                IdDep = f.IdDep,
                IdCom = f.IdCom,

            }).ToList();

            return lista;
        }

    }


}
