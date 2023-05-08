using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using ProyectoFDI.v2.Code;
using ProyectoFDI.v2.Models;

namespace ProyectoFDI.v2.Controllers
{
    public class BloqueController : Controller
    {
        private string apiUrl;
        public int idCompetencia;
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
            this.idCompetencia = competencia;
            ViewBag.idcompetenciav = competencia;
            ViewBag.ListadoDeportistas = listadoDeportista();
            ViewBag.ListadoDeportistaCompetencia = ListadoDeportistaCompetencia();
            Console.WriteLine(idCompetencia);
            return View();
        }

        public ActionResult AgregarResultadoDeportistaClasificacion(Competencium competencia)
        {
            try
            {
                
                APIConsumer<Competencium>.Insert(apiUrl, competencia);

                return RedirectToAction("Index", new { competencia = competencia.IdCom });

                //return RedirectToAction(nameof(Index), new RouteValueDictionary { { "competencia", idCompetencia } });

               // return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(competencia);
            }
        }
        public void PuestoDeportista(CompetenciaBloqueClasifica depor_comparacion)
        {
            List<CompetenciaBloqueClasifica> list = ListadoDeportistaCompetencia();
            Console.WriteLine(list.Count);
            if (ListadoDeportistaCompetencia().Count == 0)
            {
                depor_comparacion.Puesto = 1;
            }
            else
            {
                foreach(CompetenciaBloqueClasifica depo in list)
                {
                    if (depor_comparacion.TopCla > depo.TopCla)
                    {
                        depor_comparacion.Puesto = depo.TopCla;

                    }
                    else
                    {
                        
                    }
                }
            }
        
        }
        public ActionResult Create(CompetenciaBloqueClasifica competenciablo)
        {
           
            try
            {
                PuestoDeportista(competenciablo);
                APIConsumer<CompetenciaBloqueClasifica>.Insert(apiUrl, competenciablo);
                return RedirectToAction("Index", new { competencia = competenciablo.IdCom });

               // return RedirectToAction(nameof(Index), new RouteValueDictionary { { "competencia", competencia } });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(competenciablo);
            }
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

        private List<CompetenciaBloqueClasifica> ListadoDeportistaCompetencia()
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

           lista.Sort((c1, c2) => Comparer<int>.Default.Compare((int)c1.Puesto, (int)c2.Puesto));
            return lista;
        }


    }


}
