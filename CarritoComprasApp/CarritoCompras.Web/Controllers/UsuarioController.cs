using Microsoft.AspNetCore.Mvc;
using CarritoCompras.Web.Data;
using CarritoCompras.Web.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

namespace CarritoCompras.Web.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public IActionResult Login(string usuario, string clave)
        {
            string claveHash = ConvertirSha256(clave);
            var user = _context.Usuarios
                .FirstOrDefault(u => u.UsuarioLogin == usuario && u.Clave == claveHash);

            if (user == null)
            {
                return RedirectToAction("Registrar");
            }

            HttpContext.Session.SetInt32("UsuarioId", user.Id);
            HttpContext.Session.SetString("Rol", user.Rol);

            if (user.Rol == "Admin")
                return RedirectToAction("Transacciones", "Admin");
            else
                return RedirectToAction("ProductosDisponibles", "Compra");
        }

        // GET: Registro
        public IActionResult Registrar()
        {
            return View();
        }

        // POST: Registro
        [HttpPost]
        public IActionResult Registrar(Usuario nuevo)
        {
            nuevo.Clave = ConvertirSha256(nuevo.Clave);
            nuevo.Rol = "Comprador"; // Por defecto

            _context.Usuarios.Add(nuevo);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        private string ConvertirSha256(string texto)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(texto);
            var hash = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
