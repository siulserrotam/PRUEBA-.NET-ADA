using Microsoft.AspNetCore.Mvc;
using Infraestructure.Data;
using Domain.Models;

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
        public IActionResult RealizarCompra(int productoId, int cantidad)
        {
            var producto = _context.Productos.FirstOrDefault(p => p.Id == productoId);

            if (producto == null)
            {
                TempData["Error"] = "Producto no encontrado.";
                return RedirectToAction("Productos", "Cliente");
            }

            if (cantidad <= 0 || cantidad > producto.CantidadDisponible)
            {
                TempData["Error"] = "Cantidad inválida o supera el stock disponible.";
                return RedirectToAction("ConfirmarCompra", "Cliente", new { id = productoId });
            }

            // Simula un usuario autenticado (reemplaza esto por el usuario real desde sesión)
            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioIdStr) || !int.TryParse(usuarioIdStr, out int usuarioId))
            {
                TempData["Error"] = "Usuario no autenticado.";
                return RedirectToAction("Login", "Login");
            }

            // Crear transacción
            var transaccion = new Transaccion
            {
                UsuarioId = usuarioId,
                ProductoId = productoId,
                Cantidad = cantidad,
                Fecha = DateTime.Now
            };

            // Actualizar stock y guardar cambios
            producto.CantidadDisponible -= cantidad;
            _context.Transacciones.Add(transaccion);
            _context.SaveChanges();

            TempData["Success"] = "Compra realizada exitosamente.";
            return RedirectToAction("Productos", "Cliente");
        }
    }
}
