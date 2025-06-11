using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CarritoCompras.Web.Models;

public class AdminController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AdminController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> ProductosDisponibles()
    {
        var client = _httpClientFactory.CreateClient("Api");
        var response = await client.GetAsync("/api/productos/disponibles");
        var content = await response.Content.ReadAsStringAsync();
        var productos = JsonSerializer.Deserialize<List<Producto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return View(productos);
    }

    public async Task<IActionResult> UsuariosCompradores()
    {
        var client = _httpClientFactory.CreateClient("Api");
        var response = await client.GetAsync("/api/usuarios/compradores");
        var content = await response.Content.ReadAsStringAsync();
        var usuarios = JsonSerializer.Deserialize<List<Usuario>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return View(usuarios);
    }

    [HttpPost]
    public async Task<IActionResult> ActualizarProducto(int id, int cantidad)
    {
        var client = _httpClientFactory.CreateClient("Api");
        var data = new { Id = id, Cantidad = cantidad, Usuario = "admin@admin.com" }; // usar el usuario logueado idealmente

        var json = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

        var response = await client.PutAsync($"/api/productos/{id}", json);

        if (response.IsSuccessStatusCode)
            return RedirectToAction("ProductosDisponibles");
        else
            return BadRequest("Error actualizando el producto.");
    }
}
