using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFDI.v2.Code;
using ProyectoFDI.v2.Models;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.JSInterop;
using System.Globalization;
using DinkToPdf;
using Microsoft.AspNetCore.Http.Extensions;
using DinkToPdf.Contracts;

namespace ProyectoFDI.v2.Controllers
{
    public class DificultadController : Controller
    {
        private string apiUrl;
        private int idCom;
        private readonly IConverter _converter;

        public DificultadController(IConfiguration configuration, IConverter converter)
        {
            apiUrl = configuration["urlBase"].ToString() + "DetalleCompetenciaDificultad/";
            _converter = converter;
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

        public ActionResult Finales(int competencia)
        {
            this.idCom = competencia;
            ViewBag.idcompetencia = competencia;
            GetCompetencia(idCom);
            List<DetalleCompetenciaDificultad> deportistasOrdenados = listaDetallesFinales();

            return View(deportistasOrdenados);
        }

        private List<DetalleCompetenciaDificultad> listaDetallesFinales()
        {
            var detalles = APIConsumer<DetalleCompetenciaDificultad>.Select(apiUrl);
            var lista = detalles.Select(f => new DetalleCompetenciaDificultad
            {
                IdDetalleDificultad = f.IdDetalleDificultad,
                IdCom = f.IdCom,
                IdDep = f.IdDep,
                Clas1Res = f.Clas1Res,
                Clas2Res = f.Clas2Res,
                FinalRes = f.FinalRes,
                Puesto = f.Puesto,
                PuestoInicialRes = f.PuestoInicialRes,
                IdComNavigation = f.IdComNavigation,
                IdDepNavigation = f.IdDepNavigation
            }).OrderBy(f => f.PuestoInicialRes).Take(8).ToList();

            return lista;
        }

        private List<DetalleCompetenciaDificultad> listaDetalles()
        {
            var detalles = APIConsumer<DetalleCompetenciaDificultad>.Select(apiUrl);
            var lista = detalles.Select(f => new DetalleCompetenciaDificultad
            {
                IdDetalleDificultad = f.IdDetalleDificultad,
                IdCom = f.IdCom,
                IdDep = f.IdDep,
                Clas1Res = f.Clas1Res,
                Clas2Res = f.Clas2Res,
                FinalRes = f.FinalRes,
                Puesto = f.Puesto,
                PuestoInicialRes = f.PuestoInicialRes,
                IdComNavigation = f.IdComNavigation,
                IdDepNavigation = f.IdDepNavigation
            }).OrderBy(f => f.Puesto.HasValue ? 0 : 1)
              .ThenBy(f => f.Puesto)
              .ThenBy(f => f.PuestoInicialRes)
              .ToList();

            return lista;
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
        public ActionResult AgregarFinal(int idDetalle, string deportistaNombre, string final)
        {
            try
            {
                DetalleCompetenciaDificultad detalleOld = APIConsumer<DetalleCompetenciaDificultad>.SelectOne(apiUrl + idDetalle.ToString());
                DetalleCompetenciaDificultad detallenew = detalleOld;

                detallenew.FinalRes = final;

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

        [HttpPost]
        public IActionResult AsignarPuestosFinal(int idCompetencia)
        {
            try
            {
                // Obtener detalles y calcular puestos de ambas clasificaciones
                var detalles = APIConsumer<DetalleCompetenciaDificultad>.Select(apiUrl).Where(f => f.IdCom == idCompetencia).ToList();
                List<Tuple<int, int>> puestosFinal= CalcularPuestosClasificacion(detalles, 3);

                
                // Asignar puestos a los detalles
                for (int i = 0; i < puestosFinal.Count; i++)
                {
                    int deportistaId = puestosFinal[i].Item1;
                    DetalleCompetenciaDificultad detalle = detalles.FirstOrDefault(d => d.IdDep == deportistaId);

                    if (detalle != null)
                    {
                        detalle.Puesto = puestosFinal[i].Item2; // +1 porque los puestos comienzan en 1
                        APIConsumer<DetalleCompetenciaDificultad>.Update(apiUrl + detalle.IdDetalleDificultad.ToString(), detalle);
                    }
                }

                // Calcular y asignar los puestos iniciales para los participantes que no pasan a la final
                var puestosIniciales = detalles.Where(d => !puestosFinal.Any(pf => pf.Item1 == d.IdDep)).OrderBy(d => d.PuestoInicialRes).ToList();
                int puestoInicial = puestosFinal.Count + 1; // El siguiente puesto después de los puestos finales

                foreach (var detalle in puestosIniciales)
                {
                    detalle.Puesto = puestoInicial;
                    APIConsumer<DetalleCompetenciaDificultad>.Update(apiUrl + detalle.IdDetalleDificultad.ToString(), detalle);
                    puestoInicial++;
                }

                var deportistasOrdenados = detalles
                    .Where(d => d.FinalRes != null) // Filtrar los detalles donde IdComNavigation no sea null
                    .OrderBy(d => d.Puesto)
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
                    }).Take(1).ToList();

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
                if(tipo == 3)
                {
                    detalles = detalles.Where(d => d.FinalRes != null).ToList();
                    var detallesOrdenados = detalles.OrderByDescending(d => d.FinalRes).ToList();

                    int puesto = 1;
                    int empate = 1;

                    string resultadoAnterior = detallesOrdenados[0].FinalRes;

                    foreach (var detalle in detallesOrdenados)
                    {
                        if (detalle.FinalRes == "top")
                        {
                            // Si es "top", asignar el puesto y continuar
                            puestos.Add(new Tuple<int, int>(detalle.IdDep.Value, puesto));
                        }
                        if (detalle.FinalRes.EndsWith("+"))
                        {
                            // Si termina con "+", es un número sumado 0.5
                            double numero = double.Parse(detalle.FinalRes.TrimEnd('+'));
                            puestos.Add(new Tuple<int, int>(detalle.IdDep.Value, puesto));
                        }
                        if (detalle.FinalRes.All(char.IsDigit))
                        {


                            puestos.Add(new Tuple<int, int>(detalle.IdDep.Value, puesto));
                        }
                        puesto += empate;
                        resultadoAnterior = detalle.FinalRes;
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

        public FileResult ExportarExcel(int id)
        {
            List<DetalleCompetenciaDificultad> lista = listaDetalles(); 

            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection("Data Source=MSI;Initial Catalog=ProyectoFDI.v2;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("select dt.puesto as PUESTO_FINAL, dt.puesto_inicial_res as PUESTO_CLASIFICACION," +
                    "CONCAT(dp.nombres_dep,' ',dp.apellidos_dep) as DEPORTISTA," +
                    "dt.clas1_res as CLASIFICACION_1, dt.clas2_res as CLASIFICACION_2, dt.final_res as FINAL " +
                    "from detalle_competencia_dificultad dt INNER JOIN deportista dp on dt.id_dep=dp.id_dep " +
                    "where id_com = " + id + "GROUP BY dt.puesto, dt.puesto_inicial_res, dp.nombres_dep, dp.apellidos_dep," +
                    "dt.clas1_res, dt.clas2_res, dt.final_res ORDER BY dt.puesto");


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

        public IActionResult VistaPDFListaResultados(int competencia)
        {           
          ViewBag.competencium = APIConsumer<VistaCompetencium>.SelectOne(apiUrl.Replace("DetalleCompetenciaDificultad", "VistaCompetenciums") + competencia);
          ViewBag.detalleCompetencium = APIConsumer<VistaViasResultado>.Select(apiUrl.Replace("DetalleCompetenciaDificultad", "VistaViasResultadoes") + competencia);
            
            var detalle = ViewBag.detalleCompetencium;
            return View();
        }


        public IActionResult MostrarPDFNuevaPagina(int competencia)
        {

            string pagina_actual = HttpContext.Request.Path;
            string url_pagina = HttpContext.Request.GetEncodedUrl();
            url_pagina = url_pagina.Replace(pagina_actual, "");

            // Eliminar el primer parámetro competencia=11 de la URL
            int index = url_pagina.IndexOf("?competencia=");
            if (index != -1)
            {
                url_pagina = url_pagina.Remove(index);
            }

            url_pagina = $"{url_pagina}/Dificultad/VistaPDFListaResultados?competencia={competencia}";




            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings()
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait
                },
                Objects = {
                    new ObjectSettings(){
                        Page = url_pagina
                    }
                }

            };

            var archivoPDF = _converter.Convert(pdf);


            return File(archivoPDF, "application/pdf");
        }
    }
}
