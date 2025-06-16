using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Domain.Models;
using Application.Interfaces;

namespace Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly ITransaccionService _transaccionService;
        private readonly IUsuarioService _usuarioService;
        private readonly IProductoService _productoService;

        public AdminController(ITransaccionService transaccionService, IUsuarioService usuarioService, IProductoService productoService)
        {
            _transaccionService = transaccionService;
            _usuarioService = usuarioService;
            _productoService = productoService;
        }

        public async Task<IActionResult> Transacciones()
        {
            var transacciones = await _transaccionService.ObtenerHistorialTransaccionesAsync();
            return View(transacciones);
        }

        public async Task<IActionResult> Usuarios()
        {
            var usuarios = await _usuarioService.ObtenerUsuariosCompradoresAsync();
            return View(usuarios);
        }

        public async Task<IActionResult> Productos()
        {
            var productos = await _productoService.ObtenerProductosDisponiblesAsync();
            return View(productos);
        }

        public async Task<IActionResult> ActualizarProducto(int id)
        {
            var producto = await _productoService.ObtenerProductoPorIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarProducto(Producto model)
        {
            // Validar que el rol del usuario sea Administrador
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Administrador")
            {
                TempData["Error"] = "No tiene permisos para realizar esta acción.";
                return RedirectToAction("Productos");
            }

            // Validar que la cantidad disponible sea un valor positivo
            if (model.CantidadDisponible <= 0)
            {
                TempData["Error"] = "La cantidad disponible debe ser mayor a 0.";
                return RedirectToAction("Productos");
            }

            // Actualizar el producto en la base de datos
            var productoExistente = await _productoService.ObtenerProductoPorIdAsync(model.Id);
            if (productoExistente == null)
            {
                return NotFound();
            }

            productoExistente.CantidadDisponible = model.CantidadDisponible;
            await _productoService.ActualizarProductoAsync(productoExistente);

            TempData["Success"] = "Producto actualizado correctamente.";
            return RedirectToAction("Productos");
        }
    }
}
