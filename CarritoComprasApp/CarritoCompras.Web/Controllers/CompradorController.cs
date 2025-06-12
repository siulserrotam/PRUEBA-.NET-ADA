using Microsoft.AspNetCore.Mvc;
using CarritoCompras.Web.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Security.Claims;

namespace CarritoCompras.Web.Controllers
{
    public class CompradorController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CompradorController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /comprador
        public async Task<IActionResult> Comprador()
        {
            var client = _httpClientFactory.CreateClient("Api");
            var response = await client.GetAsync("/api/productos/disponibles");

            if (!response.IsSuccessStatusCode)
                return View("Error");

            var content = await response.Content.ReadAsStringAsync();
            var productos = JsonSerializer.Deserialize<List<Producto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(productos);
        }

        // GET: /comprar
        [HttpGet]
        public async Task<IActionResult> Comprar(int id, int cantidad)
        {
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
            }

            return View(producto);
        }

        // POST: /comprar
        [HttpPost]
        public async Task<IActionResult> ComprarConfirmado(int id, int cantidad)
        {
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
                return RedirectToAction("Comprador");
            }

            ViewBag.Error = await response.Content.ReadAsStringAsync();
            return View("Error");
        }
    }
}
