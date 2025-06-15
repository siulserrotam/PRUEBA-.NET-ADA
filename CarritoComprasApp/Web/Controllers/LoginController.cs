using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Models;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public LoginController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string usuarioLogin, string clave)
        {
            var usuario = await _usuarioService.LoginAsync(usuarioLogin, clave);

            if (usuario == null)
            {
                ViewBag.Mensaje = "Credenciales incorrectas.";
                return View();
            }

            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
            HttpContext.Session.SetString("Rol", usuario.Rol);

            if (usuario.Rol == "Administrador")
                return RedirectToAction("Index", "Admin");
            else
                return RedirectToAction("Productos", "Cliente");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
