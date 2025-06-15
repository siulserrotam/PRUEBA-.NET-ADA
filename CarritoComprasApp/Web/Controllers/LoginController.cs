using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(); // Retorna la vista Login/Index.cshtml
        }

        [HttpPost]
        public IActionResult Index(string usuarioLogin, string clave)
        {
            // Lógica de autenticación (puedes integrar IUsuarioService aquí)
            if (usuarioLogin == "admin" && clave == "123") // Solo como ejemplo
            {
                // Aquí puedes establecer sesión y redirigir
                return RedirectToAction("Productos", "Cliente"); // o Admin
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

