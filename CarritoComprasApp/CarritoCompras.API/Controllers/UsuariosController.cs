using Microsoft.AspNetCore.Mvc;
using CarritoCompras.Web.Data;
using CarritoCompras.Web.Models;

namespace CarritoCompras.Web.API.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /api/usuarios
        [HttpGet]
        public async Task<IActionResult> GetCompradores()
        {
            var usuarios = await _context.ObtenerUsuariosCompradoresAsync();
            return Ok(usuarios);
        }

        // GET: /api/usuarios/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null || usuario.Rol != "Comprador")
                return NotFound();

            return Ok(usuario);
        }
    }
}
