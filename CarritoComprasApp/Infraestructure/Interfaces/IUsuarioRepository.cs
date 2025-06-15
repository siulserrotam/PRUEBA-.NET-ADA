using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;

namespace Infraestructure.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObtenerPorCredencialesAsync(string usuarioLogin, string clave);
        Task<Usuario?> ObtenerPorIdentificacionAsync(string identificacion);
        Task<Usuario?> ObtenerPorIdAsync(int id);
        Task<List<Usuario>> ObtenerUsuariosClientesAsync();
        Task CrearUsuarioAsync(Usuario usuario);
        Task<List<Usuario>> ObtenerUsuariosCompradoresSP();
    }
}
