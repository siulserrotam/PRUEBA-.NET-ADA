using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Data
{
    public static class DbInitializer
    {
        public static void Inicializar(AppDbContext context)
        {
            // Aplica migraciones pendientes
            context.Database.Migrate();

            // Verifica si ya existen usuarios
            if (!context.Usuarios.Any())
            {
                var usuarios = new List<Usuario>
                {
                    new Usuario
                    {
                        Nombres = "Admin",
                        Direccion = "Oficina",
                        Telefono = "1234567890",
                        UsuarioLogin = "admin",
                        Identificacion = "1001",
                        Clave = "admin123", // Debes hashearla si usas seguridad real
                        Rol = "Administrador"
                    },
                    new Usuario
                    {
                        Nombres = "Juan Pérez",
                        Direccion = "Calle 123",
                        Telefono = "555123456",
                        UsuarioLogin = "juanp",
                        Identificacion = "1002",
                        Clave = "cliente123",
                        Rol = "Cliente"
                    }
                };
                context.Usuarios.AddRange(usuarios);
                context.SaveChanges();
            }

            // Verifica si ya existen productos
            if (!context.Productos.Any())
            {
                var productos = new List<Producto>
                {
                    new Producto { Nombre = "Laptop", Descripcion = "Laptop básica", CantidadDisponible = 10 },
                    new Producto { Nombre = "Mouse", Descripcion = "Mouse inalámbrico", CantidadDisponible = 50 }
                };
                context.Productos.AddRange(productos);
                context.SaveChanges();
            }
        }
    }
}
