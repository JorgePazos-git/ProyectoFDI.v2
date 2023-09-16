using DocumentFormat.OpenXml.Office2013.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFDI.v2.Code;
using ProyectoFDI.v2.Models;

namespace ProyectoFDI.v2.Controllers
{
    public class DificultadController : Controller
    {
        private string apiUrl;
        private int idCom;

        public DificultadController(IConfiguration configuration)
        {
            apiUrl = configuration["urlBase"].ToString() + "DetalleCompetenciaDificultad/";
        }

        private List<SelectListItem> listaDeportistas()
        {
            var deportistas = APIConsumer<Deportistum>.Select(apiUrl.Replace("DetalleCompetenciaDificultad", "Deportista"));
            var lista = deportistas.Select(f => new SelectListItem
            {
                Value = f.IdDep.ToString(),
                Text = f.NombresDep + " " + f.ApellidosDep
            }).ToList();

            return lista;
        }

        private List<SelectListItem> listaCompetencias()
        {
            var deportistas = APIConsumer<Competencium>.Select(apiUrl.Replace("DetalleCompetenciaDificultad", "Competencia"));
            var lista = deportistas.Select(f => new SelectListItem
            {
                Value = f.IdCom.ToString(),
                Text = f.NombreCom
            }).ToList();

            return lista;
        }

        // GET: HomeController1
        public ActionResult Index(int competencia)
        {
            this.idCom = competencia;
            ViewBag.idcompetencia = competencia;
            GetCompetencia(idCom);

            return View(APIConsumer<DetalleCompetenciaDificultad>.Select(apiUrl));
        }

        public ActionResult lista()
        {
            return View(APIConsumer<DetalleCompetenciaDificultad>.Select(apiUrl));
        }

        private void GetCompetencia(int idCom)
        {

            string newUrl = apiUrl.Replace("DetalleCompetenciaDificultad",
                "Competencia") + idCom.ToString();

            Competencium competencia = APIConsumer<Competencium>.SelectOne(newUrl);

            ViewBag.competencia = competencia;
        }

        // GET: HomeController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeController1/Create
        public ActionResult Create(int idCom)
        {
            ViewBag.compe = idCom;
            ViewBag.listaDeportistas = listaDeportistas();
            ViewBag.listaCompetencias = listaCompetencias();
            List<DetalleCompetenciaDificultad> lista = new List<DetalleCompetenciaDificultad>();
            DetalleCompetenciaDificultad n = new DetalleCompetenciaDificultad();
            lista.Add(n);
            return View(lista);
        }

        // POST: HomeController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(List<DetalleCompetenciaDificultad> detalles)
        {
            try
            {
                foreach(DetalleCompetenciaDificultad detalle in detalles)
                {
                    APIConsumer<DetalleCompetenciaDificultad>.Insert(apiUrl, detalle);
                }
                    
               
                return RedirectToAction("Edit", "Competencia", new { id = detalles[0].IdCom });

            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController1/Edit/5
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

        // GET: HomeController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController1/Delete/5
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
