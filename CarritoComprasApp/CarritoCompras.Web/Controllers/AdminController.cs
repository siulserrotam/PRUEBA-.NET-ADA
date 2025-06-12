using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CarritoCompras.Web.Models;
using System.Net.Http.Headers;

public class AdminController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AdminController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // GET: /admin/productos
    public async Task<IActionResult> ProductosDisponibles()
    {
        var client = _httpClientFactory.CreateClient("Api");
        var response = await client.GetAsync("/api/productos/disponibles");

        if (!response.IsSuccessStatusCode)
            return View("Error");

        var content = await response.Content.ReadAsStringAsync();
        var productos = JsonSerializer.Deserialize<List<Producto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return View(productos);
    }

    // GET: /admin/usuarios
    public async Task<IActionResult> UsuariosCompradores()
    {
        var client = _httpClientFactory.CreateClient("Api");
        var response = await client.GetAsync("/api/usuarios/compradores");

        if (!response.IsSuccessStatusCode)
            return View("Error");

        var content = await response.Content.ReadAsStringAsync();
        var usuarios = JsonSerializer.Deserialize<List<Usuario>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return View(usuarios);
    }

    // POST: /admin/actualizar-producto
    [HttpPost]
    public async Task<IActionResult> ActualizarProducto(int id, int cantidad)
    {
        var client = _httpClientFactory.CreateClient("Api");

        // Puedes obtener el usuario real desde el contexto si lo tienes en sesión
        var usuario = User.Identity.Name ?? "admin@admin.com";

        var data = new
        {
            Cantidad = cantidad,
            Usuario = usuario
        };

        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        client.DefaultRequestHeaders.Remove("x-rol"); // Por si existe de antes
        client.DefaultRequestHeaders.Add("x-rol", "Administrador");

        var response = await client.PutAsync($"/api/productos/{id}", content);

        if (response.IsSuccessStatusCode)
            return RedirectToAction("ProductosDisponibles");

        var error = await response.Content.ReadAsStringAsync();
        ViewBag.Error = error;
        return View("Error");
    }
}
