using CarritoCompras.Web.Data;
using CarritoCompras.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CarritoCompras.Web.Controllers
{
    public class CompraController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public CompraController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> Productos()
        {
            var productos = await _context.Productos
                .FromSqlRaw("EXEC sp_ObtenerProductosDisponibles")
                .ToListAsync();

            return View(productos);
        }

        [HttpPost]
        public async Task<IActionResult> Comprar(int productoId, int cantidad)
        {
            string? usuario = HttpContext.Session.GetString("Usuario");

            if (string.IsNullOrEmpty(usuario))
            {
                return RedirectToAction("Index", "Login");
            }

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_InsertarTransaccion @Usuario, @ProductoId, @Cantidad",
                    new SqlParameter("@Usuario", usuario),
                    new SqlParameter("@ProductoId", productoId),
                    new SqlParameter("@Cantidad", cantidad)
                );

                TempData["Success"] = "Compra realizada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "No se pudo realizar la compra: " + ex.Message;
            }

            return RedirectToAction("Productos");
        }
    }
}