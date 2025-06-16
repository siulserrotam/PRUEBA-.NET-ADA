using Domain.Models;

namespace Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario?> LoginAsync(string usuarioLogin, string clave);

        Task<Usuario?> ObtenerPorIdentificacionAsync(string identificacion);
        Task<Usuario?> ObtenerPorIdAsync(int id);
        Task<List<Usuario>> ObtenerUsuariosClientesAsync();
        Task<List<Usuario>> ObtenerUsuariosCompradoresAsync();
        Task CrearUsuarioAsync(Usuario usuario);
    }
}
