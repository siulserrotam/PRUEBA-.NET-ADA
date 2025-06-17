using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Transacciones()
        {
            var transacciones = await _transaccionService.ObtenerHistorialTransaccionesAsync();
            return View("Transacciones", transacciones);
        }

        public async Task<IActionResult> Usuarios()
        {
            var usuarios = await _usuarioService.ObtenerUsuariosCompradoresAsync();
            return View("Usuarios", usuarios);
        }

        public async Task<IActionResult> Productos()
        {
            var productos = await _productoService.ObtenerProductosDisponiblesAsync();
            return View("Productos", productos);
        }

        public async Task<IActionResult> ActualizarProducto(int id)
        {
            var producto = await _productoService.ObtenerProductoPorIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }


            return View("ActualizarProducto", producto);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarProducto(Producto model)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Administrador")
            {
                TempData["Error"] = "No tiene permisos para realizar esta acción.";
                return RedirectToAction("Productos");
            }


            if (model.CantidadDisponible <= 0)
            {
                TempData["Error"] = "La cantidad disponible debe ser mayor a 0.";
                return RedirectToAction("Productos");
            }


            var productoExistente = await _productoService.ObtenerProductoPorIdAsync(model.Id);
            if (productoExistente == null)
            {
                return NotFound();
            }

            productoExistente.CantidadDisponible = model.CantidadDisponible;
            await _productoService.ActualizarProductoAsync(productoExistente);

            TempData["Success"] = "Producto actualizado correctamente.";
            return RedirectToAction("productos");

        }
    }
}
