using CarritoCompras.Web.Data;
using CarritoCompras.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class TransaccionesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TransaccionesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var transacciones = await _context.Transacciones
            .Include(t => t.Producto)
            .Include(t => t.Usuario)
            .Select(t => new TransaccionDTO
            {
                Id = t.Id,
                UsuarioId = t.Usuario.UsuarioLogin, 
                Producto = t.Producto.Nombre,
                Cantidad = t.Cantidad,
                Fecha = t.Fecha
            })
            .ToListAsync();

        return Ok(transacciones);
    }
}
