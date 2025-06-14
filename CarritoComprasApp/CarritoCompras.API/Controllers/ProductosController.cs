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

        [HttpGet]
        public async Task<IActionResult> GetDisponibles()
        {
            var productos = await _context.ObtenerProductosDisponiblesAsync();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound();

            return Ok(producto);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Producto productoActualizado)
        {
            if (!Request.Headers.TryGetValue("x-rol", out var rol) || rol != "Administrador")
                return Unauthorized("No tienes permiso para actualizar productos.");

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound("Producto no encontrado.");

            producto.Nombre = productoActualizado.Nombre;
            producto.Descripcion = productoActualizado.Descripcion;
            producto.Cantidad = productoActualizado.Cantidad;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Producto actualizado correctamente.",
                producto.Id,
                producto.Nombre,
                producto.Cantidad,
                producto.Descripcion
            });
        }
    }
}
