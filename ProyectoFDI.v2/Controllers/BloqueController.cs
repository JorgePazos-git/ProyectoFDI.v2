using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProyectoFDI.v2.Code;
using ProyectoFDI.v2.Models;
using System.Runtime.Intrinsics.X86;

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

        [Authorize(Roles = "Administrador,Juez")]
        public IActionResult Index(int competencia)
        {
            this.idCompetencia = competencia;
            ViewBag.idcompetenciav = competencia;
            ViewBag.ListadoDeportistas = listadoDeportista(competencia);
            ViewBag.ListadoDeportistaCompetencia = ListadoDeportistaCompetencia(competencia);
            Console.WriteLine(idCompetencia);
            return View();
        }

        [Authorize(Roles = "Administrador,Juez")]
        public IActionResult agregardeportista(int competencia)
        {
            Console.Write("Valor: " + competencia);
            ViewBag.idcompetenciav = competencia;

            Competencium data = APIConsumer<Competencium>.SelectOne(apiUrl.Replace("CompetenciaBloqueClasificas", "competencia") + competencia.ToString());
            ViewBag.idcompetenciav = competencia;
            ViewBag.ListadoDeportistas = listadoDeportistas((int)data.IdGen, (int)data.IdCat);
            ViewBag.ListadoDeportistaCompetencia = ListadoDeportistaCompetenciaAsignacion(competencia);
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
        public void PuestoDeportista(CompetenciaBloqueClasifica depor_comparacion, List<CompetenciaBloqueClasifica> list)
        {
            if (list.Count == 0)
            {
                depor_comparacion.Puesto = 1;
                depor_comparacion.ClasiBloque = true;
                Console.WriteLine("Entra");
                return;
            }
            else
            {

                foreach (CompetenciaBloqueClasifica depo in list)
                {
                    if (depor_comparacion.TopCla == depo.TopCla &&
                        depor_comparacion.ZonaCla == depo.ZonaCla &&
                        depor_comparacion.TopIntentosCla == depo.TopIntentosCla &&
                        depor_comparacion.ZonaIntentosCla == depo.ZonaIntentosCla)
                    {
                        depor_comparacion.Puesto = depo.Puesto;
                        return;
                    }

                    if (depor_comparacion.TopCla > depo.TopCla ||
                        (depor_comparacion.TopCla == depo.TopCla && depor_comparacion.ZonaCla > depo.ZonaCla) ||
                        (depor_comparacion.TopCla == depo.TopCla && depor_comparacion.ZonaCla == depo.ZonaCla && depor_comparacion.TopIntentosCla < depo.TopIntentosCla) ||
                        (depor_comparacion.TopCla == depo.TopCla && depor_comparacion.ZonaCla == depo.ZonaCla && depor_comparacion.TopIntentosCla == depo.TopIntentosCla && depor_comparacion.ZonaIntentosCla < depo.ZonaIntentosCla))
                    {
                        depor_comparacion.Puesto = depo.Puesto;

                        return;
                    }
                }

            }

            depor_comparacion.Puesto = (list.Count) + 1;
         
        }


    

       
        [Authorize(Roles = "Administrador,Juez")]
        public ActionResult AgregarDepo(CompetenciaBloqueClasifica competenciablo)
        {
            List<CompetenciaBloqueClasifica> list = ListadoDeportistaCompetenciaAsignacion((int)competenciablo.IdCom);
            if (comprobarDeportistaAgregado(competenciablo, list))
            {
                TempData["ErrorMessage"] = "Este deportista ya ha sido agregado a la competencia.";
                return RedirectToAction("agregardeportista", new { competencia = competenciablo.IdCom });
            }
            APIConsumer<CompetenciaBloqueClasifica>.Insert(apiUrl, competenciablo);
            TempData["ErrorMessage"] = null;

            return RedirectToAction("agregardeportista", new { competencia = competenciablo.IdCom });


        }
        [Authorize(Roles = "Administrador,Juez")]
        public ActionResult Create(CompetenciaBloqueClasifica competenciablo)
        {
            Console.WriteLine("Entra al metodo");
            try
            {
                

                List<CompetenciaBloqueClasifica> list = ListadoDeportistaCompetencia((int)competenciablo.IdCom);
                ;
         
                List<CompetenciaBloqueClasifica> listNull = ListadoDeportistaCompetenciaNulos((int)competenciablo.IdCom);

                if (comprobarDeportistaAgregado(competenciablo, list))
                {
                    TempData["ErrorMessage"] = "Este deportista ya ha sido agregado a la competencia.";
                    return RedirectToAction("Index", new { competencia = competenciablo.IdCom });
                }
                int idasignacion =0;
                foreach(CompetenciaBloqueClasifica depor in listNull)
                {
                    Console.WriteLine(competenciablo.IdDep + "-" + depor.IdDep);
                    if (competenciablo.IdDep == depor.IdDep)
                    {
                        Console.WriteLine("Entra al IF: "+depor.IdCompeBloqueCla);
                        idasignacion = depor.IdCompeBloqueCla;
                    }
                }

                Console.WriteLine("Mitad del metodo");

                PuestoDeportista(competenciablo, list);
                Console.WriteLine("Pasapuestos");
                if (competenciablo.Puesto <= 6)
                {
                    competenciablo.ClasiBloque = true;
                }
                else
                {
                    competenciablo.ClasiBloque = false;
                }

                Console.WriteLine("Casi al metodo: "+idasignacion);
                competenciablo.IdCompeBloqueCla = idasignacion;

                CompetenciaBloqueClasifica competenciabloConsult = APIConsumer<CompetenciaBloqueClasifica>.SelectOne(apiUrl + competenciablo.IdCompeBloqueCla);
                CompetenciaBloqueClasifica competenciabloEnviar = competenciabloConsult;

                competenciabloEnviar.TopCla = competenciablo.TopCla;
                competenciabloEnviar.ZonaCla = competenciablo.ZonaCla;
                competenciabloEnviar.TopIntentosCla = competenciablo.TopIntentosCla;
                competenciabloEnviar.ZonaIntentosCla = competenciablo.ZonaIntentosCla;
                competenciabloEnviar.ClasiBloque = competenciablo.ClasiBloque;
                competenciabloEnviar.Puesto = competenciablo.Puesto;


                APIConsumer<CompetenciaBloqueClasifica>.Update(apiUrl+ competenciablo.IdCompeBloqueCla.ToString(), competenciabloEnviar);
                Console.WriteLine("Pasa al metodo");


                TempData["ErrorMessage"] = null;

                return RedirectToAction("Index", new { competencia = competenciablo.IdCom });

               // return RedirectToAction(nameof(Index), new RouteValueDictionary { { "competencia", competencia } });
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                ModelState.AddModelError("", ex.Message);
                return View(competenciablo);
            }
        }

        public bool comprobarDeportistaAgregado(CompetenciaBloqueClasifica depor_comparacion, List<CompetenciaBloqueClasifica> list)
        {
            return list.Any(item => item.IdDep == depor_comparacion.IdDep);
        }



        private List<Deportistum> listadoDeportista()
        {
            var deportista = APIConsumer<Deportistum>.Select(apiUrl.Replace("CompetenciaBloqueClasificas", "Deportista"));
            var lista = deportista.Select(f => new Deportistum
            {
                IdDep = f.IdDep,
                NombresDep = f.NombresDep,
                ApellidosDep = f.ApellidosDep
            }).ToList();

            return lista;
        }
        private List<Deportistum> listadoDeportista(int compe)
        {
            var deportista = APIConsumer<Deportistum>.Select(apiUrl.Replace("CompetenciaBloqueClasificas", "Deportista")+"competencia/"+compe);
            var lista = deportista.Select(f => new Deportistum
            {
                IdDep = f.IdDep,
                NombresDep = f.NombresDep,
                ApellidosDep = f.ApellidosDep
            }).ToList();

            return lista;
        }

        private List<Deportistum> listadoDeportistas(int genero, int compe )
        {
            Console.WriteLine(apiUrl.Replace("CompetenciaBloqueClasificas", "Deportista") + genero + "-" + compe);
            var deportista = APIConsumer<Deportistum>.Select(apiUrl.Replace("CompetenciaBloqueClasificas", "Deportista")+ genero + "-"+ compe);
            var lista = deportista.Select(f => new Deportistum
            {
                IdDep = f.IdDep,
                NombresDep = f.NombresDep,
                ApellidosDep = f.ApellidosDep
            }).ToList();

            return lista;
        }

        [Authorize(Roles = "Administrador,Juez")]
        [HttpPost]
        public IActionResult SeleccionarDeportista(int deportistaSeleccionado)
        {
            // Hacer algo con el deportista seleccionado
            return RedirectToAction("Index");
        }

        private List<CompetenciaBloqueClasifica> ListadoDeportistaCompetencia(int id )
        {
            var competenciaBloque = APIConsumer<CompetenciaBloqueClasifica>.Select(apiUrl + "/compebloque/" + id);
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
                IdCompeBloqueCla = f.IdCompeBloqueCla
                
            }).Where(f => f.Puesto != null)  // Filtra los elementos donde Puesto no es nulo
                .ToList();
            if (!lista.IsNullOrEmpty())
            {
                lista.Sort((c1, c2) => Comparer<int>.Default.Compare((int)c1.Puesto, (int)c2.Puesto));
            }
          
            return lista;
        }

        private List<CompetenciaBloqueClasifica> ListadoDeportistaCompetenciaNulos(int id)
        {
            var competenciaBloque = APIConsumer<CompetenciaBloqueClasifica>.Select(apiUrl + "/compebloque/" + id);
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
                IdCompeBloqueCla = f.IdCompeBloqueCla

            }) // Filtra los elementos donde Puesto no es nulo
                .ToList();

            return lista;
        }

        private List<CompetenciaBloqueClasifica> ListadoDeportistaCompetenciaAsignacion(int id)
        {
            var competenciaBloque = APIConsumer<CompetenciaBloqueClasifica>.Select(apiUrl + "/compebloque/" + id);
            var lista = competenciaBloque.Select(f => new CompetenciaBloqueClasifica
            {
                
                IdDep = f.IdDep,
                IdCom = f.IdCom,
            }).ToList();


            return lista;
        }

    }


}
