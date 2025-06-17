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
            // Validar rol de usuario
            var rol = Request.Headers["x-rol"].FirstOrDefault();
            if (rol != "Administrador")
            {
                return Unauthorized(new { mensaje = "Acceso restringido a administradores." });
            }

            // Buscar producto
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound(new { mensaje = "Producto no encontrado." });
            }

            // Validar cantidad disponible
            if (model.CantidadDisponible < 0)
            {
                return BadRequest(new { mensaje = "La cantidad disponible no puede ser negativa." });
            }

            // Actualizar producto
            try
            {
                producto.CantidadDisponible = model.CantidadDisponible;
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Producto actualizado correctamente.", producto });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error de concurrencia al actualizar el producto." });
            }
        }
    }
}