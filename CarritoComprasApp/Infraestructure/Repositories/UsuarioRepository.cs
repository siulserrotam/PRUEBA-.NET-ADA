using Domain.Models;
using Infraestructure.Data;
using Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> ObtenerPorLoginAsync(string usuarioLogin)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.UsuarioLogin == usuarioLogin);
        }

        public async Task<Usuario?> ObtenerPorIdentificacionAsync(string identificacion)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Identificacion == identificacion);
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<List<Usuario>> ObtenerUsuariosClientesAsync()
        {
            return await _context.Usuarios
                .Where(u => u.Rol == "Cliente")
                .ToListAsync();
        }

        public async Task CrearUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Usuario>> ObtenerUsuariosCompradoresSP()
        {
            return await _context.Usuarios
                .Where(u => u.Rol == "Comprador")
                .ToListAsync();
        }
    }
}
