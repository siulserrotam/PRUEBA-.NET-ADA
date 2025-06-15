using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Infraestructure.Data
{
    public static class DbInitializer
    {
        public static void Inicializar(AppDbContext context)
        {
            context.Database.Migrate();

            var hasher = new PasswordHasher<Usuario>();

            if (!context.Usuarios.Any())
            {
                var admin = new Usuario
                {
                    Nombre = "Admin",
                    Direccion = "Oficina",
                    Telefono = "1234567890",
                    UsuarioLogin = "admin",
                    Identificacion = "1001",
                    Rol = "Administrador"
                };
                admin.Clave = hasher.HashPassword(admin, "admin123");

                var cliente = new Usuario
                {
                    Nombre = "Juan Pérez",
                    Direccion = "Calle 123",
                    Telefono = "555123456",
                    UsuarioLogin = "juanp",
                    Identificacion = "1002",
                    Rol = "Cliente"
                };
                cliente.Clave = hasher.HashPassword(cliente, "cliente123");

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
