using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFDI.v2.Code;
using ProyectoFDI.v2.Models;
using System.Data;

namespace ProyectoFDI.v2.Controllers
{
    public class EntrenadorController : Controller
    {
        private string apiUrl;

        public EntrenadorController(IConfiguration configuration)
        {
            apiUrl = configuration["urlBase"].ToString() + "Entrenador/";
        }

        [Authorize(Roles = "Administrador")]
        // GET: EntrenadorController
        public ActionResult Index(string? searchFor)
        {
            ViewBag.SearchFor = "" + searchFor;

            if (string.IsNullOrWhiteSpace(searchFor))
            {
                return View(APIConsumer<Entrenador>.Select(apiUrl));
            }
            else
            {
                return View(APIConsumer<Entrenador>.Select_SearchFor(apiUrl, searchFor));
            }
        }
        private List<SelectListItem> listaProvincias()
        {
            var provincias = APIConsumer<Provincium>.Select(apiUrl.Replace("Entrenador", "Provincia"));
            var lista = provincias.Select(f => new SelectListItem
            {
                Value = f.IdPro.ToString(),
                Text = f.NombrePro
            }).ToList();

            return lista;
        }
        private List<SelectListItem> listaDeportistas()
        {
            var deportistas = APIConsumer<Deportistum>.Select(apiUrl.Replace("Entrenador", "Deportista"));
            var lista = deportistas.Select(f => new SelectListItem
            {
                Value = f.IdDep.ToString(),
                Text = f.NombresDep
            }).ToList();

            return lista;
        }
        private List<SelectListItem> listaUsuarios()
        {
            var usuarios = APIConsumer<Usuario>.Select(apiUrl.Replace("Entrenador", "Usuario"));
            var lista = usuarios.Select(f => new SelectListItem
            {
                Value = f.IdUsu.ToString(),
                Text = f.NombreUsu
            }).ToList();

            return lista;
        }

        private List<SelectListItem> listaEstados()
        {
            var roles = new List<SelectListItem>();
            roles.Add(new SelectListItem { Text = "Activo", Value = "true" });
            roles.Add(new SelectListItem { Text = "Inactivo", Value = "false" });
            return roles;
        }

        [Authorize(Roles = "Administrador")]
        // GET: EntrenadorController/Details/5
        public ActionResult Details(int id)
        {
            var data = APIConsumer<Entrenador>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador")]
        // GET: EntrenadorController/Create
        public ActionResult Create()
        {
            var url = apiUrl;
            ViewBag.ListaDeportistas = listaDeportistas();
            ViewBag.ListaProvincias = listaProvincias();
            ViewBag.ListaUsuarios = listaUsuarios();
            ViewBag.ListaEstados = listaEstados();
            return View();
        }

        [Authorize(Roles = "Administrador")]
        // POST: EntrenadorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Entrenador entrenador)
        {
            try
            {
                Usuario usuario = new Usuario();
                usuario.IdUsu = 0;
                usuario.NombreUsu = entrenador.CedulaEnt;
                usuario.ClaveUsu = entrenador.CedulaEnt;
                usuario.FechaCreacion = DateTime.Now;
                usuario.RolesUsu = "Entrenador";
                usuario.ActivoUsu = true;

                entrenador.IdUsuNavigation = usuario;

                APIConsumer<Entrenador>.Insert(apiUrl, entrenador);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(entrenador);
            }
        }

        [Authorize(Roles = "Administrador")]
        // GET: EntrenadorController/Edit/5
        public ActionResult Edit(int id)
        {
            var url = apiUrl;
            ViewBag.ListaDeportistas = listaDeportistas();
            ViewBag.ListaProvincias = listaProvincias();
            ViewBag.ListaUsuarios = listaUsuarios();
            ViewBag.ListaEstados = listaEstados();
            var data = APIConsumer<Entrenador>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador")]
        // POST: EntrenadorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Models.Entrenador entrenador)
        {
            try
            {
                APIConsumer<Entrenador>.Update(apiUrl + id.ToString(), entrenador);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(entrenador);
            }
        }

        [Authorize(Roles = "Administrador")]
        // GET: EntrenadorController/Delete/5
        public ActionResult Delete(int id)
        {
            var data = APIConsumer<Entrenador>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador")]
        // POST: EntrenadorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Models.Entrenador entrenador)
        {
            try
            {
                var data = APIConsumer<Entrenador>.SelectOne(apiUrl + id.ToString());
                Usuario usuario = data.IdUsuNavigation;
                usuario.ActivoUsu = false;
                data.ActivoEnt = false;
                APIConsumer<Entrenador>.Update(apiUrl + id.ToString(), data);
                APIConsumer<Usuario>.Update(apiUrl.Replace("Entrenador", "Usuario") + usuario.IdUsu.ToString(), usuario);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(entrenador);
            }
        }
    }
}
