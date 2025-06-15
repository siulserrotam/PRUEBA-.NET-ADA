using Domain.Models;

namespace Application.Interfaces
{
    public interface ITransaccionService
    {
        Task RegistrarTransaccionAsync(int usuarioId, int productoId, int cantidad);
        Task<List<Transaccion>> ObtenerHistorialTransaccionesAsync();
    }
}
