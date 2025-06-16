using Application.Interfaces;
using Domain.Models;
using Infraestructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
       public class UsuarioService : IUsuarioService
        {
            private readonly IUsuarioRepository _usuarioRepository;

            public UsuarioService(IUsuarioRepository usuarioRepository)
            {
                _usuarioRepository = usuarioRepository;
            }

            public async Task<Usuario?> LoginAsync(string usuarioLogin, string clave)
            {
                var usuario = await _usuarioRepository.ObtenerPorLoginAsync(usuarioLogin);
                if (usuario == null)
                    return null;

                // Verifica el hash de la contraseña
                bool claveValida = BCrypt.Net.BCrypt.Verify(clave, usuario.Clave);
                return claveValida ? usuario : null;
            }

            public async Task<Usuario?> ObtenerPorIdentificacionAsync(string identificacion)
            {
                if (string.IsNullOrWhiteSpace(identificacion))
                    throw new ArgumentException("La identificación es obligatoria.");

                return await _usuarioRepository.ObtenerPorIdentificacionAsync(identificacion);
            }

            public async Task<Usuario?> ObtenerPorIdAsync(int id)
            {
                if (id <= 0)
                    throw new ArgumentException("ID inválido.");

                return await _usuarioRepository.ObtenerPorIdAsync(id);
            }

            public async Task<List<Usuario>> ObtenerUsuariosClientesAsync()
            {
                return await _usuarioRepository.ObtenerUsuariosClientesAsync();
            }

            public async Task<List<Usuario>> ObtenerUsuariosCompradoresAsync()
            {
                return await _usuarioRepository.ObtenerUsuariosCompradoresSP();
            }

           public async Task CrearUsuarioAsync(Usuario usuario)
            {
                if (string.IsNullOrWhiteSpace(usuario.UsuarioLogin) || string.IsNullOrWhiteSpace(usuario.Clave))
                    throw new ArgumentException("UsuarioLogin y Clave son obligatorios.");

                // Hashea la clave ANTES de guardarla
                usuario.Clave = BCrypt.Net.BCrypt.HashPassword(usuario.Clave);

                await _usuarioRepository.CrearUsuarioAsync(usuario);
            }

    }
}
