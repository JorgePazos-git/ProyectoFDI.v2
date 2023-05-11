using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            ViewBag.ListadoDeportistas = listadoDeportista();
            ViewBag.ListadoDeportistaCompetencia = ListadoDeportistaCompetencia(competencia);
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

                bool aux = true; ;
                bool comprobracion = true;
               
                foreach(CompetenciaBloqueClasifica depo in list)
                {
                    if (comprobracion)
                    {


                        if (depor_comparacion.TopCla == depo.TopCla && depor_comparacion.ZonaCla == depo.ZonaCla && depor_comparacion.TopIntentosCla == depo.TopIntentosCla && depor_comparacion.ZonaIntentosCla == depo.ZonaIntentosCla)
                        {
                            AsignacionPuesto(depor_comparacion, depo);
                            comprobracion = false;
                            aux = false;
                           
                        }
                        else
                        {
                            if (depor_comparacion.TopCla > depo.TopCla)
                            {
                                AsignacionPuesto(depor_comparacion, depo);
                                comprobracion = false;
                                aux = false;
                            }
                            else
                            {
                                if(depor_comparacion.TopCla == depo.TopCla)
                                {
                                    if (depor_comparacion.ZonaCla > depo.ZonaCla)
                                    {
                                        AsignacionPuesto(depor_comparacion, depo);
                                        comprobracion = false;
                                        aux = false;
                                    }
                                    else
                                    {
                                        if (depor_comparacion.ZonaCla == depo.ZonaCla)
                                        {
                                            //
                                            if (depor_comparacion.TopIntentosCla < depo.TopIntentosCla)
                                            {
                                                AsignacionPuesto(depor_comparacion, depo);
                                                comprobracion = false;
                                                aux = false;
                                            }
                                            else
                                            {
                                                if (depor_comparacion.TopIntentosCla == depo.TopIntentosCla)
                                                {
                                                    if (depor_comparacion.ZonaIntentosCla < depo.ZonaIntentosCla)
                                                    {
                                                        AsignacionPuesto(depor_comparacion, depo);
                                                        comprobracion = false;
                                                        aux = false;
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                            }

                        }

                    }
                    else
                    {
                        int puesto = (int)(depo.Puesto + 1);
                        depo.Puesto = puesto;
                        if (puesto <= 6)
                        {
                            depo.ClasiBloque = true;
                        }
                        else
                        {
                            depo.ClasiBloque = false;
                        }
                        APIConsumer<CompetenciaBloqueClasifica>.Update(apiUrl + depo.IdCompeBloqueCla, depo);
                        aux = false;
                    }

                    //

                }
                if (aux)
                {
                    depor_comparacion.Puesto = (list.Count)+1;
                    if (depor_comparacion.Puesto <= 6)
                    {
                        depor_comparacion.ClasiBloque = true;
                    }
                    else
                    {
                        depor_comparacion.ClasiBloque = false;
                    }

                }


            }




            Console.WriteLine("Acaba organizar puesto");
        }

        public void AsignacionPuesto(CompetenciaBloqueClasifica depor_comparacion, CompetenciaBloqueClasifica depo)
        {
            Console.Write("Entra metodo");

            depor_comparacion.Puesto = depo.Puesto;
            int puesto = (int)(depo.Puesto + 1);
            depo.Puesto = puesto;
            if (puesto <= 6)
            {
                depo.ClasiBloque = true;
            }
            else
            {
                depo.ClasiBloque = false;
            }
            APIConsumer<CompetenciaBloqueClasifica>.Update(apiUrl + depo.IdCompeBloqueCla, depo);
            Console.Write("Pasa metodo");
        }

        [Authorize(Roles = "Administrador,Juez")]
        public ActionResult Create(CompetenciaBloqueClasifica competenciablo)
        {
           
            try
            {
                List<CompetenciaBloqueClasifica> list = ListadoDeportistaCompetencia((int)competenciablo.IdCom);
                if (comprobarDeportistaAgregado(competenciablo, list))
                {
                    TempData["ErrorMessage"] = "Este deportista ya ha sido agregado a la competencia.";
                    return RedirectToAction("Index", new { competencia = competenciablo.IdCom });
                }

                


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
                APIConsumer<CompetenciaBloqueClasifica>.Insert(apiUrl, competenciablo);
                

                if (list.Count >= 7)
                {
                    list = ListadoDeportistaCompetencia((int)competenciablo.IdCom);
                    CompetenciaBloqueClasifica com6 = list[5];
                    CompetenciaBloqueClasifica com7 = list[6];

                    if (com6.TopCla == com7.TopCla && com6.ZonaCla == com7.ZonaCla && com6.TopIntentosCla == com7.TopIntentosCla && com6.ZonaIntentosCla == com7.ZonaIntentosCla)
                    {
                        com7.ClasiBloque = true;
                        APIConsumer<CompetenciaBloqueClasifica>.Update(apiUrl + com7.IdCompeBloqueCla, com7);

                    }

                }

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
            }).ToList();

           lista.Sort((c1, c2) => Comparer<int>.Default.Compare((int)c1.Puesto, (int)c2.Puesto));
            return lista;
        }


    }


}
