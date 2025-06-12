using Microsoft.AspNetCore.Mvc;
using CarritoCompras.Web.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Cryptography;
using System.Text;

namespace CarritoCompras.Web.Controllers
{
    public class AuthController : Controller
    {
        // Simulación temporal de usuarios en memoria
        private static List<Usuario> _usuarios = new List<Usuario>();

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string usuarioLogin, string clave)
        {
            var hash = CalcularHash(clave);
            var usuario = _usuarios.FirstOrDefault(u => u.UsuarioLogin == usuarioLogin && u.Clave == hash);

            if (usuario == null)
            {
                ViewBag.Error = "Credenciales inválidas";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nombres),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim("UsuarioLogin", usuario.UsuarioLogin)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (usuario.Rol == "Administrador")
                return RedirectToAction("Index", "Admin");
            else
                return RedirectToAction("Productos", "Comprador");
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(Usuario model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_usuarios.Any(u => u.UsuarioLogin == model.UsuarioLogin))
            {
                ViewBag.Error = "El usuario ya existe";
                return View(model);
            }

            model.Clave = CalcularHash(model.Clave);
            model.Rol = "Comprador";
            model.Id = _usuarios.Count + 1;

            _usuarios.Add(model);

            TempData["Mensaje"] = "Usuario registrado correctamente";
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // Utilidad: Calcular hash SHA256
        private string CalcularHash(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
