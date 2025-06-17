using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITransaccionService
    {
        /// <summary>
        /// Registra una transacción (compra) realizada por un usuario sobre un producto.
        /// </summary>
        /// <param name="usuarioId">ID del usuario que compra</param>
        /// <param name="productoId">ID del producto comprado</param>
        /// <param name="cantidad">Cantidad de productos comprados</param>
        Task RegistrarTransaccionAsync(int usuarioId, int productoId, int cantidad);

        /// <summary>
        /// Obtiene el historial completo de transacciones registradas en el sistema.
        /// </summary>
        /// <returns>Lista de transacciones</returns>
        Task<List<Transaccion>> ObtenerHistorialTransaccionesAsync();
    }
}
