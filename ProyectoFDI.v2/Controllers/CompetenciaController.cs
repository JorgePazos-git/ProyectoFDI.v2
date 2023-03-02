using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ProyectoFDI.v2.Code;
using ProyectoFDI.v2.Models;
using System.Data;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Data.SqlClient;
using ClosedXML.Excel;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.JSInterop;

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
            
            if(data.DetalleCompetencia.All(a => a.ClasRes != null))
            {
                ViewBag.ListaClasificados = listaClasificacion(id);
                if (data.DetalleCompetencia.Count >= 8 && data.DetalleCompetencia.Count < 16)
                {
                    if (data.DetalleCompetencia.Count(a => a.CuartosRes != null) >= 2)
                    {
                        ViewBag.ListaClasififadosCuartos = PrimeraVezEnfrentar(listaClasificacion(id), "cuartos");
                        if (data.DetalleCompetencia.Count(a => a.SemiRes != null) >= 2)
                        {
                            ViewBag.ListaClasificadosSemi = SiguienteRonda(PrimeraVezEnfrentar(listaClasificacion(id), "cuartos"), "semi");
                            if (data.DetalleCompetencia.Count(a => a.FinalRes != null) >= 2)
                            {
                                ViewBag.ListaClasificadosFinal = SiguienteRonda(SiguienteRonda(PrimeraVezEnfrentar(listaClasificacion(id), "cuartos"), "semi"), "final");
                            }
                        }

                    }
                    
                }
            }

            
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
                ViewBag.ListaClasificados = listaClasificacion(id);
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
            int puesto = 1;

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

            for(int i = 0; i < resultadosOrdenados.Count; i++)
            {
                resultadosOrdenados[i].Puesto = puesto;
                APIConsumer<DetalleCompetencium>.Update(apiUrl.Replace("Competencia", "DetalleCompetencia") +
                    resultadosOrdenados[i].IdDetalle, resultadosOrdenados[i]);

                puesto++;
            }

            puesto = 1;

            // Retornamos la lista de los 16 mejores resultados
            ViewBag.ListaClasifica = resultadosOrdenados;
            ViewBag.ListaClasificadosJSON = JsonSerializer.Serialize(resultadosOrdenados);
            return resultadosOrdenados;

        }
        public List<DetalleCompetencium> PrimeraVezEnfrentar(List<DetalleCompetencium> clasificados, string fase)
        {
            List<DetalleCompetencium> siguienteRonda = new List<DetalleCompetencium>();
            
            // Ordena la lista de deportistas por clasificación
            //if (fase == "octavos")
            //{
            //    clasificados = clasificados.OrderBy(d => d.OctavosRes).ToList();
            //}

            //if (fase == "cuartos")
            //{
            //    clasificados = clasificados.OrderBy(d => d.CuartosRes).ToList();
                
            //}

            //if (fase == "semi")
            //{
            //    clasificados = clasificados.OrderBy(d => d.SemiRes).ToList();
            //}

            // Enfrenta a los deportistas de las posiciones simétricas
            for (int i = 0; i < clasificados.Count / 2; i++)
            {
                DetalleCompetencium mejorDeportista = Enfrentar(clasificados[i], clasificados[clasificados.Count - i - 1], fase);
                siguienteRonda.Add(mejorDeportista);
            }

            if(fase == "octavos")
            {
                ViewBag.ListaCuartos = siguienteRonda;
                ViewBag.ListaCuartosJSON = JsonSerializer.Serialize(siguienteRonda);
            }

            if (fase == "cuartos")
            {
                ViewBag.ListaSemi = siguienteRonda;
                ViewBag.ListaSemiJSON = JsonSerializer.Serialize(siguienteRonda);
            }

            if (fase == "semi")
            {
                ViewBag.ListaFinal = siguienteRonda;
                ViewBag.ListaFinalJSON = JsonSerializer.Serialize(siguienteRonda);
            }

            return siguienteRonda;
        }

        public List<DetalleCompetencium> SiguienteRonda(List<DetalleCompetencium> clasificados, string fase)
        {
            List<DetalleCompetencium> siguienteRonda = new List<DetalleCompetencium>();

            // Enfrenta a los deportistas de las posiciones simétricas
            for (int i = 0; i < clasificados.Count / 2; i++)
            {
                DetalleCompetencium mejorDeportista = Enfrentar(clasificados[i], clasificados[clasificados.Count - i - 1], fase);
                siguienteRonda.Add(mejorDeportista);
            }

            if (fase == "cuartos")
            {
                ViewBag.ListaSemi = siguienteRonda;
                ViewBag.ListaSemiJSON = JsonSerializer.Serialize(siguienteRonda);
            }

            if (fase == "semi")
            {
                ViewBag.ListaFinal = siguienteRonda;
                ViewBag.ListaFinalJSON = JsonSerializer.Serialize(siguienteRonda);
            }

            if(fase == "final")
            {
                ViewBag.Ganador = siguienteRonda;
                ViewBag.GanadorJSON = JsonSerializer.Serialize(siguienteRonda);
            }

            return siguienteRonda;
        }

        private DetalleCompetencium Enfrentar(DetalleCompetencium deportista1, DetalleCompetencium deportista2, string fase)
        {
            // Simula un enfrentamiento entre los dos deportistas
            // y devuelve al mejor de ellos
            CultureInfo culture = CultureInfo.InvariantCulture;

            if(fase == "octavos")
            {
                if (Double.Parse(deportista1.OctavosRes, culture) > Double.Parse(deportista2.OctavosRes, culture))
                {
                    return deportista2;
                }
                else
                {
                    return deportista1;
                }
            }
            else
            {
                if (fase == "cuartos")
                {
                    if (Double.Parse(deportista1.CuartosRes, culture) > Double.Parse(deportista2.CuartosRes, culture))
                    {
                        return deportista2;
                    }
                    else
                    {
                        return deportista1;
                    }
                }
                else
                {
                    if(fase == "semi")
                    {
                        if (Double.Parse(deportista1.SemiRes, culture) > Double.Parse(deportista2.SemiRes, culture))
                        {
                            return deportista2;
                        }
                        else
                        {
                            return deportista1;
                        }
                    }
                    else
                    {
                        if (Double.Parse(deportista1.FinalRes, culture) > Double.Parse(deportista2.FinalRes, culture))
                        {
                            return deportista2;
                        }
                        else
                        {
                            return deportista1;
                        }
                    }
                }
            }
        }

        public FileResult ExportarExcel(int id)
        {
            var ganador = SiguienteRonda(SiguienteRonda(PrimeraVezEnfrentar(listaClasificacion(id), "cuartos"), "semi"), "final");
            var final = SiguienteRonda(PrimeraVezEnfrentar(listaClasificacion(id), "cuartos"), "semi");
            var semi = PrimeraVezEnfrentar(listaClasificacion(id), "cuartos");
            var cuartos = listaClasificacion(id);

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

            //Primer Lugar
            DetalleCompetencium detalle = APIConsumer<DetalleCompetencium>.SelectOne(apiUrl.Replace("Competencia", "DetalleCompetencia") + ganador[0].IdDetalle.ToString());
            detalle.Puesto = 1;
            APIConsumer<DetalleCompetencium>.Update(apiUrl.Replace("Competencia", "DetalleCompetencia") + ganador[0].IdDetalle.ToString(), detalle);

            //Segundo Lugar
            DetalleCompetencium segundo = new DetalleCompetencium();

            for(int i = 0; i < final.Count; i++)
            {
                if (final[i].IdDetalle != ganador[0].IdDetalle)
                {
                    segundo = APIConsumer<DetalleCompetencium>.SelectOne(apiUrl.Replace("Competencia", "DetalleCompetencia") + final[i].IdDetalle.ToString());
                    segundo.Puesto = 2;
                    APIConsumer<DetalleCompetencium>.Update(apiUrl.Replace("Competencia", "DetalleCompetencia") + segundo.IdDetalle.ToString(), segundo);
                }
            }

            //Tercer Lugar / Cuarto Lugar
            semi.RemoveAll(deportista => deportista.IdDetalle == ganador[0].IdDetalle);
            semi.RemoveAll(deportista => deportista.IdDetalle == segundo.IdDetalle);
            semi = semi.OrderByDescending(d => Double.Parse(d.SemiRes)).ToList();
            DetalleCompetencium tercero = new DetalleCompetencium();
            DetalleCompetencium cuarto = new DetalleCompetencium();


            tercero = APIConsumer<DetalleCompetencium>.SelectOne(apiUrl.Replace("Competencia", "DetalleCompetencia") + semi[0].IdDetalle.ToString());
            tercero.Puesto = 3;
            APIConsumer<DetalleCompetencium>.Update(apiUrl.Replace("Competencia", "DetalleCompetencia") + tercero.IdDetalle.ToString(), tercero);

            cuarto = APIConsumer<DetalleCompetencium>.SelectOne(apiUrl.Replace("Competencia", "DetalleCompetencia") + semi[1].IdDetalle.ToString());
            cuarto.Puesto = 4;
            APIConsumer<DetalleCompetencium>.Update(apiUrl.Replace("Competencia", "DetalleCompetencia") + cuarto.IdDetalle.ToString(), cuarto);

            //Quinto a Octavo
            cuartos.RemoveAll(deportista => deportista.IdDetalle == ganador[0].IdDetalle);
            cuartos.RemoveAll(deportista => deportista.IdDetalle == segundo.IdDetalle);
            cuartos.RemoveAll(deportista => deportista.IdDetalle == tercero.IdDetalle);
            cuartos.RemoveAll(deportista => deportista.IdDetalle == cuarto.IdDetalle);
            cuartos = cuartos.OrderBy(d => Double.Parse(d.CuartosRes)).ToList();

            DetalleCompetencium quinto = APIConsumer<DetalleCompetencium>.SelectOne(apiUrl.Replace("Competencia", "DetalleCompetencia") + cuartos[0].IdDetalle.ToString());
            quinto.Puesto = 5;
            APIConsumer<DetalleCompetencium>.Update(apiUrl.Replace("Competencia", "DetalleCompetencia") + quinto.IdDetalle.ToString(), quinto);

            DetalleCompetencium sexto = APIConsumer<DetalleCompetencium>.SelectOne(apiUrl.Replace("Competencia", "DetalleCompetencia") + cuartos[1].IdDetalle.ToString());
            sexto.Puesto = 6;
            APIConsumer<DetalleCompetencium>.Update(apiUrl.Replace("Competencia", "DetalleCompetencia") + sexto.IdDetalle.ToString(), sexto);

            DetalleCompetencium septimo = APIConsumer<DetalleCompetencium>.SelectOne(apiUrl.Replace("Competencia", "DetalleCompetencia") + cuartos[2].IdDetalle.ToString());
            septimo.Puesto = 7;
            APIConsumer<DetalleCompetencium>.Update(apiUrl.Replace("Competencia", "DetalleCompetencia") + septimo.IdDetalle.ToString(), septimo);

            DetalleCompetencium octavo = APIConsumer<DetalleCompetencium>.SelectOne(apiUrl.Replace("Competencia", "DetalleCompetencia") + cuartos[3].IdDetalle.ToString());
            octavo.Puesto = 8;
            APIConsumer<DetalleCompetencium>.Update(apiUrl.Replace("Competencia", "DetalleCompetencia") + octavo.IdDetalle.ToString(), octavo);


            //Demas
            listaDetalles.RemoveAll(deportista => deportista.IdDetalle == ganador[0].IdDetalle);
            listaDetalles.RemoveAll(deportista => deportista.IdDetalle == segundo.IdDetalle);
            listaDetalles.RemoveAll(deportista => deportista.IdDetalle == tercero.IdDetalle);
            listaDetalles.RemoveAll(deportista => deportista.IdDetalle == cuarto.IdDetalle);
            listaDetalles.RemoveAll(deportista => deportista.IdDetalle == quinto.IdDetalle);
            listaDetalles.RemoveAll(deportista => deportista.IdDetalle == sexto.IdDetalle);
            listaDetalles.RemoveAll(deportista => deportista.IdDetalle == septimo.IdDetalle);
            listaDetalles.RemoveAll(deportista => deportista.IdDetalle == octavo.IdDetalle);

            listaDetalles = listaDetalles.OrderBy(d => Double.Parse(d.ClasRes)).ToList();
            int puesto = 9;

            for (int i = 0; i < listaDetalles.Count; i++)
            {
                DetalleCompetencium data = APIConsumer<DetalleCompetencium>.SelectOne(apiUrl.Replace("Competencia", "DetalleCompetencia") + listaDetalles[i].IdDetalle.ToString());
                data.Puesto = puesto;
                APIConsumer<DetalleCompetencium>.Update(apiUrl.Replace("Competencia", "DetalleCompetencia") + data.IdDetalle.ToString(), data);
                puesto++;
            }

            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection("Data Source=proyectofdi.database.windows.net;Initial Catalog=ProyectoFDI.v2;User ID=proyectofdi;Password=Allistar123.;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("select dt.puesto as PUESTO, CONCAT(dp.nombres_dep,' ',dp.apellidos_dep) as DEPORTISTA" +
                    ", dt.clas_res as ResultadoClasificacion, dt.octavos_res as ResultadoOctavos" +
                    ", dt.cuartos_res as ResultadoCuartos, dt.semi_res as ResultadoSemiFinal, dt.final_res as ResultadoFinal" +
                    " from detalle_competencia dt INNER JOIN deportista dp on dt.id_dep = dp.id_dep where id_com = " + id +
                    " group by dt.puesto, dp.nombres_dep, dp.apellidos_dep, dt.clas_res, dt.octavos_res, dt.cuartos_res, dt.semi_res, dt.final_res order by puesto");

                sb.AppendLine("select * from detalle_competencia where id_com =" + id);

                SqlCommand cmd = new SqlCommand(sb.ToString(), cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            dt.TableName = "Reporte Competencia";

            using (XLWorkbook libro = new XLWorkbook())
            {
                var hoja = libro.Worksheets.Add(dt);

                hoja.ColumnsUsed().AdjustToContents();

                using (MemoryStream stream = new MemoryStream())
                {
                    libro.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte Competencia" + ".xlsx");
                }
            }
        }

        public void AgregarResult(int id, string result, string fase)
        {
                DetalleCompetencium detalle = APIConsumer<DetalleCompetencium>.SelectOne(
                    apiUrl.Replace("Competencia", "DetalleCompetencia") + id.ToString());

                if(fase == "octavos")
                {
                    detalle.OctavosRes = result;
                }
                if (fase == "cuartos")
                {
                    detalle.CuartosRes = result;
                }
                if (fase == "semi")
                {
                    detalle.SemiRes = result;
                }
                if (fase == "final")
                {
                    detalle.FinalRes = result;
                }

                APIConsumer<DetalleCompetencium>.Update(apiUrl.Replace("Competencia", "DetalleCompetencia")
                    + id.ToString(), detalle);
        }
    }
}
