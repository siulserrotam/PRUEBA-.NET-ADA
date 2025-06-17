using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly ITransaccionService _transaccionService;

        public ClienteController(
            IProductoService productoService,
            ITransaccionService transaccionService)
        {
            _productoService = productoService;
            _transaccionService = transaccionService;
        }

        // Muestra la lista de productos disponibles
        [HttpGet]
        public async Task<IActionResult> Productos()
        {
            var productos = await _productoService.ObtenerProductosDisponiblesAsync();
            return View(productos);
        }

        // Muestra la vista de confirmación de compra
        [HttpGet]
        public async Task<IActionResult> ConfirmarCompra(int id)
        {
            var producto = await _productoService.ObtenerProductoPorIdAsync(id);
            if (producto == null)
            {
                return NotFound("Producto no encontrado");
            }

            return View(producto);
        }

        // Procesa la compra
        [HttpPost]
        public async Task<IActionResult> ConfirmarCompra(int id, int cantidad)
        {
            var producto = await _productoService.ObtenerProductoPorIdAsync(id);
            if (producto == null)
            {
                TempData["Error"] = "Producto no disponible.";
                return RedirectToAction("Productos");
            }

            if (cantidad <= 0 || cantidad > producto.CantidadDisponible)
            {
                TempData["Error"] = "Cantidad inválida o excede el stock disponible.";
                return RedirectToAction("ConfirmarCompra", new { id });
            }

            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioIdStr) || !int.TryParse(usuarioIdStr, out int usuarioId))
            {
                TempData["Error"] = "Usuario no autenticado.";
                return RedirectToAction("Login", "Login");
            }

            try
            {
                await _transaccionService.RegistrarTransaccionAsync(usuarioId, id, cantidad);
                TempData["Success"] = "Compra realizada correctamente.";
            }
            catch
            {
                TempData["Error"] = "Error al procesar la compra.";
            }

            return RedirectToAction("Productos");
        }
    }
}
