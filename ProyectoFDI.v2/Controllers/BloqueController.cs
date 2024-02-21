using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFDI.v2.Code;
using ProyectoFDI.v2.Models;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.JSInterop;
using System.Globalization;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace ProyectoFDI.v2.Controllers
{
    public class BloqueController : Controller
    {
        public int idCompetencia;
        private string apiUrl;

        public BloqueController(IConfiguration configuration)
        {
            apiUrl = configuration["urlBase"].ToString() + "PuntajeBloques/";
        }

        // GET: BloqueController
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Index(int competencia)
        {
            Console.WriteLine("Entra metodo");

            this.idCompetencia = competencia;
            ViewBag.idcompetenciav = competencia;

            ViewBag.ListadoDeportistas = listadoDeportista();
            ViewBag.listadoDeportistaCompetencia = listadoDeportistaCompetencia(idCompetencia, "Clasificatoria");

            return View();
        }
        public ActionResult TablaPosicionesClasificatoria(int id)
        {
            ViewBag.idcompetenciav = id;

            ViewBag.ListadoPosicionesClasificatoria = ListadoPosicionesClasificatoria(id, "Clasificatoria");

            return View();
        }
        private List<VistaPuntajesDeportista> ListadoPosicionesClasificatoria(int id,String etapa)
        {
            List<VistaPuntajesDeportista> lista = new List<VistaPuntajesDeportista>();

            try
            {
                var deportista = APIConsumer<VistaPuntajesDeportista>.Select(apiUrl.Replace("PuntajeBloques", "VistaPuntajesDeportistas/ByCompetencia") + id+"/"+etapa);

                lista = deportista.Select(f => new VistaPuntajesDeportista
                {
                    IdVw = f.IdVw,
                    IdCom = f.IdCom,
                    IdDep = f.IdDep,
                    Etapa = f.Etapa,
                    IntentosTops = f.IntentosTops,
                    ZonasRealizadas = f.ZonasRealizadas,
                    IntentosZonas = f.IntentosZonas,
                    NombreDep = f.NombreDep,
                    TopsRealizados = f.TopsRealizados
         
                }).ToList();
            }
            catch (Exception ex)
            {
                // Manejar la excepción, aquí simplemente retornamos una lista vacía
                Console.WriteLine("Ocurrió una excepción: " + ex.Message);
            }
            return lista;

        }


            public ActionResult AgregarResultados(int id)
        {
            if (listadoDeportistaCompetencia(id, "Final").Count ==0)
            {
                ViewBag.habilitado = true;
            }
            else
            {
                ViewBag.habilitado = false;
            }

            ViewBag.idcompetenciav = id;

            ViewBag.listadoDeportistaCompetencia = listadoDeportistaCompetencia(id, "Clasificatoria");

            return View();
        }

        public ActionResult TablaResultados(int idcompetenciav)
        {
            ViewBag.idcompetenciav = idcompetenciav;

            ViewBag.listadoPuntajeBloque = ListadoPuntajeBloque(idcompetenciav,"Clasificatoria");
            ViewBag.listadoDeportistaCompetencia = listadoDeportistaCompetencia(idcompetenciav, "Clasificatoria");


            return View();
        }
        [HttpPost]
        public IActionResult AgregarDepoDatos(int IdDep, int IdCom, int Top1, int Top2, int Top3, int Top4, int Zona1, int Zona2, int Zona3, int Zona4,string etapa)
        {
            PuntajeBloque a;
            try
            {
                a = APIConsumer<PuntajeBloque>.SelectOne(apiUrl + IdCom + "/" + IdDep + "/" + 1+"/"+etapa);
            }
            catch (Exception e)
            {
                a = null;
            }
            if (a==null)
            {
                PuntajeBloque puntajeBloque = new PuntajeBloque();
                puntajeBloque.IdDep = IdDep;
                puntajeBloque.IdCom = IdCom;
                puntajeBloque.Etapa = etapa;

                // Bloque 1
                puntajeBloque.NumeroBloque = 1;
                puntajeBloque.IntentosTops = Top1;
                puntajeBloque.IntentosZonas = Zona1;
                APIConsumer<PuntajeBloque>.Insert(apiUrl, puntajeBloque);

                // Bloque 2
                puntajeBloque.NumeroBloque = 2;
                puntajeBloque.IntentosTops = Top2;
                puntajeBloque.IntentosZonas = Zona2;
                APIConsumer<PuntajeBloque>.Insert(apiUrl, puntajeBloque);

                // Bloque 3
                puntajeBloque.NumeroBloque = 3;
                puntajeBloque.IntentosTops = Top3;
                puntajeBloque.IntentosZonas = Zona3;
                APIConsumer<PuntajeBloque>.Insert(apiUrl, puntajeBloque);

                // Bloque 4
                puntajeBloque.NumeroBloque = 4;
                puntajeBloque.IntentosTops = Top4;
                puntajeBloque.IntentosZonas = Zona4;
                APIConsumer<PuntajeBloque>.Insert(apiUrl, puntajeBloque);
                if(etapa == "Final")
                {
                    return RedirectToAction("AgregarResultadosFinales", "Bloque", new { id = IdCom });

                }
                return RedirectToAction("AgregarResultados", "Bloque", new { id = IdCom });
            }


            //Bloque 1
            String conexion = apiUrl + IdCom + "/" + IdDep + "/" + 1+"/"+etapa;
            var puntaje = APIConsumer<PuntajeBloque>.SelectOne(conexion);
            puntaje.IntentosTops = Top1;
            puntaje.IntentosZonas = Zona1;
            APIConsumer<PuntajeBloque>.Update(apiUrl + puntaje.IdBloPts.ToString(), puntaje);

            //Bloque2
            puntaje = APIConsumer<PuntajeBloque>.SelectOne(apiUrl + IdCom + "/" + IdDep + "/" + 2 + "/" + etapa);
            puntaje.IntentosTops = Top2;
            puntaje.IntentosZonas = Zona2;
            APIConsumer<PuntajeBloque>.Update(apiUrl + puntaje.IdBloPts.ToString(), puntaje);

            //Bloque 3
            puntaje = APIConsumer<PuntajeBloque>.SelectOne(apiUrl + IdCom + "/" + IdDep + "/" + 3 + "/" + etapa);
            puntaje.IntentosTops = Top3;
            puntaje.IntentosZonas = Top3;
            APIConsumer<PuntajeBloque>.Update(apiUrl + puntaje.IdBloPts.ToString(), puntaje);

            //Bloque 4
            puntaje = APIConsumer<PuntajeBloque>.SelectOne(apiUrl + IdCom + "/" + IdDep + "/" + 4 + "/" + etapa);
            puntaje.IntentosTops = Top4;
            puntaje.IntentosZonas = Zona4;
            APIConsumer<PuntajeBloque>.Update(apiUrl + puntaje.IdBloPts.ToString(), puntaje);
            if (etapa == "Final")
            {
                return RedirectToAction("AgregarResultadosFinales", "Bloque", new { id = IdCom });

            }
            return RedirectToAction("AgregarResultados", "Bloque", new { id = IdCom });


        }


        private List<Deportistum> listadoDeportista()
        {
            var deportista = APIConsumer<Deportistum>.Select(apiUrl.Replace("PuntajeBloques", "Deportista"));
            var lista = deportista.Select(f => new Deportistum
            {
                IdDep = f.IdDep,
                NombresDep = f.NombresDep,
                ApellidosDep = f.ApellidosDep
            }).ToList();

            return lista;
        }
        private List<PuntajeBloque> ListadoPuntajeBloque(int id, String etapa)
{
    List<PuntajeBloque> lista = new List<PuntajeBloque>();

    try
    {
        var deportista = APIConsumer<PuntajeBloque>.Select(apiUrl + "Competencia/" + id+"/"+etapa);

        lista = deportista.Select(f => new PuntajeBloque
        {
            NumeroBloque = f.NumeroBloque,
            IntentosTops = f.IntentosTops,
            IntentosZonas = f.IntentosZonas,
            IdDep = f.IdDep
            
        }).ToList();
    }
    catch (Exception ex)
    {
        // Manejar la excepción, aquí simplemente retornamos una lista vacía
        Console.WriteLine("Ocurrió una excepción: " + ex.Message);
    }

    return lista;
}


        //private List<PuntajeBloque> ListadoPuntajeBloque(int id)
        //{

        //    try
        //    {
        //        var deportista = APIConsumer<PuntajeBloque>.Select(apiUrl + "Competencia/" + id);

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    var lista = deportista.Select(f => new PuntajeBloque
        //    {
        //        NumeroBloque = f.NumeroBloque,
        //        IntentosTops = f.IntentosTops,
        //        IntentosZonas = f.IntentosZonas,
        //        IdDep = f.IdDep

        //    }).ToList();

        //    return lista;
        //}
        private List<Deportistum> listadoDeportistaCompetencia(int id,string etapa)
        {
            List<Deportistum> lista = new List<Deportistum>();

            try
            {
                var deportista = APIConsumer<Deportistum>.Select(apiUrl.Replace("PuntajeBloques", "Deportista/competencia") + id+"/"+etapa);

                lista = deportista.Select(f => new Deportistum
                {
                    IdDep = f.IdDep,
                    NombresDep = f.NombresDep,
                    ApellidosDep = f.ApellidosDep
                }).ToList();
            }
            catch (Exception ex)
            {
                // Manejar la excepción, aquí simplemente retornamos una lista vacía
                Console.WriteLine("Ocurrió una excepción: " + ex.Message);
            }

            return lista;
        }


        //private List<Deportistum> listadoDeportistaCompetencia(int id)
        //{
        //    var deportista = APIConsumer<Deportistum>.Select(apiUrl.Replace("PuntajeBloques", "Deportista/competencia")+id);
        //    var lista = deportista.Select(f => new Deportistum
        //    {
        //        IdDep = f.IdDep,
        //        NombresDep = f.NombresDep,
        //        ApellidosDep = f.ApellidosDep
        //    }).ToList();

        //    return lista;
        //}
        public ActionResult AgregarDepo(ResultadoBloque resultadoBloque)
        {
            resultadoBloque.Etapa = "Clasificatoria";
            APIConsumer<ResultadoBloque>.Insert(apiUrl.Replace("PuntajeBloques", "ResultadoBloques"), resultadoBloque);
            TempData["ErrorMessage"] = null;

           
            return RedirectToAction("Index", "Bloque", new { competencia = resultadoBloque.IdCom });


        }

        // GET: BloqueController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BloqueController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BloqueController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BloqueController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BloqueController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BloqueController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BloqueController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        //FASE FINAL METODOS
        public ActionResult AgregarResultadosFinales(int id)
        {
            ViewBag.idcompetenciav = id;
            List<Deportistum> deportista = listadoDeportistaCompetencia(id, "Final");
            if(deportista.Count == 0)
            {
                AgregarDeportistasFinales(ListadoPosicionesClasificatoria(id,"Clasificatoria"));
                deportista = listadoDeportistaCompetencia(id, "Final");
            }
            ViewBag.listadoDeportistaCompetencia = deportista;


            return View();
        }

        public ActionResult TablaResultadosFinales(int idcompetenciav)
        {
            ViewBag.idcompetenciav = idcompetenciav;

            ViewBag.listadoPuntajeBloque = ListadoPuntajeBloque(idcompetenciav, "Final");
            ViewBag.listadoDeportistaCompetencia = listadoDeportistaCompetencia(idcompetenciav, "Final");


            return View();
        }
        public ActionResult TablaPosicionesFinales(int id)
        {
            ViewBag.idcompetenciav = id;

            ViewBag.ListadoPosicionesClasificatoria = ListadoPosicionesClasificatoria(id,"Final");

            return View();
        }

        public void AgregarDeportistasFinales(List<VistaPuntajesDeportista> listadoresultados) {
            int aux= 1;
            ResultadoBloque registro;
            ResultadoBloque registrofinal = new ResultadoBloque();
            bool veriEmpa = false;
            foreach (var item in listadoresultados)
            {
                registro = APIConsumer<ResultadoBloque>.SelectOne(apiUrl.Replace("PuntajeBloques", "ResultadoBloques") + item.IdCom+"/"+item.IdDep+"/Clasificatoria");
                registro.Puesto = aux;
                APIConsumer<ResultadoBloque>.Update(apiUrl.Replace("PuntajeBloques", "ResultadoBloques") + registro.IdResBloque.ToString(), registro);
                if(aux <= 6 | veriEmpa)
                {
                    registrofinal.Etapa = "Final";
                    registrofinal.IdDep = registro.IdDep;
                    registrofinal.IdCom = registro.IdCom;
                    APIConsumer<ResultadoBloque>.Insert(apiUrl.Replace("PuntajeBloques", "ResultadoBloques"), registrofinal);
                    registrofinal = new ResultadoBloque();
                    if (aux == 6 && item.IntentosZonas == listadoresultados[aux].IntentosZonas && item.IntentosZonas == listadoresultados[aux].IntentosZonas
                        && item.TopsRealizados == listadoresultados[aux].TopsRealizados && item.ZonasRealizadas == listadoresultados[aux].ZonasRealizadas)
                    {
                        veriEmpa = true;
                    }
                    else
                    {
                        veriEmpa = false;
                    }
                }
                

                aux++;
            }
        }

    }
}
