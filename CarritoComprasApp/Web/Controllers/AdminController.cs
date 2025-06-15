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
            var productos = await _productoService.ObtenerProductosDisponiblesAsync();
            var producto = productos.FirstOrDefault(p => p.Id == id);

            if (producto == null)
                return NotFound();

            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarProducto(Producto model)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Administrador")
            {
                TempData["Error"] = "No tiene permisos para realizar esta acción.";
                return RedirectToAction("Productos");
            }

            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7249");

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/productos/{model.Id}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Producto actualizado correctamente.";
            }
            else
            {
                TempData["Error"] = "Error al actualizar el producto.";
            }

            return RedirectToAction("Productos");
        }
    }
}
