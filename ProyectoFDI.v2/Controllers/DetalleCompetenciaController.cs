using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFDI.v2.Code;
using ProyectoFDI.v2.Models;
using System.Data;
using System.Runtime.Intrinsics.Arm;

namespace ProyectoFDI.v2.Controllers
{
    public class DetalleCompetenciaController : Controller
    {
        private string apiUrl;
        private readonly IConverter _converter;

        public DetalleCompetenciaController(IConfiguration configuration, IConverter converter)
        {
            apiUrl = configuration["urlBase"].ToString() + "DetalleCompetencia/";
            ViewBag.ReturnTo = "index";
            _converter = converter;
        }

        [Authorize(Roles = "Administrador,Juez")]
        // GET: DetalleCompetenciaController
        public ActionResult Index()
        {
            return View(APIConsumer<DetalleCompetencium>.Select(apiUrl));
        }

        private List<SelectListItem> listaDeportistas()
        {
            var deportistas = APIConsumer<Deportistum>.Select(apiUrl.Replace("DetalleCompetencia", "Deportista"));
            var lista = deportistas.Select(f => new SelectListItem
            {
                Value = f.IdDep.ToString(),
                Text = f.NombresDep + " " + f.ApellidosDep
            }).ToList();

            return lista;
        }

        private List<SelectListItem> listaCompetencias()
        {
            var deportistas = APIConsumer<Competencium>.Select(apiUrl.Replace("DetalleCompetencia", "Competencia"));
            var lista = deportistas.Select(f => new SelectListItem
            {
                Value = f.IdCom.ToString(),
                Text = f.NombreCom
            }).ToList();

            return lista;
        }

        [Authorize(Roles = "Administrador,Juez")]
        // GET: DetalleCompetenciaController/Details/5
        public ActionResult Details(int id)
        {
            var data = APIConsumer<DetalleCompetencium>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador,Juez")]
        // GET: DetalleCompetenciaController/Create
        public ActionResult Create(string? returnTo)
        {
            ViewBag.ReturnTo = returnTo;
            ViewBag.listaDeportistas = listaDeportistas();
            ViewBag.listaCompetencias = listaCompetencias();
            return View();
        }

        [Authorize(Roles = "Administrador,Juez")]
        // POST: DetalleCompetenciaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DetalleCompetencium detalle, string returnTo)
        {
            try
            {
                APIConsumer<DetalleCompetencium>.Insert(apiUrl, detalle);
                if (returnTo == null)
                {
                    return RedirectToAction(nameof(Index));
                    //return RedirectToAction("Edit", "Competencia", new { id = detalle.IdCom });
                }
                else
                {
                    //return RedirectToAction(nameof(Create));
                    return RedirectToAction("Edit", "Competencia", new { id = detalle.IdCom });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(detalle);
            }
        }

        [Authorize(Roles = "Administrador,Juez")]
        // GET: DetalleCompetenciaController/Edit/5
        public ActionResult Edit(int id, string? returnTo)
        {
            ViewBag.listaDeportistas = listaDeportistas();
            ViewBag.listaCompetencias = listaCompetencias();
            ViewBag.ReturnTo = returnTo;
            var data = APIConsumer<DetalleCompetencium>.SelectOne(apiUrl + id.ToString());
            return View(data);
        }

        [Authorize(Roles = "Administrador,Juez")]
        // GET: DetalleCompetenciaController/Edit/5
        public ActionResult Resultados(int id, string? returnTo)
        {
            ViewBag.listaDeportistas = listaDeportistas();
            ViewBag.listaCompetencias = listaCompetencias();
            ViewBag.ReturnTo = returnTo;
            var data = APIConsumer<DetalleCompetencium>.SelectOne(apiUrl + id.ToString());
            ViewBag.data = data;
            return View(data);
        }

        [Authorize(Roles = "Administrador,Juez")]
        // POST: DetalleCompetenciaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, DetalleCompetencium detalle, string returnTo)
        {
            try
            {
                APIConsumer<DetalleCompetencium>.Update(apiUrl + id.ToString(), detalle);
                if (returnTo == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {   
                    return RedirectToAction("Edit", "Competencia", new { id = detalle.IdCom });           
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(detalle);
            }
        }

        [Authorize(Roles = "Administrador,Juez")]
        // POST: DetalleCompetenciaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Resultados(int id, DetalleCompetencium detalle, string returnTo)
        {
            try
            {
                APIConsumer<DetalleCompetencium>.Update(apiUrl + id.ToString(), detalle);
                if (returnTo == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Resultados", "Competencia");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(detalle);
            }
        }

        [Authorize(Roles = "Administrador,Juez")]
        // GET: DetalleCompetenciaController/Delete/5
        public ActionResult Delete(int id, string? returnTo)
        {
            var data = APIConsumer<DetalleCompetencium>.SelectOne(apiUrl + id.ToString());
            ViewBag.ReturnTo = returnTo;
            return View(data);
        }

        [Authorize(Roles = "Administrador,Juez")]
        // POST: DetalleCompetenciaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, DetalleCompetencium detalle, string returnTo)
        {
            try
            {
                APIConsumer<DetalleCompetencium>.Delete(apiUrl + id.ToString());

                return RedirectToAction("Resultados", "Competencia");
                //if (returnTo == null)
                //{
                //    return RedirectToAction(nameof(Index));
                //}
                //else
                //{
                //    return RedirectToAction("Edit", "Competencia", new { id = detalle.IdCom });
                //}
            }
            catch
            {
                return View(detalle);
            }
        }

        public IActionResult VistaPDFListaResultados(int competencia)
        {
            ViewBag.competencium = APIConsumer<VistaCompetencium>.SelectOne(apiUrl.Replace("DetalleCompetencia", "VistaCompetenciums") + competencia);
            ViewBag.detalleCompetencium = APIConsumer<VistaVeloClasificacion>.Select(apiUrl.Replace("DetalleCompetencia", "VistaVeloClasificacions") + competencia)
                    .OrderBy(p => p.Puesto == null ? int.MaxValue : p.Puesto)
                    .ThenBy(p => p.ResultadoClasificacion);

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

            url_pagina = $"{url_pagina}/DetalleCompetencia/VistaPDFListaResultados?competencia={competencia}";




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

        public IActionResult VistaPDFListaResultadosFinal(int competencia)
        {
         
            ViewBag.competencium = APIConsumer<VistaCompetencium>.SelectOne(apiUrl.Replace("DetalleCompetencia", "VistaCompetenciums") + competencia);
            var lista = APIConsumer<VistaVeloFinal>.Select(apiUrl.Replace("DetalleCompetencia", "VistaVeloFinals") + competencia);

            // Definir un valor especial para los resultados nulos
            const double valorNulo = double.MaxValue;

            // Ordenar la lista
            var listaOrdenada = lista.OrderBy(d =>
            {
                // Manejar los casos especiales "fall" y "fs" asignándoles valores que los coloquen al final
                if (d.ResultadoFinal == "fall")
                    return double.MaxValue - 2; // Usar el valor máximo de double para ponerlo al final
                else if (d.ResultadoFinal == "fs")
                    return double.MaxValue - 1; // Usar un valor cercano al máximo para "fs"

                // Si el resultado no es "fall" ni "fs", convertirlo a double y devolverlo
                if (string.IsNullOrEmpty(d.ResultadoFinal))
                    return valorNulo; // Asignar valor especial para los resultados nulos
                else
                    return Convert.ToDouble(d.ResultadoFinal); // Convertir el resultado final a double
            })
            .ThenBy(d =>
            {
                // Manejar los casos especiales "fall" y "fs" asignándoles valores que los coloquen al final
                if (d.ResultadoSemifinal == "fall")
                    return double.MaxValue - 2; // Usar el valor máximo de double para ponerlo al final
                else if (d.ResultadoSemifinal == "fs")
                    return double.MaxValue - 1; // Usar un valor cercano al máximo para "fs"

                // Si el resultado no es "fall" ni "fs", convertirlo a double y devolverlo
                if (string.IsNullOrEmpty(d.ResultadoSemifinal))
                    return valorNulo; // Asignar valor especial para los resultados nulos
                else
                    return Convert.ToDouble(d.ResultadoSemifinal); // Convertir el resultado final a double
            })
            .ThenBy(d =>
            {
                // Manejar los casos especiales "fall" y "fs" asignándoles valores que los coloquen al final
                if (d.ResultadoCuartos == "fall")
                    return double.MaxValue - 2; // Usar el valor máximo de double para ponerlo al final
                else if (d.ResultadoCuartos == "fs")
                    return double.MaxValue - 1; // Usar un valor cercano al máximo para "fs"

                // Si el resultado no es "fall" ni "fs", convertirlo a double y devolverlo
                if (string.IsNullOrEmpty(d.ResultadoCuartos))
                    return valorNulo; // Asignar valor especial para los resultados nulos
                else
                    return Convert.ToDouble(d.ResultadoCuartos); // Convertir el resultado final a double
            })
            .ThenBy(d =>
            {
                // Manejar los casos especiales "fall" y "fs" asignándoles valores que los coloquen al final
                if (d.ResultadoOctavos == "fall")
                    return double.MaxValue - 2; // Usar el valor máximo de double para ponerlo al final
                else if (d.ResultadoOctavos == "fs")
                    return double.MaxValue - 1; // Usar un valor cercano al máximo para "fs"

                // Si el resultado no es "fall" ni "fs", convertirlo a double y devolverlo
                if (string.IsNullOrEmpty(d.ResultadoOctavos))
                    return valorNulo; // Asignar valor especial para los resultados nulos
                else
                    return Convert.ToDouble(d.ResultadoOctavos); // Convertir el resultado final a double
            })
            .ThenBy(d =>
            {
                // Manejar los casos especiales "fall" y "fs" asignándoles valores que los coloquen al final
                if (d.ResultadoClasificacion == "fall")
                    return double.MaxValue - 2; // Usar el valor máximo de double para ponerlo al final
                else if (d.ResultadoClasificacion == "fs")
                    return double.MaxValue - 1; // Usar un valor cercano al máximo para "fs"

                // Si el resultado no es "fall" ni "fs", convertirlo a double y devolverlo
                if (string.IsNullOrEmpty(d.ResultadoClasificacion))
                    return valorNulo; // Asignar valor especial para los resultados nulos
                else
                    return Convert.ToDouble(d.ResultadoClasificacion); // Convertir el resultado final a double
            })
            .ToList();

            // Asignar la lista ordenada de nuevo a ViewBag.detalleCompetencium
            ViewBag.detalleCompetencium = listaOrdenada;

            return View();
        }


        public IActionResult MostrarPDFNuevaPaginaFinal(int competencia)
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

            url_pagina = $"{url_pagina}/DetalleCompetencia/VistaPDFListaResultadosFinal?competencia={competencia}";




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
