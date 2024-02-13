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
            ViewBag.listadoDeportistaCompetencia = listadoDeportistaCompetencia(idCompetencia);

            return View();
        }
        public ActionResult AgregarResultados(int id)
        {
            ViewBag.idcompetenciav = id;

            ViewBag.listadoDeportistaCompetencia = listadoDeportistaCompetencia(id);

            return View();
        }

        public ActionResult TablaResultados(int idcompetenciav)
        {
            ViewBag.idcompetenciav = idcompetenciav;

            ViewBag.listadoPuntajeBloque = ListadoPuntajeBloque(idcompetenciav);
            ViewBag.listadoDeportistaCompetencia = listadoDeportistaCompetencia(idcompetenciav);


            return View();
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

        private List<PuntajeBloque> ListadoPuntajeBloque(int id)
        {
            var deportista = APIConsumer<PuntajeBloque>.Select(apiUrl+ "Competencia/"+id);
            var lista = deportista.Select(f => new PuntajeBloque
            {
                NumeroBloque = f.NumeroBloque,
                IntentosTops = f.IntentosTops,
                IntentosZonas = f.IntentosZonas,
                IdDep = f.IdDep
                
            }).ToList();

            return lista;
        }

        private List<Deportistum> listadoDeportistaCompetencia(int id)
        {
            var deportista = APIConsumer<Deportistum>.Select(apiUrl.Replace("PuntajeBloques", "Deportista/competencia")+id);
            var lista = deportista.Select(f => new Deportistum
            {
                IdDep = f.IdDep,
                NombresDep = f.NombresDep,
                ApellidosDep = f.ApellidosDep
            }).ToList();

            return lista;
        }
        public ActionResult AgregarDepo(ResultadoBloque resultadoBloque)
        {
            
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
    }
}
