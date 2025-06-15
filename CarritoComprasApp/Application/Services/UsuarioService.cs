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
        // private readonly IHashService _hashService; // Si usas un servicio para encriptar claves
        // private readonly ILogger<UsuarioService> _logger; // Si deseas incluir logs

        public UsuarioService(IUsuarioRepository usuarioRepository /*, IHashService hashService, ILogger<UsuarioService> logger */)
        {
            _usuarioRepository = usuarioRepository;
            // _hashService = hashService;
            // _logger = logger;
        }

        public async Task<Usuario?> LoginAsync(string usuarioLogin, string clave)
        {
            if (string.IsNullOrWhiteSpace(usuarioLogin) || string.IsNullOrWhiteSpace(clave))
                throw new ArgumentException("Usuario y clave son obligatorios.");

            // var claveEncriptada = _hashService.HashPassword(clave); // Si aplicas hashing
            return await _usuarioRepository.ObtenerPorCredencialesAsync(usuarioLogin, clave /* o claveEncriptada */);
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

        public async Task CrearUsuarioAsync(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.UsuarioLogin) || string.IsNullOrWhiteSpace(usuario.Clave))
                throw new ArgumentException("UsuarioLogin y Clave son obligatorios.");

            // usuario.Clave = _hashService.HashPassword(usuario.Clave); // Opcional
            await _usuarioRepository.CrearUsuarioAsync(usuario);
        }

        public async Task<List<Usuario>> ObtenerUsuariosCompradoresAsync()
        {
            return await _usuarioRepository.ObtenerUsuariosCompradoresSP();
        }
    }
}
