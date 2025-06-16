using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;

namespace Web.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IProductoService _productoService;

        public ClienteController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        public async Task<IActionResult> Productos()
        {
            var productos = await _productoService.ObtenerProductosAsync();
            return View(productos);
        }
    }
}
