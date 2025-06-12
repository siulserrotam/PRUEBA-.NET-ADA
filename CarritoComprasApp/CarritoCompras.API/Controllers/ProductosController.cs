using Microsoft.AspNetCore.Mvc;
using CarritoCompras.Web.Data;
using CarritoCompras.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace CarritoCompras.Web.API.Controllers
{
    [Route("api/productos")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /api/productos
        [HttpGet]
        public async Task<IActionResult> GetDisponibles()
        {
            var productos = await _context.ObtenerProductosDisponiblesAsync();
            return Ok(productos);
        }

        // GET: /api/productos/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound();

            return Ok(producto);
        }

        // PUT: /api/productos/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ActualizarCantidad(int id, [FromBody] int nuevaCantidad)
        {
            var usuario = User.Identity?.Name ?? "Desconocido";
            await _context.ActualizarProductoAsync(id, nuevaCantidad, usuario);
            return NoContent();
        }
    }
}
