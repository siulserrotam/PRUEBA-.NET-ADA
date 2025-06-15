using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Security.Claims;
using CarritoCompras.Api.Models;

namespace CarritoCompras.Web.Controllers
{
    public class CompradorController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CompradorController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Mostrar productos disponibles
        public async Task<IActionResult> Productos()
        {
            var client = _httpClientFactory.CreateClient("Api");
            var response = await client.GetAsync("/api/productos/disponibles");

            if (!response.IsSuccessStatusCode)
                return View("Error");

            var content = await response.Content.ReadAsStringAsync();
            var productos = JsonSerializer.Deserialize<List<Producto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View("Productos", productos);
        }

        // Vista previa de compra con validación de stock
        [HttpGet]
        public async Task<IActionResult> Comprar(int id, int cantidad)
        {
            if (id <= 0 || cantidad <= 0)
            {
                ViewBag.Error = "ID de producto o cantidad inválida.";
                return View("Error");
            }

            var client = _httpClientFactory.CreateClient("Api");
            var response = await client.GetAsync($"/api/productos/{id}");

            if (!response.IsSuccessStatusCode)
                return View("Error");

            var content = await response.Content.ReadAsStringAsync();
            var producto = JsonSerializer.Deserialize<Producto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            ViewBag.CantidadSolicitada = cantidad;

            if (cantidad > producto.Cantidad)
            {
                ViewBag.Mensaje = $"Solo hay {producto.Cantidad} unidades disponibles. ¿Desea comprar esa cantidad?";
                ViewBag.ConfirmarConStockDisponible = true;
                ViewBag.CantidadDisponible = producto.Cantidad;
            }

            return View("Comprar", producto);
        }

        // Confirmación de compra y creación de transacción
        [HttpPost]
        public async Task<IActionResult> ComprarConfirmado(int id, int cantidad)
        {
            if (id <= 0 || cantidad <= 0)
            {
                ViewBag.Error = "Datos inválidos para la compra.";
                return View("Error");
            }

            var client = _httpClientFactory.CreateClient("Api");

            var usuarioLogin = User.FindFirst("UsuarioLogin")?.Value ?? "comprador@demo.com";

            var data = new
            {
                ProductoId = id,
                Cantidad = cantidad,
                UsuarioLogin = usuarioLogin
            };

            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/transacciones", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensaje"] = "Compra realizada correctamente.";
                return RedirectToAction("Productos");
            }

            // Mostrar mensaje de error detallado
            var errorContent = await response.Content.ReadAsStringAsync();
            ViewBag.Error = $"Error al procesar la compra. Código: {response.StatusCode}. Detalle: {errorContent}";
            return View("Error");
        }
    }
}
