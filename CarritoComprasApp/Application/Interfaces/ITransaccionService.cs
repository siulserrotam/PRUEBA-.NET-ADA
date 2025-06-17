using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITransaccionService
    {
        Task RegistrarTransaccionAsync(int usuarioId, int productoId, int cantidad);

        Task<List<Transaccion>> ObtenerHistorialTransaccionesAsync();
    }
}
