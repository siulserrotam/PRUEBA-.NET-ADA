using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CarritoCompras.Web.Models;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Diagnostics;

public class AdminController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AdminController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Administrador()
    {
        return View();
    }

   public async Task<IActionResult> Productos()
    {
        var client = _httpClientFactory.CreateClient("Api");
        var response = await client.GetAsync("/api/productos/disponibles");

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "No se pudo obtener la lista de productos.";
            return View("Error");
        }

        var content = await response.Content.ReadAsStringAsync();
        var productos = JsonSerializer.Deserialize<List<Producto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return View(productos);
    }

    [HttpGet]
    public async Task<IActionResult> Usuarios()
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
    
    [HttpGet]
    public async Task<IActionResult> HistorialTransacciones()
    {
        var client = _httpClientFactory.CreateClient("Api");
        var response = await client.GetAsync("/api/transacciones");

        if (!response.IsSuccessStatusCode)
        {
            return View("Error", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

        var content = await response.Content.ReadAsStringAsync();
        var transacciones = JsonSerializer.Deserialize<List<TransaccionDTO>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return View(transacciones);
    }

    [HttpPost]
    public async Task<IActionResult> ActualizarProducto(int id, int cantidad)
    {
        if (cantidad < 0)
        {
            ViewBag.Error = "La cantidad debe ser mayor o igual a 0.";
            return View("Error");
        }

        var client = _httpClientFactory.CreateClient("Api");

        // Obtener el producto actual
        var getResponse = await client.GetAsync($"/api/productos/{id}");
        if (!getResponse.IsSuccessStatusCode)
        {
            ViewBag.Error = "No se pudo obtener el producto para actualizar.";
            return View("Error");
        }

        var getContent = await getResponse.Content.ReadAsStringAsync();
        var producto = JsonSerializer.Deserialize<Producto>(getContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Actualizar la cantidad
        producto.Cantidad = cantidad;

        // Preparar y enviar el producto actualizado
        var putContent = new StringContent(JsonSerializer.Serialize(producto), Encoding.UTF8, "application/json");

        client.DefaultRequestHeaders.Remove("x-rol");
        client.DefaultRequestHeaders.Add("x-rol", "Administrador");

        var putResponse = await client.PutAsync($"/api/productos/{id}", putContent);

        if (putResponse.IsSuccessStatusCode)
            return RedirectToAction("Productos");

        ViewBag.Error = "Error al actualizar el producto: " + await putResponse.Content.ReadAsStringAsync();
        return View("Error");
    }

}
