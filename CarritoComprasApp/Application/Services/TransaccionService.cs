using Application.Interfaces;
using Domain.Models;

using Infraestructure.Interfaces;

public class TransaccionService : ITransaccionService
{
    private readonly ITransaccionRepository _repo;

    public TransaccionService(ITransaccionRepository repo)
    {
        _repo = repo;
    }

    public async Task RegistrarTransaccionAsync(int usuarioId, int productoId, int cantidad)
    {
        // Lógica para registrar la transacción, validaciones, etc.
        await _repo.RegistrarTransaccionSP(usuarioId, productoId, cantidad);
    }

    public async Task<List<Transaccion>> ObtenerHistorialTransaccionesAsync()
    {
        return await _repo.ObtenerHistorialTransaccionesSP();
    }
}
