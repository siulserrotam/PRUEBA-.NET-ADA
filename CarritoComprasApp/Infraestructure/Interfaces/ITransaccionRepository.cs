using Domain.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Infraestructure.Interfaces
{
    public interface ITransaccionRepository
    {
        Task RegistrarTransaccionSP(int usuarioId, int productoId, int CantidadDisponible);
        Task<List<Transaccion>> ObtenerHistorialTransaccionesSP();
    }
}
