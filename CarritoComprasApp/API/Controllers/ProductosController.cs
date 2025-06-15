using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Infraestructure.Data;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/productos")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("disponibles")]
        public async Task<IActionResult> GetDisponibles()
        {
            var productos = await _context.Productos
                .Where(p => p.CantidadDisponible > 0)
                .ToListAsync();
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

        // Método 1: Actualización completa
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
            producto.CantidadDisponible = productoActualizado.CantidadDisponible;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Producto actualizado correctamente.",
                producto.Id,
                producto.Nombre,
                producto.CantidadDisponible,
                producto.Descripcion
            });
        }

        // Método 2: Solo actualizar el stock (con ruta diferente)
        [HttpPut("actualizar-stock/{id}")]
        public async Task<IActionResult> ActualizarProducto(int id, [FromBody] Producto model)
        {
            if (model == null || id != model.Id)
                return BadRequest();

            var rol = "Administrador"; // Simulado. Cambiar por lógica real si se necesita

            if (rol != "Administrador")
                return Unauthorized("Solo los administradores pueden actualizar productos");

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound();

            producto.CantidadDisponible = model.CantidadDisponible;

            await _context.SaveChangesAsync();
            return Ok(producto);
        }
    }
}
