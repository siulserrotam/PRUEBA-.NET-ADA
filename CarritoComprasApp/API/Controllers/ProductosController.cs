using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infraestructure.Data;
using Domain.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/productos")]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProductosDisponibles()
        {
            var productos = await _context.Productos.Where(p => p.CantidadDisponible > 0).ToListAsync();
            return Ok(productos);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProducto(int id, [FromBody] Producto model)
        {
            var rol = Request.Headers["x-rol"].FirstOrDefault();
            if (rol != "Administrador") return Unauthorized("Acceso restringido a administradores.");

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound("Producto no encontrado.");

            producto.CantidadDisponible = model.CantidadDisponible;
            await _context.SaveChangesAsync();

            return Ok(producto);
        }
    }
}
