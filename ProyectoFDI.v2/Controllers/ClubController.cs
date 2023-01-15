using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoFDI.v2.Code;
using ProyectoFDI.v2.Models;
using System.Data;

namespace ProyectoFDI.v2.Controllers
{
    public class ClubController : Controller
    {
        private string apiUrl;

        public ClubController(IConfiguration configuration)
        {
            apiUrl = configuration["urlBase"].ToString() + "Club/";
        }

        [Authorize(Roles = "Administrador,Entrenador")]
        // GET: ClubController
        public ActionResult Index()
        {
            var data = APIConsumer<Club>.Select(apiUrl);
            return View(data);
        }

        [Authorize(Roles = "Administrador,Entrenador")]
        // GET: ClubController/Details/5
        public ActionResult Details(int id)
        {
            var data = APIConsumer<Club>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador,Entrenador")]
        // GET: ClubController/Create
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrador,Entrenador")]
        // POST: ClubController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Club club)
        {
            try
            {
                APIConsumer<Club>.Insert(apiUrl, club);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(club);
            }
        }

        [Authorize(Roles = "Administrador,Entrenador")]
        // GET: ClubController/Edit/5
        public ActionResult Edit(int id)
        {
            var data = APIConsumer<Club>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador,Entrenador")]
        // POST: ClubController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Club club)
        {
            try
            {
                APIConsumer<Club>.Update(apiUrl + id.ToString(), club);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(club);
            }
        }

        [Authorize(Roles = "Administrador,Entrenador")]
        // GET: ClubController/Delete/5
        public ActionResult Delete(int id)
        {
            var data = APIConsumer<Club>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador,Entrenador")]
        // POST: ClubController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Club club)
        {
            try
            {
                APIConsumer<Club>.Delete(apiUrl + id.ToString());
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(club);
            }
        }
    }
}
