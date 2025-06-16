using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infraestructure.Interfaces
{
   public interface IUsuarioRepository
    {
        Task<Usuario?> ObtenerPorLoginAsync(string usuarioLogin);
        Task<Usuario?> ObtenerPorIdentificacionAsync(string identificacion);
        Task<Usuario?> ObtenerPorIdAsync(int id);
        Task<List<Usuario>> ObtenerUsuariosClientesAsync();
        Task CrearUsuarioAsync(Usuario usuario);
        Task<List<Usuario>> ObtenerUsuariosCompradoresSP();
    }
}
