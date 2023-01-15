using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFDI.v2.Code;
using ProyectoFDI.v2.Models;
using System.Data;

namespace ProyectoFDI.v2.Controllers
{
    public class UsuarioController : Controller
    {
        private string apiUrl;

        public UsuarioController(IConfiguration configuration)
        {
            apiUrl = configuration["urlBase"].ToString() + "Usuario/";
        }

        private List<SelectListItem> listaRoles()
        {
            var roles = new List<SelectListItem>();
            roles.Add(new SelectListItem { Text = "Administrador", Value = "Administrador" });
            roles.Add(new SelectListItem { Text = "Juez", Value = "Juez" });
            roles.Add(new SelectListItem { Text = "Entrenador", Value = "Entrenador" });
            roles.Add(new SelectListItem { Text = "Deportista", Value = "Deportista" });
            return roles;
        }

        [Authorize(Roles = "Administrador")]
        // GET: UsuarioController
        public ActionResult Index()
        {
            var data = APIConsumer<Usuario>.Select(apiUrl);
            return View(data);
        }

        [Authorize(Roles = "Administrador")]
        // GET: UsuarioController/Details/5
        public ActionResult Details(int id)
        {
            var data = APIConsumer<Usuario>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador")]
        // GET: UsuarioController/Create
        public ActionResult Create()
        {
            ViewBag.ListadoRoles = listaRoles();
            return View();
        }

        [Authorize(Roles = "Administrador")]
        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario usuario)
        {
            try
            {
                APIConsumer<Usuario>.Insert(apiUrl, usuario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(usuario);
            }
        }

        [Authorize(Roles = "Administrador")]
        // GET: UsuarioController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.ListadoRoles = listaRoles();
            var data = APIConsumer<Usuario>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador")]
        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Usuario usuario)
        {
            try
            {
                APIConsumer<Usuario>.Update(apiUrl + id.ToString(), usuario);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(usuario);
            }
        }

        [Authorize(Roles = "Administrador")]
        // GET: UsuarioController/Delete/5
        public ActionResult Delete(int id)
        {
            var data = APIConsumer<Usuario>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador")]
        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Usuario usuario)
        {
            try
            {
                APIConsumer<Usuario>.Delete(apiUrl + id.ToString());
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(usuario);
            }
        }
    }
}
