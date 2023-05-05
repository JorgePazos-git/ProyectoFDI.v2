using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFDI.v2.Code;
using ProyectoFDI.v2.Models;
using System.Data;

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
            ViewBag.ListaCategorias = listaCategorias();
            ViewBag.ListaClubes = listaClubes();
            ViewBag.ListaEntrenadores = listaEntrenadores();
            ViewBag.ListaGeneros = listaGeneros();
            ViewBag.ListaModalidades = listaModalidades();
            ViewBag.ListaProvincias = listaProvincias();
            ViewBag.ListaUsuarios = listaUsuarios();
            ViewBag.ListaEstados = listaEstados();
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

        private List<Modalidad> listaModalidades()
        {
            var modalidades = APIConsumer<Modalidad>.Select(apiUrl.Replace("Deportista", "Modalidad"));
            var lista = modalidades.Select(f => new Modalidad
            {
                IdMod = f.IdMod,
                DescripcionMod = f.DescripcionMod
            }).ToList();

            return lista;
        }

        private List<Provincium> listaProvincias()
        {
            var provincias = APIConsumer<Provincium>.Select(apiUrl.Replace("Deportista", "Provincia"));
            var lista = provincias.Select(f => new Provincium
            {
                IdPro = f.IdPro,
                NombrePro = f.NombrePro
            }).ToList();

            return lista;
        }

        private List<Categorium> listaCategorias()
        {
            var categorias = APIConsumer<Categorium>.Select(apiUrl.Replace("Deportista", "Categoria"));
            var lista = categorias.Select(f => new Categorium
            {
                IdCat = f.IdCat,
                NombreCat = f.NombreCat
            }).ToList();

            return lista;
        }

        private List<Club> listaClubes()
        {
            var clubes = APIConsumer<Club>.Select(apiUrl.Replace("Deportista", "Club"));
            var lista = clubes.Select(f => new Club
            {
                IdClub = f.IdClub,
                NombreClub = f.NombreClub
            }).ToList();

            return lista;
        }

        private List<Genero> listaGeneros()
        {
            var generos = APIConsumer<Genero>.Select(apiUrl.Replace("Deportista", "Genero"));
            var lista = generos.Select(f => new Genero
            {
                IdGen = f.IdGen,
                NombreGen = f.NombreGen
            }).ToList();

            return lista;
        }

        private List<Entrenador> listaEntrenadores()
        {
            var entrenadores = APIConsumer<Entrenador>.Select(apiUrl.Replace("Deportista", "Entrenador"));
            var lista = entrenadores.Select(f => new Entrenador
            {
                IdEnt = f.IdEnt,
                NombresEnt = f.NombresEnt,
                ApellidosEnt = f.ApellidosEnt
            }).ToList();

            return lista;
        }

        private List<Usuario> listaUsuarios()
        {
            var usuarios = APIConsumer<Usuario>.Select(apiUrl.Replace("Deportista", "Usuario"));
            var lista = usuarios.Select(f => new Usuario
            {
                IdUsu = f.IdUsu,
                NombreUsu = f.NombreUsu
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

        private List<SelectListItem> listadoModalidades()
        {
            var modalidades = APIConsumer<Modalidad>.Select(apiUrl.Replace("Deportista", "Modalidad"));
            var lista = modalidades.Select(f => new SelectListItem
            {
                Value = f.IdMod.ToString(),
                Text = f.DescripcionMod
            }).ToList();

            return lista;
        }

        private List<SelectListItem> listadoProvincias()
        {
            var provincias = APIConsumer<Provincium>.Select(apiUrl.Replace("Deportista", "Provincia"));
            var lista = provincias.Select(f => new SelectListItem
            {
                Value = f.IdPro.ToString(),
                Text = f.NombrePro
            }).ToList();

            return lista;
        }

        private List<SelectListItem> listadoCategorias()
        {
            var categorias = APIConsumer<Categorium>.Select(apiUrl.Replace("Deportista", "Categoria"));
            var lista = categorias.Select(f => new SelectListItem
            {
                Value = f.IdCat.ToString(),
                Text = f.NombreCat
            }).ToList();

            return lista;
        }

        private List<SelectListItem> listadoClubes()
        {
            var clubes = APIConsumer<Club>.Select(apiUrl.Replace("Deportista", "Club"));
            var lista = clubes.Select(f => new SelectListItem
            {
                Value = f.IdClub.ToString(),
                Text = f.NombreClub
            }).ToList();

            return lista;
        }

        private List<SelectListItem> listadoGeneros()
        {
            var generos = APIConsumer<Genero>.Select(apiUrl.Replace("Deportista", "Genero"));
            var lista = generos.Select(f => new SelectListItem
            {
                Value = f.IdGen.ToString(),
                Text = f.NombreGen
            }).ToList();

            return lista;
        }

        private List<SelectListItem> listadoEntrenadores()
        {
            var entrenadores = APIConsumer<Entrenador>.Select(apiUrl.Replace("Deportista", "Entrenador"));
            var lista = entrenadores.Select(f => new SelectListItem
            {
                Value = f.IdEnt.ToString(),
                Text = f.NombresEnt + " " + f.ApellidosEnt
            }).ToList();

            return lista;
        }

        private List<SelectListItem> listadoUsuarios()
        {
            var usuarios = APIConsumer<Usuario>.Select(apiUrl.Replace("Deportista", "Usuario"));
            var lista = usuarios.Select(f => new SelectListItem
            {
                Value = f.IdUsu.ToString(),
                Text = f.NombreUsu
            }).ToList();

            return lista;
        }


        [Authorize(Roles = "Administrador,Deportista,Juez,Entrenador")]
        // GET: DeportistaController/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.ListaCategorias = listaCategorias();
            ViewBag.ListaClubes = listaClubes();
            ViewBag.ListaEntrenadores = listaEntrenadores();
            ViewBag.ListaGeneros = listaGeneros();
            ViewBag.ListaModalidades = listaModalidades();
            ViewBag.ListaProvincias = listaProvincias();
            ViewBag.ListaUsuarios = listaUsuarios();

            ViewBag.ListaEstados = listaEstados();
            var data = APIConsumer<Deportistum>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador,Entrenador")]
        // GET: DeportistaController/Create
        public ActionResult Create()
        {
            ViewBag.ListaEstados = listaEstados();

            ViewBag.ListadoCategorias = listadoCategorias();
            ViewBag.ListadoClubes = listadoClubes();
            ViewBag.ListadoEntrenadores = listadoEntrenadores();
            ViewBag.ListadoGeneros = listadoGeneros();
            ViewBag.ListadoModalidades = listadoModalidades();
            ViewBag.ListadoProvincias = listadoProvincias();
            ViewBag.ListadoUsuarios = listadoUsuarios();

            return View();
        }

        [Authorize(Roles = "Administrador,Entrenador")]
        // POST: DeportistaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Deportistum deportista)
        {
            try
            {
                if (!string.IsNullOrEmpty(deportista.CedulaDep))
                {
                    Usuario usuario = new Usuario();
                    usuario.IdUsu = 0;
                    usuario.NombreUsu = deportista.CedulaDep;
                    usuario.ClaveUsu = deportista.CedulaDep;
                    usuario.FechaCreacion = DateTime.Now;
                    usuario.RolesUsu = "Deportista";
                    usuario.ActivoUsu = true;

                    deportista.IdUsuNavigation = usuario;
                }

                APIConsumer<Deportistum>.Insert(apiUrl, deportista);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(deportista);
            }
        }

        [Authorize(Roles = "Administrador,Entrenador")]
        // GET: DeportistaController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.ListadoCategorias = listadoCategorias();
            ViewBag.ListadoClubes = listadoClubes();
            ViewBag.ListadoEntrenadores = listadoEntrenadores();
            ViewBag.ListadoGeneros = listadoGeneros();
            ViewBag.ListadoModalidades = listadoModalidades();
            ViewBag.ListadoProvincias = listadoProvincias();
            ViewBag.ListadoUsuarios = listadoUsuarios();
            ViewBag.ListaEstados = listaEstados();
            var data = APIConsumer<Deportistum>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador,Entrenador")]
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
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(deportista);
            }
        }

        [Authorize(Roles = "Administrador,Entrenador")]
        // GET: DeportistaController/Delete/5
        public ActionResult Delete(int id)
        {
            var data = APIConsumer<Deportistum>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador,Entrenador")]
        // POST: DeportistaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Models.Deportistum deportista)
        {
            try
            {
                var data = APIConsumer<Deportistum>.SelectOne(apiUrl + id.ToString());
                Usuario usuario = data.IdUsuNavigation;
                usuario.ActivoUsu = false;
                data.ActivoDep = false;
                APIConsumer<Deportistum>.Update(apiUrl + id.ToString(), data);
                APIConsumer<Usuario>.Update(apiUrl.Replace("Deportista", "Usuario") + usuario.IdUsu.ToString(), usuario);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(deportista);
            }
        }

    }
}
