using Microsoft.AspNetCore.Mvc;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    public class CompraController : Controller
    {
        private readonly AppDbContext _context;

        public CompraController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult ConfirmarCompra(int productoId, int cantidad)
        {
            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioIdStr) || !int.TryParse(usuarioIdStr, out int usuarioId))
            {
                TempData["Error"] = "Usuario no autenticado.";
                return RedirectToAction("Login", "Login");
            }

            try
            {
                var query = "EXEC usp_InsertarTransaccion @UsuarioId = {0}, @ProductoId = {1}, @Cantidad = {2}";
                _context.Database.ExecuteSqlRaw(query, usuarioId, productoId, cantidad);

                TempData["Success"] = "Compra realizada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al realizar la compra: " + ex.Message;
            }

            return RedirectToAction("Productos", "Cliente");
        }
    }
}
