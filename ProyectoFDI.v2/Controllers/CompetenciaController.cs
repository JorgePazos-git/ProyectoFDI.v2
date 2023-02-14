﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFDI.v2.Code;
using ProyectoFDI.v2.Models;
using System.Data;

namespace ProyectoFDI.v2.Controllers
{
    public class CompetenciaController : Controller
    {
        private string apiUrl;

        public CompetenciaController(IConfiguration configuration)
        {
            apiUrl = configuration["urlBase"].ToString() + "Competencia/";
        }

        // GET: CompetenciaController
        public ActionResult Index(string? searchFor)
        {
            ViewBag.ListadoCategorias = listadoCategorias();
            ViewBag.ListadoGeneros = listadoGeneros();
            ViewBag.ListadoModalidades = listadoModalidades();
            ViewBag.ListadoJueces = listadoJueces();
            ViewBag.ListadoSedes = listadoSedes();

            ViewBag.SearchFor = "" + searchFor;

            if (string.IsNullOrWhiteSpace(searchFor))
            {
                return View(APIConsumer<Competencium>.Select(apiUrl));
            }
            else
            {
                return View(APIConsumer<Competencium>.Select_SearchFor(apiUrl, searchFor));
            }
        }

        [Authorize(Roles = "Administrador,Juez")]
        public ActionResult Resultados(string? searchFor)
        {
            ViewBag.ListadoCategorias = listadoCategorias();
            ViewBag.ListadoGeneros = listadoGeneros();
            ViewBag.ListadoModalidades = listadoModalidades();
            ViewBag.ListadoJueces = listadoJueces();
            ViewBag.ListadoSedes = listadoSedes();

            ViewBag.SearchFor = "" + searchFor;

            if (string.IsNullOrWhiteSpace(searchFor))
            {
                return View(APIConsumer<Competencium>.Select(apiUrl));
            }
            else
            {
                return View(APIConsumer<Competencium>.Select_SearchFor(apiUrl, searchFor));
            }
        }

        private List<SelectListItem> listaModalidades()
        {
            var modalidades = APIConsumer<Modalidad>.Select(apiUrl.Replace("Competencia", "Modalidad"));
            var lista = modalidades.Select(f => new SelectListItem
            {
                Value = f.IdMod.ToString(),
                Text = f.DescripcionMod
            }).ToList();

            return lista;
        }

        private List<SelectListItem> listaCategorias()
        {
            var categorias = APIConsumer<Categorium>.Select(apiUrl.Replace("Competencia", "Categoria"));
            var lista = categorias.Select(f => new SelectListItem
            {
                Value = f.IdCat.ToString(),
                Text = f.NombreCat
            }).ToList();

            return lista;
        }

        private List<SelectListItem> listaGeneros()
        {
            var generos = APIConsumer<Genero>.Select(apiUrl.Replace("Competencia", "Genero"));
            var lista = generos.Select(f => new SelectListItem
            {
                Value = f.IdGen.ToString(),
                Text = f.NombreGen
            }).ToList();

            return lista;
        }

        private List<SelectListItem> listaSedes()
        {
            var sedes = APIConsumer<Sede>.Select(apiUrl.Replace("Competencia", "Sede"));
            var lista = sedes.Select(f => new SelectListItem
            {
                Value = f.IdSede.ToString(),
                Text = f.NombreSede
            }).ToList();

            return lista;
        }

        private List<SelectListItem> listaJueces()
        {
            var jueces = APIConsumer<Juez>.Select(apiUrl.Replace("Competencia", "Juez"));
            var lista = jueces.Select(f => new SelectListItem
            {
                Value = f.IdJuez.ToString(),
                Text = f.NombresJuez + " " + f.ApellidosJuez
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

        private List<Modalidad> listadoModalidades()
        {
            var modalidades = APIConsumer<Modalidad>.Select(apiUrl.Replace("Competencia", "Modalidad"));
            var lista = modalidades.Select(f => new Modalidad
            {
                IdMod = f.IdMod,
                DescripcionMod = f.DescripcionMod
            }).ToList();

            return lista;
        }

        private List<Categorium> listadoCategorias()
        {
            var categorias = APIConsumer<Categorium>.Select(apiUrl.Replace("Competencia", "Categoria"));
            var lista = categorias.Select(f => new Categorium
            {
                IdCat = f.IdCat,
                NombreCat = f.NombreCat
            }).ToList();

            return lista;
        }

        private List<Genero> listadoGeneros()
        {
            var generos = APIConsumer<Genero>.Select(apiUrl.Replace("Competencia", "Genero"));
            var lista = generos.Select(f => new Genero
            {
                IdGen = f.IdGen,
                NombreGen = f.NombreGen
            }).ToList();

            return lista;
        }

        private List<Sede> listadoSedes()
        {
            var sedes = APIConsumer<Sede>.Select(apiUrl.Replace("Competencia", "Sede"));
            var lista = sedes.Select(f => new Sede
            {
                IdSede = f.IdSede,
                NombreSede = f.NombreSede
            }).ToList();

            return lista;
        }

        private List<Juez> listadoJueces()
        {
            var jueces = APIConsumer<Juez>.Select(apiUrl.Replace("Competencia", "Juez"));
            var lista = jueces.Select(f => new Juez
            {
                IdJuez = f.IdJuez,
                NombresJuez = f.NombresJuez,
                ApellidosJuez = f.ApellidosJuez
            }).ToList();
            return lista;
        }

        [Authorize(Roles = "Administrador,Deportista,Juez,Entrenador")]
        // GET: CompetenciaController/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.ListadoCategorias = listadoCategorias();
            ViewBag.ListadoGeneros = listadoGeneros();
            ViewBag.ListadoModalidades = listadoModalidades();
            ViewBag.ListadoJueces = listadoJueces();
            ViewBag.ListadoSedes = listadoSedes();

            var data = APIConsumer<Competencium>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador,Juez")]
        // GET: CompetenciaController/Create
        public ActionResult Create()
        {
            ViewBag.ListaCategorias = listaCategorias();
            ViewBag.ListaGeneros = listaGeneros();
            ViewBag.ListaModalidades = listaModalidades();
            ViewBag.ListaJueces = listaJueces();
            ViewBag.ListaSedes = listaSedes();
            ViewBag.ListadoEstados = listaEstados();
            return View();
        }

        [Authorize(Roles = "Administrador,Juez")]
        // POST: CompetenciaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Competencium competencia)
        {
            try
            {
                APIConsumer<Competencium>.Insert(apiUrl, competencia);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(competencia);
            }
        }

        [Authorize(Roles = "Administrador,Juez")]
        // GET: CompetenciaController/Edit/5
        public ActionResult Edit(int id)
        {
            var data = APIConsumer<Competencium>.SelectOne(apiUrl + id.ToString());
            ViewBag.ListaCategorias = listaCategorias();
            ViewBag.ListaGeneros = listaGeneros();
            ViewBag.ListaModalidades = listaModalidades();
            ViewBag.ListaJueces = listaJueces();
            ViewBag.ListaSedes = listaSedes();
            ViewBag.ListadoEstados = listaEstados();

            return View(data);
        }

        [Authorize(Roles = "Administrador,Juez")]
        public ActionResult AgregarResultados(int id)
        {
            var data = APIConsumer<Competencium>.SelectOne(apiUrl + id.ToString());
            ViewBag.ListaCategorias = listaCategorias();
            ViewBag.ListaGeneros = listaGeneros();
            ViewBag.ListaModalidades = listaModalidades();
            ViewBag.ListaJueces = listaJueces();
            ViewBag.ListaSedes = listaSedes();
            ViewBag.ListadoEstados = listaEstados();

            return View(data);
        }

        [Authorize(Roles = "Administrador,Juez")]
        // POST: CompetenciaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Competencium competencia)
        {
            try
            {
                APIConsumer<Competencium>.Update(apiUrl + id.ToString(), competencia);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(competencia);
            }
        }

        [Authorize(Roles = "Administrador,Juez")]
        // POST: CompetenciaController/AgregarResultados/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarResultados(int id, Competencium competencia)
        {
            try
            {
                APIConsumer<Competencium>.Update(apiUrl + id.ToString(), competencia);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(competencia);
            }
        }

        [Authorize(Roles = "Administrador,Juez")]
        // GET: CompetenciaController/Delete/5
        public ActionResult Delete(int id)
        {
            var data = APIConsumer<Competencium>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador,Juez")]
        // POST: CompetenciaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Competencium competencia)
        {
            try
            {
                var data = APIConsumer<Competencium>.SelectOne(apiUrl + id.ToString());
                data.ActivoCom = false;
                APIConsumer<Competencium>.Update(apiUrl + id.ToString(), data);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(competencia);
            }
        }
    }
}
