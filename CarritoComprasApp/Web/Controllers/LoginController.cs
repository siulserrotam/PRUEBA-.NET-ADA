using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

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

            if (usuario != null)
            {
                HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
                HttpContext.Session.SetString("Rol", usuario.Rol ?? "");
                HttpContext.Session.SetString("Nombre", usuario.Nombre ?? "");

                if (usuario.Rol == "Administrador")
                    return RedirectToAction("Index", "Admin");
                else
                    return RedirectToAction("Productos", "Cliente");
            }

            ViewBag.Mensaje = "Usuario o contraseña incorrectos";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
