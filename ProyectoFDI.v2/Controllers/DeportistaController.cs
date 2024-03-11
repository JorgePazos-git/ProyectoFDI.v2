using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using ProyectoFDI.v2.Code;
using ProyectoFDI.v2.Models;
using System.Data;
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
            catch(Exception ex)
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
            catch(Exception ex)
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

        [HttpPost]
        public ActionResult VerificarCedula(string cedula)
        {
            bool exists = false;

            Deportistum deportista = APIConsumer<Deportistum>.Select(apiUrl)
                .Where(d => d.CedulaDep != null && d.CedulaDep.Equals(cedula)).FirstOrDefault();

            if (deportista != null)
            {
                exists = true;
            }
            
            return Json(new { exists = exists });
        }


        public bool VerificaIdentificacion(string identificacion)
        {
            bool estado = false;
            char[] valced = new char[13];
            int provincia;
            if (identificacion.Length >= 10)
            {
                valced = identificacion.Trim().ToCharArray();
                provincia = int.Parse((valced[0].ToString() + valced[1].ToString()));
                if (provincia > 0 && provincia < 31) //Permitir cedulas emitidas en Consulados
                {
                    if (int.Parse(valced[2].ToString()) < 6)
                        estado = VerificaCedula(valced);
                    else if (int.Parse(valced[2].ToString()) == 6)
                    {
                        if (valced.Length == 13)
                        {
                            //Se agrega la validación de excluir de la validación de RUC, las identificaciones cuyo tercer dígito sea 6 o 9.
                            estado = true;
                        }
                        else
                            //Permitir cedulas emitidas en Consulados
                            estado = VerificaCedula(valced);
                    }
                    //Se agregó la validación del tercer dígito 8.
                    else if (int.Parse(valced[2].ToString()) == 8)
                    {
                        if (valced.Length == 13)
                            estado = VerificaSectorPublico(valced);
                        else
                            estado = false;
                    }
                    else if (int.Parse(valced[2].ToString()) == 9)
                    {
                        //Se agrega la validación de excluir de la validación de RUC, las identificaciones cuyo tercer dígito sea 6 o 9.
                        estado = true;
                    }
                }
            }
            return estado;
        }

        private bool VerificaCedula(char[] validarCedula)
        {
            int aux = 0, par = 0, impar = 0, verifi;
            for (int i = 0; i < 9; i += 2)
            {
                aux = 2 * int.Parse(validarCedula[i].ToString());
                if (aux > 9)
                    aux -= 9;
                par += aux;
            }
            for (int i = 1; i < 9; i += 2)
            {
                impar += int.Parse(validarCedula[i].ToString());
            }

            aux = par + impar;
            if (aux % 10 != 0)
            {
                verifi = 10 - (aux % 10);
            }
            else
                verifi = 0;
            if (verifi == int.Parse(validarCedula[9].ToString()))
                return true;
            else
                return false;
        }

        private bool VerificaSectorPublico(char[] validarCedula)
        {
            int aux = 0, prod, veri;
            veri = int.Parse(validarCedula[9].ToString()) + int.Parse(validarCedula[10].ToString()) + int.Parse(validarCedula[11].ToString()) + int.Parse(validarCedula[12].ToString());
            if (veri > 0)
            {
                int[] coeficiente = new int[8] { 3, 2, 7, 6, 5, 4, 3, 2 };

                for (int i = 0; i < 8; i++)
                {
                    prod = int.Parse(validarCedula[i].ToString()) * coeficiente[i];
                    aux += prod;
                }

                if (aux % 11 == 0)
                {
                    veri = 0;
                }
                else if (aux % 11 == 1)
                {
                    return false;
                }
                else
                {
                    aux = aux % 11;
                    veri = 11 - aux;
                }

                if (veri == int.Parse(validarCedula[8].ToString()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }
}
