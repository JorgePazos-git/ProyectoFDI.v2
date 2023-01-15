using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFDI.v2.Code;
using ProyectoFDI.v2.Models;
using System.Runtime.Intrinsics.Arm;

namespace ProyectoFDI.v2.Controllers
{
    public class DeportistaController : Controller
    {
        private string apiUrl;

        public DeportistaController(IConfiguration configuration)
        {
            apiUrl = configuration["urlBase"].ToString() + "Deportista/";
        }

        // GET: DeportistaController
        public ActionResult Index(string? searchFor)
        {
            ViewBag.SearchFor = "" + searchFor;

            if (string.IsNullOrWhiteSpace(searchFor))
            {
                return View(APIConsumer<Deportistum>.Select(apiUrl));
            }
            else
            {
                return View(APIConsumer<Deportistum>.Select_SearchFor(apiUrl, searchFor));
            }
        }

        private List<SelectListItem> listaModalidades()
        {
            var modalidades = APIConsumer<Modalidad>.Select(apiUrl.Replace("Deportista", "Modalidad"));
            var lista = modalidades.Select(f => new SelectListItem
            {
                Value = f.IdMod.ToString(),
                Text = f.DescripcionMod
            }).ToList();

            return lista;
        }

        private List<SelectListItem> listaProvincias()
        { 
            var provincias = APIConsumer<Provincium>.Select(apiUrl.Replace("Deportista", "Provincia"));
            var lista = provincias.Select(f => new SelectListItem
            {
                Value = f.IdPro.ToString(),
                Text = f.NombrePro
            }).ToList();

            return lista;
        }

        private List<SelectListItem> listaCategorias()
        {
            var categorias = APIConsumer<Categorium>.Select(apiUrl.Replace("Deportista", "Categoria"));
            var lista = categorias.Select(f => new SelectListItem
            {
                Value = f.IdCat.ToString(),
                Text = f.NombreCat
            }).ToList();

            return lista;
        }

        private List<SelectListItem> listaClubes()
        {
            var clubes = APIConsumer<Club>.Select(apiUrl.Replace("Deportista", "Club"));
            var lista = clubes.Select(f => new SelectListItem
            {
                Value = f.IdClub.ToString(),
                Text = f.NombreClub
            }).ToList();

            return lista;
        }

        private List<SelectListItem> listaGeneros()
        {
            var generos = APIConsumer<Genero>.Select(apiUrl.Replace("Deportista", "Genero"));
            var lista = generos.Select(f => new SelectListItem
            {
                Value = f.IdGen.ToString(),
                Text = f.NombreGen
            }).ToList();

            return lista;
        }

        private List<SelectListItem> listaEntrenadores()
        {
            var entrenadores = APIConsumer<Entrenador>.Select(apiUrl.Replace("Deportista", "Entrenador"));
            var lista = entrenadores.Select(f => new SelectListItem
            {
                Value = f.IdEnt.ToString(),
                Text = f.NombresEnt + " " + f.ApellidosEnt,
            }).ToList();

            return lista;
        }

        private List<SelectListItem> listaUsuarios()
        {
            var usuarios = APIConsumer<Usuario>.Select(apiUrl.Replace("Deportista", "Usuario"));
            var lista = usuarios.Select(f => new SelectListItem
            {
                Value = f.IdUsu.ToString(),
                Text = f.NombreUsu
            }).ToList();

            return lista;
        }


        // GET: DeportistaController/Details/5
        public ActionResult Details(int id)
        {
            var data = APIConsumer<Deportistum>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        // GET: DeportistaController/Create
        public ActionResult Create()
        {
            ViewBag.ListaCategorias = listaCategorias();
            ViewBag.ListaClubes = listaClubes();
            ViewBag.ListaEntrenadores = listaEntrenadores();
            ViewBag.ListaGeneros = listaGeneros();
            ViewBag.ListaModalidades = listaModalidades();
            ViewBag.ListaProvincias = listaProvincias();
            ViewBag.ListaUsuarios = listaUsuarios();
            return View();
        }

        // POST: DeportistaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Deportistum deportista)
        {
            try
            {
                APIConsumer<Deportistum>.Insert(apiUrl, deportista);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(deportista);
            }
        }

        // GET: DeportistaController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.ListaCategorias = listaCategorias();
            ViewBag.ListaClubes = listaClubes();
            ViewBag.ListaEntrenadores = listaEntrenadores();
            ViewBag.ListaGeneros = listaGeneros();
            ViewBag.ListaModalidades = listaModalidades();
            ViewBag.ListaProvincias = listaProvincias();
            ViewBag.ListaUsuarios = listaUsuarios();
            var data = APIConsumer<Deportistum>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        // POST: DeportistaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Models.Deportistum deportista)
        {
            try
            {
                APIConsumer<Deportistum>.Update(apiUrl + id.ToString(), deportista);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(deportista);
            }
        }

        // GET: DeportistaController/Delete/5
        public ActionResult Delete(int id)
        {
            var data = APIConsumer<Deportistum>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        // POST: DeportistaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Models.Deportistum deportista)
        {
            try
            {
                APIConsumer<Deportistum>.Delete(apiUrl + id.ToString());
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(deportista);
            }
        }
    
    }
}
