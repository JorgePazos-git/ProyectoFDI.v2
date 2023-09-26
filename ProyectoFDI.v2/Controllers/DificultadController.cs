using DocumentFormat.OpenXml.Office2013.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFDI.v2.Code;
using ProyectoFDI.v2.Models;

namespace ProyectoFDI.v2.Controllers
{
    public class DificultadController : Controller
    {
        private string apiUrl;
        private int idCom;

        public DificultadController(IConfiguration configuration)
        {
            apiUrl = configuration["urlBase"].ToString() + "DetalleCompetenciaDificultad/";
        }

        private List<SelectListItem> listaDeportistas()
        {
            var deportistas = APIConsumer<Deportistum>.Select(apiUrl.Replace("DetalleCompetenciaDificultad", "Deportista"));
            var lista = deportistas.Select(f => new SelectListItem
            {
                Value = f.IdDep.ToString(),
                Text = f.NombresDep + " " + f.ApellidosDep
            }).ToList();

            return lista;
        }

        private List<SelectListItem> listaCompetencias()
        {
            var deportistas = APIConsumer<Competencium>.Select(apiUrl.Replace("DetalleCompetenciaDificultad", "Competencia"));
            var lista = deportistas.Select(f => new SelectListItem
            {
                Value = f.IdCom.ToString(),
                Text = f.NombreCom
            }).ToList();

            return lista;
        }

        // GET: HomeController1
        public ActionResult Index(int competencia)
        {
            this.idCom = competencia;
            ViewBag.idcompetencia = competencia;
            GetCompetencia(idCom);

            return View(APIConsumer<DetalleCompetenciaDificultad>.Select(apiUrl));
        }

        public ActionResult lista()
        {
            return View(APIConsumer<DetalleCompetenciaDificultad>.Select(apiUrl));
        }

        private void GetCompetencia(int idCom)
        {

            string newUrl = apiUrl.Replace("DetalleCompetenciaDificultad",
                "Competencia") + idCom.ToString();

            Competencium competencia = APIConsumer<Competencium>.SelectOne(newUrl);

            ViewBag.competencia = competencia;
        }

        // GET: HomeController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeController1/Create
        public ActionResult Create(int idCom)
        {
            ViewBag.compe = idCom;
            ViewBag.listaDeportistas = listaDeportistas();
            ViewBag.listaCompetencias = listaCompetencias();
            List<DetalleCompetenciaDificultad> lista = new List<DetalleCompetenciaDificultad>();
            DetalleCompetenciaDificultad n = new DetalleCompetenciaDificultad();
            lista.Add(n);
            return View(lista);
        }

        // POST: HomeController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(List<DetalleCompetenciaDificultad> detalles)
        {
            try
            {
                foreach(DetalleCompetenciaDificultad detalle in detalles)
                {
                    APIConsumer<DetalleCompetenciaDificultad>.Insert(apiUrl, detalle);
                }
                    
               
                return RedirectToAction("Edit", "Competencia", new { id = detalles[0].IdCom });

            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult AgregarClasificaciones(int idDetalle, string deportistaNombre, string clasificacion1, string clasificacion2)
        {
            try
            {
                DetalleCompetenciaDificultad detalleOld = APIConsumer<DetalleCompetenciaDificultad>.SelectOne(apiUrl + idDetalle.ToString());
                DetalleCompetenciaDificultad detallenew = detalleOld;

                detallenew.Clas1Res = clasificacion1;
                detallenew.Clas2Res = clasificacion2;

                try
                {
                    APIConsumer<DetalleCompetenciaDificultad>.Update(apiUrl + idDetalle.ToString(), detallenew);
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, error = ex.Message });
                }

                
            }
            catch (Exception ex)
            {
                // Si ocurre un error, puedes devolver una respuesta de error.
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult AsignarPuestos(int idCompetencia)
        {
            try
            {
                // Obtener detalles y calcular puestos de ambas clasificaciones
                var detalles = APIConsumer<DetalleCompetenciaDificultad>.Select(apiUrl).Where(f => f.IdCom == idCompetencia).ToList();
                List<Tuple<int, int>> puestosClas1 = CalcularPuestosClasificacion(detalles, 1);
                List<Tuple<int, int>> puestosClas2 = CalcularPuestosClasificacion(detalles, 2);

                // Calcular puestos basados en ambos resultados
                var puestosCalculo = puestosClas1
                    .Join(puestosClas2, t1 => t1.Item1, t2 => t2.Item1, (t1, t2) => new { DeportistaId = t1.Item1, PuestoClas1 = t1.Item2, PuestoClas2 = t2.Item2 })
                    .Select(x => new { x.DeportistaId, Resultado = Math.Sqrt(x.PuestoClas1 * x.PuestoClas2) })
                    .OrderBy(x => x.Resultado)
                    .ToList();

                // Asignar puestos a los detalles
                for (int i = 0; i < puestosCalculo.Count; i++)
                {
                    int deportistaId = puestosCalculo[i].DeportistaId;
                    DetalleCompetenciaDificultad detalle = detalles.FirstOrDefault(d => d.IdDep == deportistaId);

                    if (detalle != null)
                    {
                        detalle.PuestoInicialRes = i + 1; // +1 porque los puestos comienzan en 1
                        APIConsumer<DetalleCompetenciaDificultad>.Update(apiUrl + detalle.IdDetalleDificultad.ToString(), detalle);
                    }
                }

                // Obtener deportistas ordenados
                var deportistasOrdenados = detalles
                    .OrderBy(d => d.PuestoInicialRes)
                    .Select(f => new DetalleCompetenciaDificultad
                    {
                        IdDetalleDificultad = f.IdDetalleDificultad,
                        Puesto = f.Puesto,
                        Clas1Res = f.Clas1Res,
                        Clas2Res = f.Clas2Res,
                        FinalRes = f.FinalRes,
                        IdCom = f.IdCom,
                        IdDep = f.IdDep,
                        IdComNavigation = f.IdComNavigation,
                        IdDepNavigation = f.IdDepNavigation,
                        PuestoInicialRes = f.PuestoInicialRes
                    }).ToList();

                return Json(new { success = true, deportistas = deportistasOrdenados });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        private List<Tuple<int, int>> CalcularPuestosClasificacion(List<DetalleCompetenciaDificultad> detalles, int tipo)
        {
            List<Tuple<int, int>> puestos = new List<Tuple<int, int>>();

            if (detalles != null && detalles.Any())
            {
                if(tipo == 1)
                {
                    var detallesOrdenados = detalles.OrderByDescending(d => d.Clas1Res).ToList();


                    int puesto = 1;
                    int empate = 1;

                    string resultadoAnterior = detallesOrdenados[0].Clas1Res;

                    foreach (var detalle in detallesOrdenados)
                    {
                        if (detalle.Clas1Res == "top")
                        {
                            // Si es "top", asignar el puesto y continuar
                            puestos.Add(new Tuple<int, int>(detalle.IdDep.Value, puesto));
                        }
                        if (detalle.Clas1Res.EndsWith("+"))
                        {
                            // Si termina con "+", es un número sumado 0.5
                            double numero = double.Parse(detalle.Clas1Res.TrimEnd('+'));
                            puestos.Add(new Tuple<int, int>(detalle.IdDep.Value, puesto));
                        }
                        if (detalle.Clas1Res.All(char.IsDigit))
                        {
                            

                            puestos.Add(new Tuple<int, int>(detalle.IdDep.Value, puesto));
                        }
                        puesto += empate;
                        resultadoAnterior = detalle.Clas1Res;
                    }
                }

                if (tipo == 2)
                {
                    var detallesOrdenados = detalles.OrderByDescending(d => d.Clas2Res).ToList();


                    int puesto = 1;
                    int empate = 1;

                    string resultadoAnterior = detallesOrdenados[0].Clas2Res;

                    foreach (var detalle in detallesOrdenados)
                    {
                        if (detalle.Clas2Res == "top")
                        {
                            // Si es "top", asignar el puesto y continuar
                            puestos.Add(new Tuple<int, int>(detalle.IdDep.Value, puesto));
                        }
                        if (detalle.Clas2Res.EndsWith("+"))
                        {
                            // Si termina con "+", es un número sumado 0.5
                            double numero = double.Parse(detalle.Clas2Res.TrimEnd('+'));
                            puestos.Add(new Tuple<int, int>(detalle.IdDep.Value, puesto));
                        }
                        if (detalle.Clas2Res.All(char.IsDigit))
                        {


                            puestos.Add(new Tuple<int, int>(detalle.IdDep.Value, puesto));
                        }
                        puesto += empate;
                        resultadoAnterior = detalle.Clas2Res;
                    }
                }
            }

            return puestos;
        }

        // GET: HomeController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
