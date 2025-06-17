using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Models;

namespace Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public RegisterController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Usuario usuario)
        {
            var existente = await _usuarioService.ObtenerPorIdentificacionAsync(usuario.Identificacion);

            if (existente != null)
            {
                ViewBag.Mensaje = "Ya existe un usuario con esa identificación.";
                return View();
            }

            usuario.Rol = "Cliente"; 

            await _usuarioService.CrearUsuarioAsync(usuario);

            return RedirectToAction("Index", "Login");
        }
    }
}
