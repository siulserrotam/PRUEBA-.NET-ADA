using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Models;
using Infraestructure.Interfaces;

namespace Application.Services
{
    public class TransaccionService : ITransaccionService
    {
        private readonly ITransaccionRepository _repo;

        public TransaccionService(ITransaccionRepository repo)
        {
            _repo = repo;
        }

        public async Task RegistrarTransaccionAsync(int usuarioId, int productoId, int cantidad)
        {
            // Puedes incluir aquí validaciones adicionales si es necesario
            await _repo.RegistrarTransaccionSP(usuarioId, productoId, cantidad);
        }

        public async Task<List<Transaccion>> ObtenerHistorialTransaccionesAsync()
        {
            return await _repo.ObtenerHistorialTransaccionesSP();
        }
    }
}
