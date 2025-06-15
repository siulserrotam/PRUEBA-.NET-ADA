using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarritoCompras.Api.Models;

namespace CarritoCompras.API.Controllers
{
    [ApiController]
    [Route("api/transacciones")] 
    public class TransaccionesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransaccionesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostTransaccion([FromBody] CrearTransaccionDto dto)
        {
            try
            {
                // Validación básica
                if (dto == null)
                    return BadRequest("La transacción no puede ser nula.");

                if (string.IsNullOrWhiteSpace(dto.UsuarioLogin))
                    return BadRequest("El campo UsuarioLogin es obligatorio.");

                if (dto.Cantidad <= 0)
                    return BadRequest("La cantidad debe ser mayor que cero.");

                if (dto.ProductoId <= 0)
                    return BadRequest("El ID del producto debe ser válido.");

                // Buscar usuario
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.UsuarioLogin == dto.UsuarioLogin);

                if (usuario == null)
                    return NotFound($"No se encontró el usuario con login '{dto.UsuarioLogin}'.");

                // Buscar producto
                var producto = await _context.Productos
                    .FirstOrDefaultAsync(p => p.Id == dto.ProductoId);

                if (producto == null)
                    return NotFound($"No se encontró el producto con ID {dto.ProductoId}.");

                // Validar stock
                if (dto.Cantidad > producto.Cantidad)
                    return BadRequest($"Stock insuficiente. Solo hay {producto.Cantidad} unidades disponibles.");

                // Crear transacción
                var transaccion = new Transaccion
                {
                    UsuarioId = usuario.Id,
                    ProductoId = producto.Id,
                    Cantidad = dto.Cantidad,
                    Fecha = DateTime.Now
                };

                _context.Transacciones.Add(transaccion);

                // Actualizar stock
                producto.Cantidad -= dto.Cantidad;
                _context.Productos.Update(producto);

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    mensaje = "Transacción registrada correctamente.",
                    transaccion
                });
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, $"Error al guardar en la base de datos: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno al crear la transacción: {ex.Message}");
            }
        }
    }
}
