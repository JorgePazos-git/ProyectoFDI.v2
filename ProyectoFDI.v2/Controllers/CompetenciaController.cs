using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFDI.v2.Code;
using ProyectoFDI.v2.Models;
using System.Data;
using System.Globalization;

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

            ViewBag.ListaClasificados = listaClasificacion(id);

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

        public List<DetalleCompetencium> listaClasificacion(int id)
        {
            List<double> tiempos = new List<double>();
            List<double> mejoresTiempos = new List<double>();
            List<DetalleCompetencium> resultadosOrdenados = new List<DetalleCompetencium>();
            int falsos = 0;

            var lista = APIConsumer<DetalleCompetencium>.Select(apiUrl.Replace("Competencia", "DetalleCompetencia"))
                .Where(f => f.IdCom == id);

            var listaDetalles = lista.Select(f => new DetalleCompetencium
            {
                IdCom = f.IdCom,
                IdDep = f.IdDep,
                ClasRes = f.ClasRes,
                CuartosRes = f.CuartosRes,
                FinalRes = f.FinalRes,
                IdDetalle = f.IdDetalle,
                OctavosRes = f.OctavosRes,
                Puesto = f.Puesto,
                SemiRes = f.SemiRes,
                IdDepNavigation = f.IdDepNavigation
            }).ToList();


            // Recorremos la lista de resultados de clasificatoria y extraemos los tiempos
            CultureInfo culture = CultureInfo.InvariantCulture;
            foreach (DetalleCompetencium resultado in listaDetalles)
            {
                if(resultado.ClasRes != null)
                {
                    
                    double tiempo = double.Parse(resultado.ClasRes,culture);
                    tiempos.Add(tiempo);
                }
                else
                {
                    falsos++;
                }               
            }

            int cantidad = listaDetalles.Count - falsos;

            // Ordenamos los tiempos de menor a mayor y tomamos los primeros 16
            if (cantidad >= 16)
            {
                List<double> tiemposOrdenados = tiempos.OrderBy(t => t).ToList();
                mejoresTiempos = tiemposOrdenados.Take(16).ToList();
            }

            if (cantidad < 16 && cantidad >= 8)
            {
                List<double> tiemposOrdenados = tiempos.OrderBy(t => t).ToList();
                mejoresTiempos = tiemposOrdenados.Take(8).ToList();
            }

            if (cantidad < 8 && cantidad >= 4)
            {
                List<double> tiemposOrdenados = tiempos.OrderBy(t => t).ToList();
                mejoresTiempos = tiemposOrdenados.Take(4).ToList();
            }

            // Recorremos la lista de resultados de clasificatoria y agregamos los resultados con los mejores tiempos a una nueva lista
            foreach (DetalleCompetencium resultado in listaDetalles)
            {
                if (mejoresTiempos.Contains(Double.Parse(resultado.ClasRes, culture)))
                {
                    resultadosOrdenados.Add(resultado);
                }
            }

            // Ordenamos la lista de los 16 mejores resultados de menor a mayor tiempo
            resultadosOrdenados = resultadosOrdenados.OrderBy(r => Double.Parse(r.ClasRes, culture)).ToList();

            // Retornamos la lista de los 16 mejores resultados
            return resultadosOrdenados;

        }
    }
}
