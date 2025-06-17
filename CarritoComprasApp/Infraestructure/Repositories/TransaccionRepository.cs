using Domain.Models;
using Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infraestructure.Data;

namespace Infraestructure.Repositories
{public class TransaccionRepository : ITransaccionRepository
{
    private readonly AppDbContext _context;

    public TransaccionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task RegistrarTransaccionSP(int usuarioId, int productoId, int cantidad)
    {
        var sql = "EXEC usp_InsertarTransaccion @UsuarioId = {0}, @ProductoId = {1}, @Cantidad = {2}";
        await _context.Database.ExecuteSqlRawAsync(sql, usuarioId, productoId, cantidad);
    }

    public async Task<List<Transaccion>> ObtenerHistorialTransaccionesSP()
    {
        var transacciones = await _context.Transacciones
            .FromSqlRaw("EXEC usp_ObtenerHistorialTransacciones")
            .ToListAsync();

        foreach (var transaccion in transacciones)
        {
            _context.Entry(transaccion).Reference(t => t.Usuario).Load();
            _context.Entry(transaccion).Reference(t => t.Producto).Load();
        }

        return transacciones;
    }
}
}
