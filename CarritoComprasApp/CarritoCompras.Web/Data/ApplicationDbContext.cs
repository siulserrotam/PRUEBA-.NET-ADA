using Microsoft.EntityFrameworkCore;
using CarritoCompras.Web.Models;
using System.Text.Json;

namespace CarritoCompras.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }

        // Consultar productos disponibles
        public async Task<List<Producto>> ObtenerProductosDisponiblesAsync()
        {
            return await Productos.FromSqlRaw("EXEC sp_ObtenerProductosDisponibles").ToListAsync();
        }

        // Consultar usuarios compradores
        public async Task<List<Usuario>> ObtenerUsuariosCompradoresAsync()
        {
            return await Usuarios.FromSqlRaw("EXEC sp_ObtenerUsuariosCompradores").ToListAsync();
        }

        // Consultar transacciones
        public async Task<List<TransaccionDTO>> ObtenerTransaccionesAsync()
        {
            return await Set<TransaccionDTO>()
                .FromSqlRaw("EXEC sp_ObtenerTransacciones")
                .ToListAsync();
        }

        // Actualizar producto
        public async Task ActualizarProductoAsync(int id, int cantidad, string usuario)
        {
            await Database.ExecuteSqlRawAsync("EXEC sp_ActualizarProducto @p0, @p1, @p2", id, cantidad, usuario);
        }

        // Insertar transacción
        public async Task InsertarTransaccionAsync(string usuarioId, int productoId, int cantidad)
        {
            await Database.ExecuteSqlRawAsync("EXEC sp_InsertarTransaccion @p0, @p1, @p2", usuarioId, productoId, cantidad);
        }

        // Cargar datos desde archivos JSON
        public async Task SeedFromJsonAsync(string seedFolder = "seed")
        {
            // Seed usuarios
            if (!Usuarios.Any())
            {
                var usuariosJson = await File.ReadAllTextAsync(Path.Combine(seedFolder, "usuarios.json"));
                var usuarios = JsonSerializer.Deserialize<List<Usuario>>(usuariosJson);
                if (usuarios != null)
                    Usuarios.AddRange(usuarios);
            }

            // Seed productos
            if (!Productos.Any())
            {
                var productosJson = await File.ReadAllTextAsync(Path.Combine(seedFolder, "productos.json"));
                var productos = JsonSerializer.Deserialize<List<Producto>>(productosJson);
                if (productos != null)
                    Productos.AddRange(productos);
            }

            await SaveChangesAsync(); // Guarda usuarios y productos antes de relacionar

            // Seed transacciones
            if (!Transacciones.Any())
            {
                var transJson = await File.ReadAllTextAsync(Path.Combine(seedFolder, "transacciones.json"));
                var transacciones = JsonSerializer.Deserialize<List<Transaccion>>(transJson);
                if (transacciones != null)
                {
                    // Establecer relaciones si no están incluidas en el JSON
                    foreach (var t in transacciones)
                    {
                        t.Usuario = await Usuarios.FindAsync(t.UsuarioId);
                        t.Producto = await Productos.FindAsync(t.ProductoId);
                    }

                    Transacciones.AddRange(transacciones);
                    await SaveChangesAsync();
                }
            }
        }
    }
}
