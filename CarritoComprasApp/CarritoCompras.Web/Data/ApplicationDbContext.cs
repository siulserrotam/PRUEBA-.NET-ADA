using Microsoft.EntityFrameworkCore;
using CarritoCompras.Web.Models;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;

namespace CarritoCompras.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }

        public async Task<List<Producto>> ObtenerProductosDisponiblesAsync()
        {
            return await Productos.FromSqlRaw("EXEC sp_ObtenerProductosDisponibles").ToListAsync();
        }

        public async Task<List<Usuario>> ObtenerUsuariosCompradoresAsync()
        {
            return await Usuarios.FromSqlRaw("EXEC sp_ObtenerUsuariosCompradores").ToListAsync();
        }

        public async Task<List<TransaccionDTO>> ObtenerTransaccionesAsync()
        {
            return await Set<TransaccionDTO>()
                .FromSqlRaw("EXEC sp_ObtenerTransacciones")
                .ToListAsync();
        }

        public async Task ActualizarProductoAsync(int id, int cantidad, string usuario)
        {
            await Database.ExecuteSqlRawAsync("EXEC sp_ActualizarProducto @p0, @p1, @p2", id, cantidad, usuario);
        }

        public async Task InsertarTransaccionAsync(string usuarioId, int productoId, int cantidad)
        {
            await Database.ExecuteSqlRawAsync("EXEC sp_InsertarTransaccion @p0, @p1, @p2", usuarioId, productoId, cantidad);
        }

        public async Task SeedFromJsonAsync(string seedFolder = "seed")
        {
            // Usuarios
            if (!Usuarios.Any())
            {
                var usuariosJson = await File.ReadAllTextAsync(Path.Combine(seedFolder, "usuarios.json"));
                var usuarios = JsonSerializer.Deserialize<List<Usuario>>(usuariosJson);

                if (usuarios != null)
                {
                    foreach (var u in usuarios)
                    {
                        u.Clave = CalcularHash(u.Clave);
                    }

                    Usuarios.AddRange(usuarios);
                    await SaveChangesAsync();
                }
            }

            // Productos
            if (!Productos.Any())
            {
                var productosJson = await File.ReadAllTextAsync(Path.Combine(seedFolder, "productos.json"));
                var productos = JsonSerializer.Deserialize<List<Producto>>(productosJson);

                if (productos != null)
                {
                    Productos.AddRange(productos);
                    await SaveChangesAsync();
                }
            }

            // Transacciones
            if (!Transacciones.Any())
            {
                var transJson = await File.ReadAllTextAsync(Path.Combine(seedFolder, "transacciones.json"));
                var transacciones = JsonSerializer.Deserialize<List<Transaccion>>(transJson);

                if (transacciones != null)
                {
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

        private string CalcularHash(string input)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
