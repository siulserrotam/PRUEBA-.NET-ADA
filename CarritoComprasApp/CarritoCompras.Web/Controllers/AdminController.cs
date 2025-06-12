using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CarritoCompras.Web.Models;
using System.Net.Http.Headers;
using System.Collections.Generic;

public class AdminController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AdminController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // GET: /admin
    public IActionResult Index()
    {
        return View();
    }

    // GET: /admin/productos
    public async Task<IActionResult> ProductosDisponibles()
    {
        var client = _httpClientFactory.CreateClient("Api");
        var response = await client.GetAsync("/api/productos");

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "No se pudo obtener la lista de productos.";
            return View("Error");
        }

        var content = await response.Content.ReadAsStringAsync();
        var productos = JsonSerializer.Deserialize<List<Producto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return View(productos);
    }

    // GET: /admin/usuarios
    public async Task<IActionResult> UsuariosCompradores()
    {
        var client = _httpClientFactory.CreateClient("Api");
        var response = await client.GetAsync("/api/usuarios");

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "No se pudo obtener la lista de usuarios compradores.";
            return View("Error");
        }

        var content = await response.Content.ReadAsStringAsync();
        var usuarios = JsonSerializer.Deserialize<List<Usuario>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return View(usuarios);
    }

    // GET: /admin/historial
    public async Task<IActionResult> HistorialTransacciones()
    {
        var client = _httpClientFactory.CreateClient("Api");
        var response = await client.GetAsync("/api/transacciones");

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "No se pudo obtener el historial de transacciones.";
            return View("Error");
        }

        var content = await response.Content.ReadAsStringAsync();
        var transacciones = JsonSerializer.Deserialize<List<Transaccion>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return View(transacciones);
    }

    // POST: /admin/actualizar-producto
    [HttpPost]
    public async Task<IActionResult> ActualizarProducto(int id, int cantidad)
    {
        if (cantidad < 0)
        {
            ViewBag.Error = "La cantidad debe ser mayor o igual a 0.";
            return View("Error");
        }

        var client = _httpClientFactory.CreateClient("Api");

        var usuario = User.Identity?.Name ?? "admin@admin.com"; // opcional: puedes mejorar con HttpContext.Session

        var data = new
        {
            Cantidad = cantidad,
            Usuario = usuario
        };

        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // Agregar encabezado personalizado para validación de rol
        client.DefaultRequestHeaders.Remove("x-rol");
        client.DefaultRequestHeaders.Add("x-rol", "Administrador");

        var response = await client.PutAsync($"/api/productos/{id}", content);

        if (response.IsSuccessStatusCode)
            return RedirectToAction("ProductosDisponibles");

        ViewBag.Error = "Error al actualizar el producto: " + await response.Content.ReadAsStringAsync();
        return View("Error");
    }
}
