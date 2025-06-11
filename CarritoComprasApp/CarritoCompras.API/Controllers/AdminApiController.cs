using CarritoCompras.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CarritoCompras.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/adminapi/productos/disponibles
        [HttpGet("productos/disponibles")]
        public async Task<IActionResult> ObtenerProductosDisponibles()
        {
            var productos = await _context.ObtenerProductosDisponiblesAsync();
            return Ok(productos);
        }

        // GET: api/adminapi/usuarios/compradores
        [HttpGet("usuarios/compradores")]
        public async Task<IActionResult> ObtenerUsuariosCompradores()
        {
            var usuarios = await _context.ObtenerUsuariosCompradoresAsync();
            return Ok(usuarios);
        }

        // PUT: api/adminapi/productos/{id}
        [HttpPut("productos/{id}")]
        public async Task<IActionResult> ActualizarProducto(int id, [FromBody] ProductoDTO dto)
        {
            // Validar rol (aquí es ideal usar JWT, pero para simplificar validaremos una cabecera temporal)
            var rol = Request.Headers["x-rol"].ToString();
            if (rol != "Administrador")
                return Unauthorized("Acceso solo para administradores");

            await _context.ActualizarProductoAsync(id, dto.Cantidad, dto.Usuario);
            return Ok(new { mensaje = "Producto actualizado correctamente." });
        }
    }

    public class ProductoDTO
    {
        public int Cantidad { get; set; }
        public string Usuario { get; set; }
    }
}
