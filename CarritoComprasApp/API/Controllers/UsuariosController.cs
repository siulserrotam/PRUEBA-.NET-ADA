using Microsoft.AspNetCore.Mvc;
using Infraestructure.Data;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompradores()
        {
            var usuarios = await _context.Usuarios
            .Where(u => u.Rol == "Comprador")
            .ToListAsync();
            return Ok(usuarios);
        }
        
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
