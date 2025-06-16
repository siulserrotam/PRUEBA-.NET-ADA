using Domain.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Infraestructure.Data
{
    public static class DbInitializer
    {
        public static void Inicializar(AppDbContext context)
        {
            context.Database.Migrate();

            if (!context.Usuarios.Any())
            {
                var admin = new Usuario
                {
                    Nombre = "Admin",
                    Direccion = "Oficina",
                    Telefono = "1234567890",
                    UsuarioLogin = "admin",
                    Identificacion = "1001",
                    Rol = "Administrador",
                    Clave = BCrypt.Net.BCrypt.HashPassword("admin123")
                };

                var cliente = new Usuario
                {
                    Nombre = "Juan Pérez",
                    Direccion = "Calle 123",
                    Telefono = "555123456",
                    UsuarioLogin = "juanp",
                    Identificacion = "1002",
                    Rol = "Cliente",
                    Clave = BCrypt.Net.BCrypt.HashPassword("cliente123")
                };

                context.Usuarios.AddRange(admin, cliente);
                context.SaveChanges();
            }

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
