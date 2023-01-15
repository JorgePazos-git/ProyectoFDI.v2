using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoFDI.v2.Code;
using ProyectoFDI.v2.Models;
using System.Data;

namespace ProyectoFDI.v2.Controllers
{
    public class SedeController : Controller
    {
        private string apiUrl;

        public SedeController(IConfiguration configuration)
        {
            apiUrl = configuration["urlBase"].ToString() + "Sede/";
        }

        [Authorize(Roles = "Administrador,Juez")]
        // GET: SedeController
        public ActionResult Index()
        {
            var data = APIConsumer<Sede>.Select(apiUrl);
            return View(data);
        }

        [Authorize(Roles = "Administrador,Juez")]
        // GET: SedeController/Details/5
        public ActionResult Details(int id)
        {
            var data = APIConsumer<Sede>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador,Juez")]
        // GET: SedeController/Create
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrador,Juez")]
        // POST: SedeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sede sede)
        {
            try
            {
                APIConsumer<Sede>.Insert(apiUrl, sede);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(sede);
            }
        }

        [Authorize(Roles = "Administrador,Juez")]
        // GET: SedeController/Edit/5
        public ActionResult Edit(int id)
        {
            var data = APIConsumer<Sede>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador,Juez")]
        // POST: SedeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Sede sede)
        {
            try
            {
                APIConsumer<Sede>.Update(apiUrl + id.ToString(), sede);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(sede);
            }
        }

        [Authorize(Roles = "Administrador,Juez")]
        // GET: SedeController/Delete/5
        public ActionResult Delete(int id)
        {
            var data = APIConsumer<Sede>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador,Juez")]
        // POST: SedeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Sede sede)
        {
            try
            {
                APIConsumer<Sede>.Delete(apiUrl + id.ToString());
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(sede);
            }
        }
    }
}
