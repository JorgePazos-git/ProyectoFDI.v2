using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFDI.v2.Code;
using ProyectoFDI.v2.Models;
using System.Data;

namespace ProyectoFDI.v2.Controllers
{
    public class JuezController : Controller
    {
        private string apiUrl;

        public JuezController(IConfiguration configuration)
        {
            apiUrl = configuration["urlBase"].ToString() + "Juez/";
        }

        [Authorize(Roles = "Administrador")]
        // GET: JuezController
        public ActionResult Index(string? searchFor)
        {
            ViewBag.SearchFor = "" + searchFor;

            if (string.IsNullOrWhiteSpace(searchFor))
            {
                return View(APIConsumer<Juez>.Select(apiUrl));
            }
            else
            {
                return View(APIConsumer<Juez>.Select_SearchFor(apiUrl, searchFor));
            }
        }

        private List<SelectListItem> listaEstados()
        {
            var roles = new List<SelectListItem>();
            roles.Add(new SelectListItem { Text = "Si", Value = "true" });
            roles.Add(new SelectListItem { Text = "No", Value = "false" });
            return roles;
        }

        private List<SelectListItem> listaProvincias()
        {
            var provincias = APIConsumer<Provincium>.Select(apiUrl.Replace("Juez", "Provincia"));
            var lista = provincias.Select(f => new SelectListItem
            {
                Value = f.IdPro.ToString(),
                Text = f.NombrePro
            }).ToList();

            return lista;
        }

        private List<SelectListItem> listaUsuarios()
        {
            var usuarios = APIConsumer<Usuario>.Select(apiUrl.Replace("Juez", "Usuario"));
            var lista = usuarios.Select(f => new SelectListItem
            {
                Value = f.IdUsu.ToString(),
                Text = f.NombreUsu
            }).ToList();

            return lista;
        }

        private List<SelectListItem> listaEstados2()
        {
            var roles = new List<SelectListItem>();
            roles.Add(new SelectListItem { Text = "Activo", Value = "true" });
            roles.Add(new SelectListItem { Text = "Inactivo", Value = "false" });
            return roles;
        }

        [Authorize(Roles = "Administrador")]
        // GET: JuezController/Details/5
        public ActionResult Details(int id)
        {
            var data = APIConsumer<Juez>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador")]
        // GET: JuezController/Create
        public ActionResult Create()
        {
            ViewBag.ListaProvincias = listaProvincias();
            ViewBag.ListaUsuarios = listaUsuarios();
            ViewBag.ListaEstados = listaEstados();
            ViewBag.ListaEstados2 = listaEstados2();
            return View();
        }

        [Authorize(Roles = "Administrador")]
        // POST: JuezController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Juez juez)
        {
            try
            {
                Usuario usuario = new Usuario();
                usuario.IdUsu = 0;
                usuario.NombreUsu = juez.CedulaJuez;
                usuario.ClaveUsu = juez.CedulaJuez;
                usuario.FechaCreacion = DateTime.Now;
                usuario.RolesUsu = "Juez";
                usuario.ActivoUsu = true;

                juez.IdUsuNavigation = usuario;

                APIConsumer<Juez>.Insert(apiUrl, juez);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(juez);
            }
        }

        [Authorize(Roles = "Administrador")]
        // GET: JuezController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.ListaProvincias = listaProvincias();
            ViewBag.ListaUsuarios = listaUsuarios();
            ViewBag.ListaEstados = listaEstados();
            ViewBag.ListaEstados2 = listaEstados2();
            var data = APIConsumer<Juez>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador")]
        // POST: JuezController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Juez juez)
        {
            try
            {
                APIConsumer<Juez>.Update(apiUrl + id.ToString(), juez);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(juez);
            }
        }
        
        [Authorize(Roles = "Administrador")]
        // GET: JuezController/Delete/5
        public ActionResult Delete(int id)
        {
            var data = APIConsumer<Juez>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador")]
        // POST: JuezController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Juez juez)
        {
            try
            {
                var data = APIConsumer<Juez>.SelectOne(apiUrl + id.ToString());
                Usuario usuario = data.IdUsuNavigation;
                usuario.ActivoUsu = false;
                data.ActivoJuez = false;
                APIConsumer<Juez>.Update(apiUrl + id.ToString(), data);
                APIConsumer<Usuario>.Update(apiUrl.Replace("Juez", "Usuario") + usuario.IdUsu.ToString(), usuario);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(juez);
            }
        }
    }
}
