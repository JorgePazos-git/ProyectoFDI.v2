using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProyectoFDI.v2.Extension;
using ProyectoFDI.v2.Models;
using System.Security.Claims;

namespace ProyectoFDI.v2.Controllers
{
    public class AccesoController : BaseController
    {
        public ProyectoFdiV2Context _context;

        public AccesoController(ProyectoFdiV2Context master)
        {
            this._context = master;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Salir()
        {
            BasicNotification("Sesion Cerrada", NotificationType.Success);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Acceso");
        }

        public Usuario ValidarUsuario(string nombre, string clave)
        {
            return _context.Usuarios.Where(s => s.NombreUsu == nombre && s.ClaveUsu == clave).FirstOrDefault();

        }
        [HttpPost]
        public async Task<IActionResult> Index(Usuario user)
        {
            Usuario usuario = ValidarUsuario(user.NombreUsu, user.ClaveUsu);

            if (usuario != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.NombreUsu)
                };

                string[] Roles = usuario.RolesUsu.Split(',');

                foreach (string rol in Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, rol));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                BasicNotification(usuario.RolesUsu, NotificationType.Info, "Rol");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                BasicNotification("Ingrese un usuario y contraseña validos", NotificationType.Error, "ERROR");
                return View();
            }
        }
    }
}
