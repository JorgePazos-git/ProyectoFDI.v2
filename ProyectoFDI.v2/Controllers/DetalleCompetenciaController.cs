using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFDI.v2.Code;
using ProyectoFDI.v2.Models;
using System.Data;
using System.Runtime.Intrinsics.Arm;

namespace ProyectoFDI.v2.Controllers
{
    public class DetalleCompetenciaController : Controller
    {
        private string apiUrl;

        public DetalleCompetenciaController(IConfiguration configuration)
        {
            apiUrl = configuration["urlBase"].ToString() + "DetalleCompetencia/";
            ViewBag.ReturnTo = "";
        }

        [Authorize(Roles = "Administrador,Juez")]
        // GET: DetalleCompetenciaController
        public ActionResult Index()
        {
            return View(APIConsumer<DetalleCompetencium>.Select(apiUrl));
        }

        private List<SelectListItem> listaDeportistas()
        {
            var deportistas = APIConsumer<Deportistum>.Select(apiUrl.Replace("DetalleCompetencia", "Deportista"));
            var lista = deportistas.Select(f => new SelectListItem
            {
                Value = f.IdDep.ToString(),
                Text = f.NombresDep + " " + f.ApellidosDep
            }).ToList();

            return lista;
        }

        private List<SelectListItem> listaCompetencias()
        {
            var deportistas = APIConsumer<Competencium>.Select(apiUrl.Replace("DetalleCompetencia", "Competencia"));
            var lista = deportistas.Select(f => new SelectListItem
            {
                Value = f.IdCom.ToString(),
                Text = f.NombreCom
            }).ToList();

            return lista;
        }

        [Authorize(Roles = "Administrador,Juez")]
        // GET: DetalleCompetenciaController/Details/5
        public ActionResult Details(int id)
        {
            var data = APIConsumer<DetalleCompetencium>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador,Juez")]
        // GET: DetalleCompetenciaController/Create
        public ActionResult Create(string? returnTo)
        {
            ViewBag.ReturnTo = returnTo;
            ViewBag.listaDeportistas = listaDeportistas();
            ViewBag.listaCompetencias = listaCompetencias();
            return View();
        }

        [Authorize(Roles = "Administrador,Juez")]
        // POST: DetalleCompetenciaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DetalleCompetencium detalle, string returnTo)
        {
            try
            {
                APIConsumer<DetalleCompetencium>.Insert(apiUrl, detalle);
                if (returnTo == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Edit", "Competencia", new { id = detalle.IdCom });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(detalle);
            }
        }

        [Authorize(Roles = "Administrador,Juez")]
        // GET: DetalleCompetenciaController/Edit/5
        public ActionResult Edit(int id, string? returnTo)
        {
            ViewBag.listaDeportistas = listaDeportistas();
            ViewBag.listaCompetencias = listaCompetencias();
            ViewBag.ReturnTo = returnTo;
            var data = APIConsumer<DetalleCompetencium>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador,Juez")]
        // POST: DetalleCompetenciaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, DetalleCompetencium detalle, string returnTo)
        {
            try
            {
                APIConsumer<DetalleCompetencium>.Update(apiUrl + id.ToString(), detalle);
                if (returnTo == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Edit", "Competencia", new { id = id });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(detalle);
            }
        }

        [Authorize(Roles = "Administrador,Juez")]
        // GET: DetalleCompetenciaController/Delete/5
        public ActionResult Delete(int id, string? returnTo)
        {
            var data = APIConsumer<DetalleCompetencium>.SelectOne(apiUrl + id.ToString());
            ViewBag.ReturnTo = returnTo;
            return View(data);
        }

        [Authorize(Roles = "Administrador,Juez")]
        // POST: DetalleCompetenciaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, DetalleCompetencium detalle, string returnTo)
        {
            try
            {
                APIConsumer<DetalleCompetencium>.Delete(apiUrl + id.ToString());
                if (returnTo == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Edit", "Competencia", new { id = id });
                }
            }
            catch
            {
                return View(detalle);
            }
        }
    }
}
